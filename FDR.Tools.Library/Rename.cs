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
            Common.ShowAttributeHelp(GetRenamePlaceholderList(), false);
            Common.Msg("");
            Common.Msg("...see the detailed format description here:");
            Common.Msg("    https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings");
        }

        public static Dictionary<string, string> GetRenamePlaceholderList()
        {
            var list = new Dictionary<string, string>()
            {
                { $"{{{NAME}}}", "Name without extension" },
                { $"{{{NAME}:start,length}}", "Name without extension with start index and character length" },
                { $"{{{PFOLDER}}}", "Parent folder's name" },
                { $"{{{PFOLDER}:start,length}}", "Parent folder's name with start index and character length" },
                { $"{{{NOW}}}", "Current date time" },
                { $"{{{NOW}:format}}", "Current date time with custom date format" },
                { $"{{{CDATE}}}", "Creation date" },
                { $"{{{CDATE}:format}}", "Creation date with custom date format" },
                { $"{{{MDATE}}}", "Modify date" },
                { $"{{{MDATE}:format}}", "Modify date with custom date format" },
                { $"{{{EDATE}}}", $"EXIF date (={SDATE})" },
                { $"{{{EDATE}:format}}", $"EXIF date (={SDATE}) with custom date format" },
                { $"{{{SDATE}}}", $"Shooting date (={EDATE})" },
                { $"{{{SDATE}:format}}", $"Shooting date (={EDATE}) with custom date format" },
                { $"{{{COUNTER}}}", "File counter" },
                { $"{{{COUNTER}:digits}}", "File counter with custom number of digits or 'auto'" }
            };
            return list;
        }

        public static Dictionary<string, string> GetDateFormatters()
        {
            var list = new Dictionary<string, string>()
            {
                { "d", "Day of the month, from 1 to 31" },
                { "dd", "Day of the month, from 01 to 31" },
                { "ddd", "Abbreviated name of the day of the week" },
                { "dddd", "Full name of the day of the week" },
                { "h", "Hour, using a 12-hour clock from 1 to 12" },
                { "hh", "Hour, using a 12-hour clock from 01 to 12" },
                { "H", "Hour, using a 24-hour clock from 0 to 23" },
                { "HH", "Hour, using a 24-hour clock from 00 to 23" },
                { "K", "Time zone information" },
                { "m", "Minute, from 0 to 59" },
                { "mm", "Minute, from 00 to 59" },
                { "M", "Month, from 1 to 12" },
                { "MM", "Month, from 01 to 12" },
                { "MMM", "Abbreviated name of the month" },
                { "MMMM", "Full name of the month" },
                { "s", "Second, from 0 to 59" },
                { "ss", "Second, from 00 to 59" },
                { "t", "First character of the AM/PM designator" },
                { "tt", "AM/PM designator" },
                { "y", "Year, from 0 to 99" },
                { "yy", "Year, from 00 to 99" },
                { "yyyy", "Year as a four-digit number" },
                { "z", "Hours offset from UTC, with no leading zeros" },
                { "zz", "Hours offset from UTC, with a leading zero for a single-digit value" },
                { "zzz", "Hours and minutes offset from UTC" },
                { ":", "Time separator" },
                { "/", "Date separator" },
                { "\\", "Escape character" }
            };
            return list;
        }

        public static string EvaluateNamePattern(string pattern, FileSystemInfo? fsi)
        {
            string result = pattern;

            var regex = new Regex(REGEX, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match match in regex.Matches(pattern).Cast<Match>())
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
            foreach (Match match in regex.Matches(pattern).Cast<Match>())
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
            foreach (Match match in regex.Matches(pattern).Cast<Match>())
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
                        _=int.TryParse(arg, out int digits);
                        result = result.Replace(match.Value, string.Format("{0:D" + digits.ToString() + "}", counter), StringComparison.InvariantCultureIgnoreCase);
                        break;
                }
            }

            return result;
        }

        public static void RenameFolder(DirectoryInfo folder, string? pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException(nameof(pattern));
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            if (!folder.Exists) throw new DirectoryNotFoundException("Folder doesn't exist! " + folder.FullName);

            var path = folder.Parent?.FullName;

            var name = EvaluateFolderNamePattern(pattern, folder);
            folder.MoveTo(Path.Combine(path??"", name));
        }

        public static void RenameFolder(DirectoryInfo folder, RenameConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            RenameFolder(folder, config.FilenamePattern);
        }

        public static string CalculateFileName(FileInfo file, RenameConfig config, int counter = 1)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (file == null) throw new ArgumentNullException(nameof(file));
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

        private static List<FileInfo>? GetSameNamedFiles(FileInfo file)
        {
            return Common.GetFiles(file.Directory!, Path.GetFileNameWithoutExtension(file.FullName) + ".*", false).ToList();
        }

        public static void RenameFile(FileInfo file, RenameConfig config, ref int counter, int progressPercent)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (!File.Exists(file.FullName)) return;    //file.Exists wouldn't work here!

            var origName = file.Name;
            var newFullName = CalculateFileName(file, config, counter);
            var newName = Path.GetFileName(newFullName);
            string destDir = Path.GetDirectoryName(newFullName)??"";

            if (string.Compare(file.FullName, newFullName, false) == 0)
            {
                Trace.WriteLine($"{file.FullName} matches the new name...");
            }
            else
            {
                List<FileInfo>? files = null;
                if (config.AdditionalFiles)
                    files = GetSameNamedFiles(file);
                else
                    files = new List<FileInfo>() { file };

                //Get the oldest file and rename all according to that
                if (files != null && files.Count > 0)
                {
                    var oldestFile = files.OrderBy(f => f.GetExifDate()).First();
                    newFullName = CalculateFileName(oldestFile, config, counter);
                    newName = Path.GetFileNameWithoutExtension(newFullName);

                    files.ForEach(f => Rename(f, destDir, newName));

                    counter++;
                }
            }

            Common.Progress(progressPercent);
            return;

            void Rename(FileInfo file, string destDir, string newName)
            {
                var ext = file.Extension;
                if (config.ExtensionCase == CharacterCasing.lower)
                    ext = ext.ToLower();
                if (config.ExtensionCase == CharacterCasing.upper)
                    ext = ext.ToUpper();

                var destFile = newName + ext;
                var destPath = Path.Combine(destDir, destFile);

                if (string.Compare(file.FullName, destPath, false) != 0)
                {
                    var destFolder = Path.GetDirectoryName(destPath);
                    if (destFolder != null && !Directory.Exists(destFolder))
                    {
                        Trace.WriteLine($"Creating destination folder {destFolder}");
                        Directory.CreateDirectory(destFolder);
                    }

#if RELEASE
                    Trace.WriteLine($"Moving file {file.Name} to {destFile}");
#else
                    Trace.WriteLine($"Moving file {file.FullName} to {destPath}");
#endif
                    File.Move(file.FullName, destPath);
                }
                else
                    Trace.WriteLine($"{file.Name} matches the new name...");
            }
        }

        public static void RenameFilesInFolder(DirectoryInfo folder, RenameConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            config.Validate();
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var filter = config.FileFilter;
            if (string.IsNullOrWhiteSpace(filter)) filter = "*.*";

            Common.Msg($"Renaming {filter} files in {folder.FullName}");
            Trace.Indent();

            var files = Common.GetFiles(folder, filter, config.Recursive).OrderBy(f => f, Common.FileComparer).ThenBy(f => f.Name).ToList();
            int fileCount = files.Count;

            var originalPattern = config.FilenamePattern;
            config.FilenamePattern = originalPattern?.Replace($"{{{COUNTER}:auto}}", $"{{{COUNTER}:" + fileCount.ToString().Length + "}");

            int counter = 1;
            Common.Progress(0);
            foreach (var file in files)
            {
                try
                {
                    RenameFile(file, config, ref counter, 100 * counter / fileCount);
                }
                catch (Exception)
                {
                    if (config.StopOnError) throw;
                }
            }

            config.FilenamePattern = originalPattern;
            Trace.Unindent();
        }
    }
}
