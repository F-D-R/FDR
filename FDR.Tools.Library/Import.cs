using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace FDR.Tools.Library
{
    public enum SourceStructure
    {
        daily_folders,
        monthly_folders,
        mixed_folders
    }

    public class SourceInfo
    {
        public DirectoryInfo DirectoryInfo { get; set; }
        public string Path { get; set; }
        public int SumFileCount { get; set; }
        public int IgnoredFileCount { get; set; }
        public int ImportFileCount { get; set; }
        public string ConfigName { get { return (ImportConfig == null) ? null : ImportConfig.Name; } }
        public ImportConfig ImportConfig { get; set; }
    }

    public static class Import
    {
        public static List<DirectoryInfo> DetectSources()
        {
            var result = new List<DirectoryInfo>();

            var drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                if (drive.IsReady)
                {
                    var root = drive.RootDirectory;
                    var dirs = root.GetDirectories("DCIM", SearchOption.TopDirectoryOnly);
                    if (dirs != null && dirs.Length > 0)
                        result.Add(dirs[0]);
                }
            }

            return result;
        }

        private static string GetVolumeLabel(string path)
        {
            string label = string.Empty;
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

        private static string GetRelativeDestFolder(FolderStructure destStruct, DateTime date, string dateFormat)
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

        private static string GetAbsoluteDestFolder(string destRoot, FolderStructure destStruct, DateTime date, string dateFormat)
        {
            return Path.Combine(destRoot, GetRelativeDestFolder(destStruct, date, dateFormat));
        }

        public static List<FileInfo> GetFiles(DirectoryInfo folder, ImportConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();

            return GetFiles(folder, config.FileFilter);
        }

        public static List<FileInfo> GetFiles(DirectoryInfo folder, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) throw new ArgumentNullException("filter");
            if (folder == null) throw new ArgumentNullException("folder");
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var files = new List<FileInfo>();
            var filters = filter.Split('|');
            foreach (var tmpfilter in filters)
            {
                files.AddRange(folder.GetFiles(tmpfilter, SearchOption.AllDirectories));
            }

            return files.OrderBy(f => f.CreationTimeUtc).ToList();
        }

        private static void CopyFile(string destroot, FileInfo file, FolderStructure deststruct, string dateFormat)
        {
            var destfolder = GetAbsoluteDestFolder(destroot, deststruct, file.CreationTime, dateFormat);
            if (!Directory.Exists(destfolder)) Directory.CreateDirectory(destfolder);

            var dest = Path.Combine(destfolder, file.Name);
            Trace.WriteLine($"Copying {file.FullName} to {dest}");
            file.CopyTo(dest);
        }

        public static void MoveFilesInFolder(DirectoryInfo folder, MoveConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();
            if (folder == null) throw new ArgumentNullException("folder");
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var filter = config.Filter;
            Msg($"Moving {filter} files in {folder.FullName} to {config.RelativeFolder}");
            Trace.Indent();

            if (string.IsNullOrWhiteSpace(filter)) filter = "*.*";
            string[] filters = filter.Split('|');

            var files = new List<FileInfo>();
            foreach (var tmpfilter in filters)
            {
                files.AddRange(folder.GetFiles(tmpfilter));
            }

            var destfolder = Path.Combine(folder.FullName, config.RelativeFolder);
            if (!Directory.Exists(destfolder)) Directory.CreateDirectory(destfolder);

            foreach (var file in files.OrderBy(f => f.CreationTimeUtc).ToList())
            {
                try
                {
                    var destfile = Path.Combine(destfolder, file.Name);
                    Trace.WriteLine($"Moving file {file.Name} to {destfile}");
                    file.MoveTo(destfile);
                }
                catch (IOException)
                {
                    if (config.StopOnError) throw;
                }
            }

            Trace.Unindent();
        }

        public static void ImportFiles(DirectoryInfo source, ImportConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();
            if (source == null) throw new ArgumentNullException("source");
            if (!source.Exists) throw new DirectoryNotFoundException($"Source folder doesn't exist! ({source.FullName})");
            if (string.IsNullOrWhiteSpace(config.DestRoot)) throw new ArgumentNullException("DestRoot");
            if (!Directory.Exists(config.DestRoot)) throw new DirectoryNotFoundException($"Destination root folder doesn't exist! ({config.DestRoot})");

            var files = GetFiles(source, config.FileFilter);

            var foldernames = new List<string>();
            //TODO: exif date...
            var dates = files.Select(f => f.CreationTime.Date).Distinct().OrderBy(d => d).ToList();
            foreach (var date in dates)
            {
                var count = files.Where(f => f.CreationTime.Date == date).Count();

                var destFolder = GetAbsoluteDestFolder(config.DestRoot, config.DestStructure, date, config.DateFormat);
                var rootFolder = Path.GetDirectoryName(destFolder);
                var folderName = Path.GetFileName(destFolder);

                if (Directory.Exists(rootFolder))
                {
                    var childFolders = Directory.GetDirectories(rootFolder, folderName + "*");
                    if (childFolders.Length > 0)
                    {
                        var destFiles = Directory.GetFiles(childFolders[0], "*", SearchOption.AllDirectories);
                        Msg($"{date:yyyy-MM-dd}: Destination exists ({destFiles.Length} files in {childFolders[0]}), ignoring {count} files", ConsoleColor.Yellow);
                        files = files.Where(f => f.CreationTime.Date != date).OrderBy(f => f.CreationTimeUtc).ToList();
                        continue;
                    }
                }

                Msg($"{date:yyyy-MM-dd}: Importing {count} files to {destFolder}");
                foldernames.Add(destFolder);
            }
            Msg("");

            // Copy
            dates = files.Select(f => f.CreationTime.Date).Distinct().OrderBy(d => d).ToList();
            foreach (var date in dates)
            {
                var count = files.Where(f => f.CreationTime.Date == date).Count();
                var folder = GetAbsoluteDestFolder(config.DestRoot, config.DestStructure, date, config.DateFormat);

                Msg($"Copying {count} files to {folder}");
                Trace.Indent();
                files.Where(file => file.CreationTime.Date == date).OrderBy(file => file.CreationTime).ToList().ForEach(file => CopyFile(config.DestRoot, file, config.DestStructure, config.DateFormat));
                Trace.Unindent();
            }

            // Rename
            if (config.RenameConfigs != null)
                foreach (var fn in foldernames)
                    foreach (var rc in config.RenameConfigs)
                        Rename.RenameFilesInFolder(new DirectoryInfo(fn), rc);

            // Move
            if (config.MoveConfigs != null)
                foreach (var fn in foldernames)
                    foreach (var mc in config.MoveConfigs)
                        MoveFilesInFolder(new DirectoryInfo(fn), mc);
        }

        private static ImportConfig FindConfig(string sourcePath, ImportConfig[] configs)
        {
            var matchingConfigs = new List<ImportConfig>();

            foreach (var config in configs)
            {
                if (config.Rules != null && config.Rules.Length > 0)
                {
                    ImportConfig tmpconfig = null;
                    bool ok = true;

                    foreach (var rule in config.Rules)
                    {
                        switch (rule.Type)
                        {
                            case RuleType.volume_label:
                                if (string.Compare(GetVolumeLabel(sourcePath), rule.Param, true) == 0)
                                    tmpconfig = config;
                                else
                                    ok = false;
                                break;

                            case RuleType.contains_file:
                                if (Directory.GetFiles(sourcePath, rule.Param, SearchOption.AllDirectories).Any())
                                    tmpconfig = config;
                                else
                                    ok = false;
                                break;

                            case RuleType.contains_folder:
                                if (Directory.GetDirectories(sourcePath, rule.Param, SearchOption.AllDirectories).Any())
                                    tmpconfig = config;
                                else
                                    ok = false;
                                break;

                            default:
                                throw new NotImplementedException($"Unknown rule type: {rule.Type}");
                        }
                    }

                    if (ok && tmpconfig != null && !matchingConfigs.Contains(tmpconfig))
                        matchingConfigs.Add(tmpconfig);
                }
            }

            if (matchingConfigs.Count == 1) return matchingConfigs[0];
            return null;
        }

        public static void ImportWizard(ImportConfig[] configs, bool auto = false)
        {
            if (configs == null) throw new ArgumentNullException("configs");
            if (configs.Length == 0) throw new ArgumentNullException("Import configurations cannot be empty!");
            foreach (var c in configs) c.Validate();

            Msg("Import");

            var sources = DetectSources();

            var sourceInfos = new List<SourceInfo>();
            foreach (var source in sources)
            {
                var si = new SourceInfo();
                si.DirectoryInfo = source;
                si.Path = source.FullName;
                si.ImportConfig = FindConfig(source.FullName, configs);
                if (si.ImportConfig != null)
                    si.SumFileCount = GetFiles(source, si.ImportConfig).Count();
                sourceInfos.Add(si);
            }

            SourceInfo selectedSI = null;
            if (sourceInfos.Count == 0)
            {
                Msg("No source has been found!", ConsoleColor.Red);
                return;
            }
            else if (auto && sourceInfos.Count == 1)
            {
                selectedSI = sourceInfos[0];
            }
            else
            {
                Msg("Select source:");
                int i = 1;
                foreach (var si in sourceInfos)
                {
                    if (si.ImportConfig == null)
                        Msg($"    {i}. {si.Path}\tNo configuration for drive {GetVolumeLabel(si.Path)}");
                    else
                        Msg($"    {i}. {si.Path}\tConfig: {si.ConfigName} ({si.SumFileCount} files on drive {GetVolumeLabel(si.Path)})");
                    i++;
                }
                Msg($"Enter the number or the source (1..{i - 1}) or ESC to abort: ", ConsoleColor.White, false);

                while (selectedSI == null)
                {
                    while (!Console.KeyAvailable) Thread.Sleep(300);
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)
                    {
                        Msg("");
                        Msg("Import aborted...", ConsoleColor.Red);
                        return;
                    }

                    int selection = 0;
                    if (int.TryParse(key.KeyChar.ToString(), out selection) && selection < i)
                    {
                        Msg($"{key.KeyChar}");
                        selectedSI = sourceInfos[selection - 1];
                    }
                }
            }
            if (selectedSI == null) return;

            if (selectedSI.ImportConfig == null)
                Msg($"Selected source: {selectedSI.Path}\tNo configuration for drive {GetVolumeLabel(selectedSI.Path)}");
            else
                Msg($"Selected source: {selectedSI.Path}\tConfig: {selectedSI.ConfigName} ({selectedSI.SumFileCount} files on drive {GetVolumeLabel(selectedSI.Path)})");

            var config = selectedSI.ImportConfig;

            if (config == null)
            {
                Msg("Select configuration:");
                int i = 1;
                foreach (var c in configs)
                {
                    Msg($"    {i}. {c.Name} ({c.DestRoot})");
                    i++;
                }
                Msg($"Enter the number or the configuration (1..{i - 1}) or ESC to abort: ", ConsoleColor.White, false);

                while (config == null)
                {
                    while (!Console.KeyAvailable) Thread.Sleep(300);
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)
                    {
                        Msg("");
                        Msg("Import aborted...", ConsoleColor.Red);
                        return;
                    }

                    int selection = 0;
                    if (int.TryParse(key.KeyChar.ToString(), out selection) && selection < i)
                    {
                        Msg($"{key.KeyChar}");
                        config = configs[selection - 1];
                        selectedSI.ImportConfig = config;
                    }
                }

                if (config != null)
                {
                    selectedSI.SumFileCount = GetFiles(selectedSI.DirectoryInfo, config).Count();
                    Msg($"Selected source: {selectedSI.Path}\tConfig: {selectedSI.ConfigName} ({selectedSI.SumFileCount} files on drive {GetVolumeLabel(selectedSI.Path)})");
                }
            }
            if (config == null) return;

            Trace.Indent();
            Trace.WriteLine($"DestRoot: {config.DestRoot}");
            Trace.WriteLine($"DestStructure: {config.DestStructure}");
            Trace.WriteLine($"DateFormat: {config.DateFormat}");
            Trace.WriteLine($"FileFilter: {config.FileFilter}");
            if (config.RenameConfigs != null)
                foreach (var rc in config.RenameConfigs)
                    Trace.WriteLine($"Rename: {rc.Filter} to {rc.FilenamePattern} ({rc.FilenameCase}.{rc.ExtensionCase})");
            if (config.MoveConfigs != null)
                foreach (var mc in config.MoveConfigs)
                    Trace.WriteLine($"Move: {mc.Filter} to {mc.RelativeFolder}");
            Trace.WriteLine("");
            Trace.Unindent();

            Import.ImportFiles(selectedSI.DirectoryInfo, config);

            Msg("");
            Msg("Successfully finished...", ConsoleColor.Green);
        }

        private static void Msg(string msg, ConsoleColor color = ConsoleColor.White, bool newline = true)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = color;
            if (newline)
                Console.WriteLine("\r" + msg);
            else
                Console.Write("\r" + msg);
            Console.ResetColor();
        }

    }
}
