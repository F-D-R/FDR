using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FDR.Tools.Library
{
    public static class Rename
    {
        private const string REGEX = "{([^:}]*):?([^}]*)}";
        private const string NOW = "now";                   // Current date time
        private const string NAME = "name";                 // Name without extension
        private const string PFOLDER = "pfolder";           // Parent folder
        private const string CDATE = "cdate";               // Creation date
        private const string MDATE = "mdate";               // Modify date
        private const string EDATE = "edate";               // EXIF date (=SDATE)
        private const string SDATE = "sdate";               // Shooting date (=EDATE)
        private const string COUNTER = "counter";           // File counter starting with 1

        public static void ShowRenameHelp()
        {
            Common.Msg("");
            Common.Msg("Renames the files matching a filter in the folder given after the -rename option based on a RenameConfig.");
            Common.Msg("");
            Common.Msg("The rename function can have the following attributes in a RenameConfigs object:");
            Common.ShowAttributeHelp(RenameConfig.GetRenameConfigAttributeList());
            Rename.ShowFilenamePatternHelp();
        }

        public static void ShowFilenamePatternHelp()
        {
            Common.Msg("");
            Common.Msg("The FilenamePattern can have the following formatting placeholders:");
            Common.ShowAttributeHelp(GetPlaceholderList(), false);
            Common.Msg("");
            Common.Msg("...see the detailed format description here:");
            Common.Msg("    https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings");
        }

        private static Dictionary<string, string> GetPlaceholderList()
        {
            var list = new Dictionary<string, string>();
            list.Add($"{{{NOW}[:format]}}", "Current date time with optional date format");
            list.Add($"{{{NAME}[:start,length]}}", "Name without extension with optional start index and character length");
            list.Add($"{{{PFOLDER}[:start,length]}}", "Parent folder's name with optional start index and character length");
            list.Add($"{{{CDATE}[:format]}}", "Creation date with optional date format");
            list.Add($"{{{MDATE}[:format]}}", "Modify date with optional date format");
            list.Add($"{{{EDATE}[:format]}}", $"EXIF date (={SDATE}) with optional date format");
            list.Add($"{{{SDATE}[:format]}}", $"Shooting date (={EDATE}) with optional date format");
            list.Add($"{{{COUNTER}[:digits]}}", "File counter with optional number of digits starting with 1 or 'auto'");
            return list;
        }

        public static string EvaluateNamePattern(string pattern, FileSystemInfo? fsi)
        {
            string result = pattern;

            var regex = new Regex(REGEX, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match match in regex.Matches(pattern))
            {
                string arg = match.Groups.Count > 2 ? match.Groups[2].Value : string.Empty;

                switch (match.Groups[1].Value.ToLower())
                {
                    case NAME:
                        if (fsi != null)
                        {
                            var args = arg.Split(",");
                            if (args.Length >= 2)
                                result = result.Replace(match.Value, Path.GetFileNameWithoutExtension(fsi.Name).Substring(int.Parse(args[0]), int.Parse(args[1])), StringComparison.InvariantCultureIgnoreCase);
                            else
                                result = result.Replace(match.Value, Path.GetFileNameWithoutExtension(fsi.Name), StringComparison.InvariantCultureIgnoreCase);
                        }
                        break;

                    case CDATE:
                        if (fsi != null)
                            result = result.Replace(match.Value, fsi.CreationTime.ToString(arg), StringComparison.InvariantCultureIgnoreCase);
                        break;

                    case MDATE:
                        if (fsi != null)
                            result = result.Replace(match.Value, fsi.LastWriteTime.ToString(arg), StringComparison.InvariantCultureIgnoreCase);
                        break;

                    case NOW:
                        result = result.Replace(match.Value, DateTime.Now.ToString(arg), StringComparison.InvariantCultureIgnoreCase);
                        break;
                }
            }

            return result;
        }

        public static string EvaluateFolderNamePattern(string pattern, DirectoryInfo? folder)
        {
            string result = EvaluateNamePattern(pattern, folder);

            var regex = new Regex(REGEX, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match match in regex.Matches(pattern))
            {
                string arg = match.Groups.Count > 2 ? match.Groups[2].Value : string.Empty;

                switch (match.Groups[1].Value.ToLower())
                {
                    case PFOLDER:
                        if (folder != null)
                        {
                            var args = arg.Split(",");
                            if (args.Length >= 2)
                                result = result.Replace(match.Value, folder.Parent?.Name.Substring(int.Parse(args[0]), int.Parse(args[1])), StringComparison.InvariantCultureIgnoreCase);
                            else
                                result = result.Replace(match.Value, folder.Parent?.Name, StringComparison.InvariantCultureIgnoreCase);
                        }
                        break;
                }
            }

            return result;
        }

        public static string EvaluateFileNamePattern(string pattern, FileInfo? file, int counter = 1)
        {
            string result = EvaluateNamePattern(pattern, file);

            var regex = new Regex(REGEX, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match match in regex.Matches(pattern))
            {
                string arg = match.Groups.Count > 2 ? match.Groups[2].Value : string.Empty;

                switch (match.Groups[1].Value.ToLower())
                {
                    case PFOLDER:
                        if (file != null)
                        {
                            var args = arg.Split(",");
                            if (args.Length >= 2)
                                result = result.Replace(match.Value, file.Directory?.Name.Substring(int.Parse(args[0]), int.Parse(args[1])), StringComparison.InvariantCultureIgnoreCase);
                            else
                                result = result.Replace(match.Value, file.Directory?.Name, StringComparison.InvariantCultureIgnoreCase);
                        }
                        break;

                    case EDATE:
                    case SDATE:
                        if (file != null)
                        {
                            var date = Common.GetExifDate(file);
                            result = result.Replace(match.Value, date.ToString(arg), StringComparison.InvariantCultureIgnoreCase);
                        }
                        break;

                    case COUNTER:
                        int digits = 0;
                        int.TryParse(arg, out digits);
                        result = result.Replace(match.Value, string.Format("{0:D" + digits.ToString() + "}", counter), StringComparison.InvariantCultureIgnoreCase);
                        break;
                }
            }

            return result;
        }

        public static void RenameFolder(DirectoryInfo folder, string? pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException("pattern");
            if (folder == null) throw new ArgumentNullException("folder");
            if (!folder.Exists) throw new DirectoryNotFoundException("Folder doesn't exist! " + folder.FullName);

            var path = folder.Parent?.FullName;

            var name = EvaluateFolderNamePattern(pattern, folder);
            folder.MoveTo(Path.Combine(path??"", name));
        }

        public static void RenameFolder(DirectoryInfo folder, RenameConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            RenameFolder(folder, config.FilenamePattern);
        }

        public static string CalculateFileName(FileInfo file, RenameConfig config, int counter = 1)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (file == null) throw new ArgumentNullException("file");
            if (!file.Exists) throw new FileNotFoundException("File doesn't exist!", file.FullName);

            var path = Path.GetDirectoryName(file.FullName)??"";

            var newName = EvaluateFileNamePattern(config.FilenamePattern??"{name}", file, counter);
            if (config.FilenameCase == CharacterCasing.lower)
                newName = newName.ToLower();
            else if (config.FilenameCase == CharacterCasing.upper)
                newName = newName.ToUpper();

            var extension = Path.GetExtension(file.Name);
            if (config.ExtensionCase == CharacterCasing.lower)
                extension = extension.ToLower();
            else if (config.ExtensionCase == CharacterCasing.upper)
                extension = extension.ToUpper();

            return Path.Combine(path, newName + extension);
        }

        public static void RenameFile(FileInfo file, RenameConfig config, int counter = 1, int progressPercent = 0)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (file == null) throw new ArgumentNullException("file");
            if (!file.Exists) throw new FileNotFoundException("File doesn't exist!", file.FullName);

            var sourcePath = Path.GetDirectoryName(file.FullName)??"";
            var origName = file.Name;
            var origNameWithoutExtension = Path.GetFileNameWithoutExtension(origName);
            var newFullName = CalculateFileName(file, config, counter);
            var destPath = Path.GetDirectoryName(newFullName)??"";
            var newName = Path.GetFileName(newFullName);

            if (string.Compare(file.FullName, newFullName, false) != 0)
            {
                var folder = Path.GetDirectoryName(newFullName);
                if (folder != null && !Directory.Exists(folder))
                {
                    Trace.WriteLine($"Creating destination folder {folder}");
                    Directory.CreateDirectory(folder);
                }
#if RELEASE
                Trace.WriteLine($"Moving file {origName} to {newName}");
#else
                Trace.WriteLine($"Moving file {file.FullName} to {newFullName}");
#endif
                file.MoveTo(newFullName);
            }
            else
                Trace.WriteLine($"{origName} matches the new name...");

            Common.Progress(progressPercent);

            if (config.AdditionalFileTypes == null) return;
            foreach (var type in config.AdditionalFileTypes)
            {
                var ext = '.' + type.Trim().TrimStart('*').TrimStart('.');
                origName = origNameWithoutExtension + ext;
                var origPath = Path.Combine(sourcePath, origName);
                if (File.Exists(origPath))
                {
                    if (config.ExtensionCase == CharacterCasing.lower)
                        ext = ext.ToLower();
                    else if (config.ExtensionCase == CharacterCasing.upper)
                        ext = ext.ToUpper();

                    newName = Path.GetFileNameWithoutExtension(newFullName) + ext;
                    var newPath = Path.Combine(destPath, newName);
                    if (string.Compare(origName, newName, false) != 0)
                    {
#if RELEASE
                        Trace.WriteLine($"Moving file {origName} to {newName}");
#else
                        Trace.WriteLine($"Moving file {origPath} to {newPath}");
#endif
                        File.Move(origPath, newPath);
                    }
                    else
                        Trace.WriteLine($"{origName} matches the new name...");
                }
            }
        }

        public static void RenameFilesInFolder(DirectoryInfo folder, RenameConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();
            if (folder == null) throw new ArgumentNullException("folder");
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var filter = config.FileFilter;
            if (string.IsNullOrWhiteSpace(filter)) filter = "*.*";

            Common.Msg($"Renaming {filter} files in {folder.FullName}");
            Trace.Indent();

            var files = Common.GetFiles(folder, filter, config.Recursive).OrderBy(f => f, Common.FileComparer).ToList();
            int fileCount = files.Count;

            var originalPattern = config.FilenamePattern;
            config.FilenamePattern = originalPattern?.Replace($"{{{COUNTER}:auto}}", $"{{{COUNTER}:" + fileCount.ToString().Length + "}");

            int counter = 1;
            Common.Progress(0);
            foreach (var file in files)
            {
                try
                {
                    RenameFile(file, config, counter, 100 * counter / fileCount);
                }
                catch (IOException)
                {
                    if (config.StopOnError) throw;
                }
                counter++;
            }

            config.FilenamePattern = originalPattern;
            Trace.Unindent();
        }
    }
}
