using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public static string EvaluateFileNamePattern(string pattern, ExifFile? file, int counter = 1)
        {
            string result = pattern;

            var regex = new Regex(REGEX, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match match in regex.Matches(pattern).Cast<Match>())
            {
                string arg = match.Groups.Count > 2 ? match.Groups[2].Value : string.Empty;

                switch (match.Groups[1].Value.ToLower())
                {
                    case EDATE:
                    case SDATE:
                        if (file != null)
                        {
                            var date = file.ExifTime;
                            result = result.Replace(match.Value, date.ToString(arg), StringComparison.InvariantCultureIgnoreCase);
                        }
                        break;
                }
            }

            return EvaluateFileNamePattern(result, file?.FileInfo, counter);
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
            //if (config == null) throw new ArgumentNullException(nameof(config));
            if (file == null) throw new ArgumentNullException(nameof(file));
            //if (!file.Exists) throw new FileNotFoundException("File doesn't exist!", file.FullName);

            //var path = Path.GetDirectoryName(file.FullName)??"";

            //var newName = EvaluateFileNamePattern(config.FilenamePattern??"{name}", file, counter);
            //if (config.FilenameCase == CharacterCasing.lower)
            //    newName = newName.ToLower();
            //else if (config.FilenameCase == CharacterCasing.upper)
            //    newName = newName.ToUpper();

            //var extension = Path.GetExtension(file.Name);
            //if (config.ExtensionCase == CharacterCasing.lower)
            //    extension = extension.ToLower();
            //else if (config.ExtensionCase == CharacterCasing.upper)
            //    extension = extension.ToUpper();

            //return Path.Combine(path, newName + extension);

            return CalculateFileName(new ExifFile(file), config, counter);
        }

        public static string CalculateFileName(ExifFile file, RenameConfig config, int counter = 1)
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
                else if (config.ExtensionCase == CharacterCasing.upper)
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

        public static void RenameFile(ExifFile file, RenameConfig config, ref int counter, int progressPercent, List<ExifFile>? fileCache = null)
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
                List<ExifFile>? files = null;
                if (config.AdditionalFiles)
                {
                    if (fileCache == null)
                    {
                        files = GetSameNamedFiles(file.FileInfo)?.Select(fi => new ExifFile(fi)).ToList();
                    }
                    else
                    {
                        var fileStart = Path.Combine(Path.GetDirectoryName(file.OriginalFullName)!, Path.GetFileNameWithoutExtension(file.OriginalName) + ".");
                        Trace.WriteLine($"Look for additional files starting with: {fileStart}");
                        files = fileCache.Where(f => f.FullName.StartsWith(fileStart)).ToList();
                    }
                }
                else
                    files = new List<ExifFile>() { file };

                //Get the oldest file and rename all according to that
                if (files != null && files.Count > 0)
                {
                    var oldestFile = files.OrderBy(f => f.ExifTime).First();
                    newFullName = CalculateFileName(oldestFile, config, counter);
                    newName = Path.GetFileNameWithoutExtension(newFullName);

                    files.ForEach(f => Rename(f, destDir, newName));

                    counter++;
                }
            }

            Common.Progress(progressPercent);
            return;

            void Rename(ExifFile file, string destDir, string newName)
            {
                var ext = file.Extension;
                if (config.ExtensionCase == CharacterCasing.lower)
                    ext = ext.ToLower();
                else if (config.ExtensionCase == CharacterCasing.upper)
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
                    file.MoveTo(destPath);
                }
                else
                    Trace.WriteLine($"{file.Name} matches the new name...");
            }
        }

        public static void CalculateNewLocation(List<ExifFile> allFiles, ExifFile file, RenameConfig config, ref int counter, int progressPercent)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (!string.IsNullOrWhiteSpace(file.NewLocation)) return;   //file already has new location
            if (!File.Exists(file.FullName)) return;    //file.Exists wouldn't work here!

            string origName = file.Name;
            string newFullName = CalculateFileName(file, config, counter);
            string newName = Path.GetFileName(newFullName);
            string newDir = Path.GetDirectoryName(newFullName)??"";

            if (string.Compare(file.FullName, newFullName, false) == 0)
            {
                Trace.WriteLine($"{file.FullName} matches the new name...");
            }
            else
            {
                if (config.AdditionalFiles)
                {
                    string fileStart = Path.Combine(Path.GetDirectoryName(file.OriginalFullName)!, Path.GetFileNameWithoutExtension(file.OriginalName) + ".");
                    Trace.WriteLine($"Look for additional files starting with: {fileStart}");
                    var files = allFiles.Where(f => f.FullName.StartsWith(fileStart)).ToList();

                    var oldestFile = files.OrderBy(f => f.ExifTime).First();
                    newFullName = CalculateFileName(oldestFile, config, counter);
                    newName = Path.GetFileNameWithoutExtension(newFullName);

                    files.ForEach(f => SetNewLocation(f, newDir, newName));
                }
                else
                {
                    //SetNewLocation(file, newDir, newName);
                    Trace.WriteLine($"New location for file {file.Name} is {newName}");
                    file.NewLocation = newFullName;
                }

                counter++;
            }

            Common.Progress(progressPercent);
            return;

            void SetNewLocation(ExifFile file, string newDir, string newName)
            {
                var ext = file.Extension;
                if (config.ExtensionCase == CharacterCasing.lower)
                    ext = ext.ToLower();
                else if (config.ExtensionCase == CharacterCasing.upper)
                    ext = ext.ToUpper();

                var destFile = newName + ext;
                var destPath = Path.Combine(newDir, destFile);

                if (string.Compare(file.FullName, destPath, false) != 0)
                {
                    //var destFolder = Path.GetDirectoryName(destPath);
                    ////TODO: only check for the first time:
                    //if (destFolder != null && !Directory.Exists(destFolder))
                    //{
                    //    Trace.WriteLine($"Creating destination folder {destFolder}");
                    //    Directory.CreateDirectory(destFolder);
                    //}

#if RELEASE
                    Trace.WriteLine($"New location for file {file.Name} is {destFile}");
#else
                    Trace.WriteLine($"New location for file {file.FullName} is {destPath}");
#endif
                    //file.MoveTo(destPath);
                    file.NewLocation = destPath;
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

            var allFiles = Common.GetExifFiles(folder, "*.*", config.Recursive);

            var regex = new Regex(Common.WildcardToRegex(filter), RegexOptions.IgnoreCase);
            Trace.WriteLine($"Regex: {regex}");
            var files = allFiles.Where(f => regex.IsMatch(f.Name)).ToList();
            int fileCount = files.Count;

            //Parallel exif loading
            Common.Msg($"Loading EXIF date of {fileCount} files...");
            var i = 0;
            DateTime dummy;
            Common.Progress(0);
            Trace.Indent();
            ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 8 };
            var task = Parallel.ForEachAsync(files, parallelOptions, async (file, token) =>
            {
                i++;
                dummy = file.ExifTime;
                Common.Progress(100 * i / fileCount);
            });
            task.Wait();
            Trace.Unindent();

            Common.Msg($"Ordering {fileCount} files...");
            //TODO: ordering by the rename pattern minus counter...
            files = files.OrderBy(f => f.ExifTime).ThenBy(f => f.Name).ToList();

            Common.Msg($"Calculating new location for {fileCount} files...");

            var originalPattern = config.FilenamePattern;
            config.FilenamePattern = originalPattern?.Replace($"{{{COUNTER}:auto}}", $"{{{COUNTER}:" + fileCount.ToString().Length + "}");

            Trace.Indent();
            int counter = 1;
            Common.Progress(0);
            foreach (var file in files)
            {
                try
                {
                    CalculateNewLocation(allFiles, file, config, ref counter, 100 * counter / fileCount);
                }
                catch (Exception)
                {
                    if (config.StopOnError) throw;
                }
            }
            Trace.Unindent();

            //Folder creation
            Common.Msg($"Creating destination folder(s)...");
            Trace.Indent();
            files.Where(f => f.NewLocationSpecified).Select(f => Path.GetDirectoryName(f.NewLocation)).Distinct().ToList().ForEach(d =>
            {
                if (!Directory.Exists(d))
                {
                    Trace.WriteLine($"Creating folder {d}");
                    Directory.CreateDirectory(d!);
                }
            });
            Trace.Unindent();

            //Parallel renaming
            Common.Msg($"Renaming {fileCount} files...");
            i = 0;
            Common.Progress(0);
            Trace.Indent();
            task = Parallel.ForEachAsync(allFiles, parallelOptions, async (file, token) =>
            {
                try
                {
                    i++;
                    Trace.WriteLine($"Moving file {file.FullName} to {file.NewLocation}");
                    file.MoveToNewLocation();
                    Common.Progress(100 * i / allFiles.Count);
                }
                catch (Exception)
                {
                    if (config.StopOnError) throw;
                }
            });
            task.Wait();
            Trace.Unindent();

            config.FilenamePattern = originalPattern;
            Trace.Unindent();
        }
    }
}
