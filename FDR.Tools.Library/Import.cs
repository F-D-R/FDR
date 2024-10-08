﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FDR.Tools.Library
{
    public static class Import
    {
        public static void ShowImportHelp()
        {
            Common.Msg("");
            Common.Msg("Imports memory card content based on the selected configuration.");
            Common.Msg("");
            Common.Msg("An import configuration has the following attributes in an ImportConfigs list:");
            Common.ShowAttributeHelp(ImportConfig.GetImportConfigAttributeList());
        }

        public static void ShowMoveHelp()
        {
            Common.Msg("");
            Common.Msg("Moves the files matching a filter in the folder given after the -move option based on a MoveConfig.");
            Common.Msg("");
            Common.Msg("The move function can have the following attributes in a MoveConfigs object:");
            Common.ShowAttributeHelp(MoveConfig.GetMoveConfigAttributeList());
            Rename.ShowFilenamePatternHelp();
        }

        internal class SourceInfo
        {
            public DirectoryInfo? DirectoryInfo { get; set; }
            public string? Path { get; set; }
            public int SumFileCount { get; set; }
            public int IgnoredFileCount { get; set; }
            public int ImportFileCount { get; set; }
            public string? ConfigName { get { return ImportConfig?.Name; } }
            public ImportConfig? ImportConfig { get; set; }
        }

        public static List<DirectoryInfo> DetectSources()
        {
            var result = new List<DirectoryInfo>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    var dir = drive.RootDirectory.GetDirectories("DCIM", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    if (dir != null) result.Add(dir);
                }
            }
            return result;
        }

        internal static string GetVolumeLabel(string path)
        {
            if (Path.IsPathRooted(path))
            {
                var root = Path.GetPathRoot(path);
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.RootDirectory.FullName == root)
                        return drive.VolumeLabel;
                }
            }
            return "<unknown>";
        }

        internal static string GetRelativeDestFolder(FolderStructure destStruct, DateTime date, string dateFormat)
        {
            var sep = Path.DirectorySeparatorChar;
            var dateString = date.ToString(dateFormat);
            switch (destStruct)
            {
                case FolderStructure.date: return $"{dateString}";
                case FolderStructure.year_date: return $"{date:yyyy}{sep}{dateString}";
                case FolderStructure.year_month_date: return $"{date:yyyy}{sep}{date:MM}{sep}{dateString}";
                case FolderStructure.year_month_day: return $"{date:yyyy}{sep}{date:MM}{sep}{date:dd}";
                case FolderStructure.year_month: return $"{date:yyyy}{sep}{date:MM}";
                default:
                    throw new NotImplementedException($"Unhandled destination structure: {destStruct}");
            }
        }

        internal static string GetAbsoluteDestFolder(string destRoot, FolderStructure destStruct, DateTime date, string dateFormat)
        {
            return Path.Combine(destRoot, GetRelativeDestFolder(destStruct, date, dateFormat));
        }

        internal static void ImportFile(string destRoot, ExifFile file, FolderStructure destStruct, string dateFormat, int progressPercent)
        {
            //TODO: configurable dest folder structure date source
            var destfolder = GetAbsoluteDestFolder(destRoot, destStruct, file.ExifTime, dateFormat);
            if (!Directory.Exists(destfolder)) Directory.CreateDirectory(destfolder);

            var dest = Path.Combine(destfolder, file.FileInfo?.Directory?.Name + "_" + file.Name);
            Trace.WriteLine($"Copying {file.FullName} to {dest}");
            Common.Progress(progressPercent);
            file.CopyAndSwitchTo(dest);
            File.SetCreationTimeUtc(dest, file.CreationTimeUtc);
            File.SetLastWriteTimeUtc(dest, file.LastWriteTimeUtc);
        }

        public static void ImportFiles(DirectoryInfo source, ImportConfig config, bool force, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(config);
            config.Validate();
            ArgumentNullException.ThrowIfNull(source);
            if (!source.Exists) throw new DirectoryNotFoundException($"Source folder doesn't exist! ({source.FullName})");
            if (string.IsNullOrWhiteSpace(config.DestRoot)) throw new ArgumentNullException(nameof(config.DestRoot));
            if (!Directory.Exists(config.DestRoot)) throw new DirectoryNotFoundException($"Destination root folder doesn't exist! ({config.DestRoot})");

            var folderNames = new List<string>();

            var files = Common.GetFilesWithOutput(source, config.FileFilter, true);
            var fileCount = files.Count;
            int i = 0;

            //TODO: configurable dest folder structure date source
            //TODO: exif loading only if necessary, i.e. date source is edate

            //Parallel exif loading
            using (new TimedScope($"Loading EXIF date of {fileCount} files...", $"Loaded EXIF date of {fileCount} files."))
            {
                i = 0;
                DateTime dummy;
                Common.Progress(0);
                Trace.Indent();
                ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 8 };
                Parallel.ForEach(files, parallelOptions, file =>
                {
                    i++;
                    dummy = file.ExifTime;
                    Common.Progress(100 * i / fileCount);
                });
                Trace.Unindent();
            }

            //TODO: configurable file ordering
            //TODO: monthly breaking (actions are run daily, renaming the files multiple times and collide)
            var dates = files.Select(f => f.ExifTime.Date).Distinct().OrderBy(d => d).ToList();
            foreach (var date in dates)
            {
                var count = files.Where(f => f.ExifTime.Date == date).Count();

                var destFolder = GetAbsoluteDestFolder(config.DestRoot, config.DestStructure, date, config.DateFormat);
                var rootFolder = Path.GetDirectoryName(destFolder);
                var folderName = Path.GetFileName(destFolder);

                if (!force && Directory.Exists(rootFolder))
                {
                    var childFolders = Directory.GetDirectories(rootFolder, folderName + "*");
                    if (childFolders.Length > 0)
                    {
                        var destFiles = Directory.GetFiles(childFolders[0], "*");
                        Common.Msg($"{date:yyyy-MM-dd}: Destination exists ({destFiles.Length} files in {childFolders[0]}), ignoring {count} files", ConsoleColor.Yellow);
                        files = files.Where(f => f.ExifTime.Date != date).OrderBy(f => f.ExifTime).ToList();
                        continue;
                    }
                }

                Common.Msg($"{date:yyyy-MM-dd}: Importing {count} files to {destFolder}");
                folderNames.Add(destFolder);
            }

            // Copy
            dates = files.Select(f => f.ExifTime.Date).Distinct().OrderBy(d => d).ToList();
            i = 0;
            Common.Progress(0);
            foreach (var date in dates)
            {
                var count = files.Where(f => f.ExifTime.Date == date).Count();
                var folder = GetAbsoluteDestFolder(config.DestRoot, config.DestStructure, date, config.DateFormat);

                using (new TimedScope($"Copying {count} files to {folder}", $"Copied {count} files to {folder}"))
                {
                    Trace.Indent();
                    foreach (var file in files.Where(f => f.ExifTime.Date == date).OrderBy(f => f.ExifTime))
                    {
                        ImportFile(config.DestRoot, file, config.DestStructure, config.DateFormat, 100 * i / fileCount);
                        i++;
                    }
                    Trace.Unindent();
                }
            }

            // Actions
            if (config.Actions != null)
                foreach (var fn in folderNames)
                    foreach (var a in config.Actions)
                        a.Do(new DirectoryInfo(fn), token, files);
        }

        internal static ImportConfig? FindConfig(DirectoryInfo sourceFolder, Dictionary<string, ImportConfig> configs)
        {
            return configs.Where(c => c.Value.Rules.Evaluate(sourceFolder)).FirstOrDefault().Value;
        }

        private static List<DirectoryInfo> GetSourceFolders(DirectoryInfo? givenFolder)
        {
            if (givenFolder != null)
            {
                Common.Msg($"Import ({givenFolder.FullName})");
                if (givenFolder.Name != "DCIM") throw new ArgumentException("The given import folder is not a DCIM folder!");
                return new List<DirectoryInfo>() { givenFolder };
            }

            Common.Msg("Import");
            return DetectSources();
        }

        private static List<SourceInfo> GetSourceInfos(Dictionary<string, ImportConfig> configs, List<DirectoryInfo> sourceFolders)
        {
            var sourceInfos = new List<SourceInfo>();
            foreach (var folder in sourceFolders)
            {
                var si = new SourceInfo()
                {
                    DirectoryInfo = folder,
                    Path = folder.FullName,
                    ImportConfig = FindConfig(folder, configs)
                };

                if (si.ImportConfig != null)
                    si.SumFileCount = Common.GetFiles(folder, si.ImportConfig).Count;
                sourceInfos.Add(si);
            }
            return sourceInfos;
        }

        private static SourceInfo? SelectSourceInfo(List<SourceInfo> sourceInfos, bool auto)
        {
            if (sourceInfos.Count == 0)
            {
                Common.Msg("No source has been found!", ConsoleColor.Red);
                return null;
            }

            if (auto && sourceInfos.Count == 1)
                return sourceInfos[0];

            Common.Msg("Select source:");
            int i = 1;
            foreach (var si in sourceInfos)
            {
                if (si.ImportConfig == null)
                    Common.Msg($"    {i}. {si.Path}\tNo configuration for drive {GetVolumeLabel(si.Path!)}");
                else
                    Common.Msg($"    {i}. {si.Path}\tConfig: {si.ConfigName} ({si.SumFileCount} files on drive {GetVolumeLabel(si.Path!)})");
                i++;
            }
            Common.Msg($"Enter the number or the source (1..{i - 1}) or ESC to abort: ", ConsoleColor.White, false);

            while (true)
            {
                while (!Console.KeyAvailable) Thread.Sleep(300);
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    Common.Msg("");
                    Common.Msg("Import aborted...", ConsoleColor.Red);
                    return null;
                }

                if (int.TryParse(key.KeyChar.ToString(), out int selection) && selection > 0 && selection < i)
                {
                    Common.Msg($"{key.KeyChar}");
                    return sourceInfos[selection - 1];
                }
            }
        }

        private static ImportConfig? SelectImportConfig(Dictionary<string, ImportConfig> configs, SourceInfo sourceInfo)
        {
            if (sourceInfo.ImportConfig == null)
                Common.Msg($"Selected source: {sourceInfo.Path}\tNo configuration for drive {GetVolumeLabel(sourceInfo.Path!)}");
            else
                Common.Msg($"Selected source: {sourceInfo.Path}\tConfig: {sourceInfo.ConfigName} ({sourceInfo.SumFileCount} files on drive {GetVolumeLabel(sourceInfo.Path!)})");

            Common.Msg("Select configuration:");
            int i = 1;
            foreach (var c in configs)
            {
                Common.Msg($"    {i}. {c.Value.Name} ({c.Value.DestRoot})");
                i++;
            }
            Common.Msg($"Enter the number or the configuration (1..{i - 1}) or ESC to abort: ", ConsoleColor.White, false);

            ImportConfig? config = null;
            while (config == null)
            {
                while (!Console.KeyAvailable) Thread.Sleep(300);
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    Common.Msg("");
                    Common.Msg("Import aborted...", ConsoleColor.Red);
                    return null;
                }

                if (int.TryParse(key.KeyChar.ToString(), out int selection) && selection > 0 && selection < i)
                {
                    Common.Msg($"{key.KeyChar}");
                    config = configs.ToArray().ElementAt(selection - 1).Value;
                    sourceInfo.ImportConfig = config;
                }
            }

            sourceInfo.SumFileCount = Common.GetFiles(sourceInfo.DirectoryInfo!, config).Count;
            Common.Msg($"Selected source: {sourceInfo.Path}\tConfig: {sourceInfo.ConfigName} ({sourceInfo.SumFileCount} files on drive {GetVolumeLabel(sourceInfo.Path!)})");
            return config;
        }

        private static void PrintImportConfiguration(ImportConfig config)
        {
            Trace.Indent();
            Trace.WriteLine($"DestRoot: {config.DestRoot}");
            Trace.WriteLine($"DestStructure: {config.DestStructure}");
            Trace.WriteLine($"DateFormat: {config.DateFormat}");
            Trace.WriteLine($"FileFilter: {config.FileFilter}");
            if (config.Actions != null)
                foreach (var a in config.Actions)
                    Trace.WriteLine($"Action: {a.Type} - {a.Config}");

            Trace.WriteLine("");
            Trace.Unindent();
        }

        public static void ImportWizard(Dictionary<string, ImportConfig> configs, DirectoryInfo? folder = null, bool auto = false, bool noactions = false, bool force = false, CancellationToken token = new())
        {
            ArgumentNullException.ThrowIfNull(configs);
            if (configs.Count == 0) throw new ArgumentNullException("Import configurations cannot be empty!");
            foreach (var c in configs) c.Value.Validate();

            var sourceFolders = GetSourceFolders(folder);
            var sourceInfos = GetSourceInfos(configs, sourceFolders);
            var selectedSourceInfo = SelectSourceInfo(sourceInfos, auto);
            if (selectedSourceInfo == null || selectedSourceInfo.DirectoryInfo == null) return;
            var config = SelectImportConfig(configs, selectedSourceInfo);
            if (config == null) return;
            if (noactions) config.Actions?.Clear();
            PrintImportConfiguration(config);

            ImportFiles(selectedSourceInfo.DirectoryInfo, config, force, token);

            Common.Msg("                        ");
            Common.Msg("Successfully finished...", ConsoleColor.Green);
        }
    }
}
