using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FDR.Tools.Library
{
    public static class Rename
    {
        private const string REGEX = "{([^:}]*):?([^}]*)}";
        private const string NOW = "now";
        private const string NAME = "name";
        private const string PFOLDER = "pfolder";
        private const string CDATE = "cdate";
        private const string MDATE = "mdate";
        private const string EDATE = "edate";
        private const string SDATE = "sdate";
        private const string COUNTER = "counter";

        public static string EvaluateNamePattern(string pattern, FileSystemInfo fsi)
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
                                result = result.Replace(match.Value, fsi.Name.Substring(int.Parse(args[0]), int.Parse(args[1])), StringComparison.InvariantCultureIgnoreCase);
                            else
                                result = result.Replace(match.Value, fsi.Name, StringComparison.InvariantCultureIgnoreCase);
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

                    case EDATE:
                    case SDATE:
                        throw new NotImplementedException("EDATE/SDATE");
                    //if (fsi != null)
                    //{
                    //    result = result.Replace(match.Value, fsi.LastWriteTime.ToString(arg), StringComparison.InvariantCultureIgnoreCase);
                    //}
                    //break;

                    case NOW:
                        result = result.Replace(match.Value, DateTime.Now.ToString(arg), StringComparison.InvariantCultureIgnoreCase);
                        break;
                }
            }

            return result;
        }

        public static string EvaluateFolderNamePattern(string pattern, DirectoryInfo folder)
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
                                result = result.Replace(match.Value, folder.Parent.Name.Substring(int.Parse(args[0]), int.Parse(args[1])), StringComparison.InvariantCultureIgnoreCase);
                            else
                                result = result.Replace(match.Value, folder.Parent.Name, StringComparison.InvariantCultureIgnoreCase);
                        }
                        break;
                }
            }

            return result;
        }

        public static string EvaluateFileNamePattern(string pattern, FileInfo file, int counter)
        {
            //TODO: EXIF date

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
                                result = result.Replace(match.Value, file.Directory.Name.Substring(int.Parse(args[0]), int.Parse(args[1])), StringComparison.InvariantCultureIgnoreCase);
                            else
                                result = result.Replace(match.Value, file.Directory.Name, StringComparison.InvariantCultureIgnoreCase);
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

        //public static void RenameFolder(string pattern, DirectoryInfo folder)
        //{
        //    var name = EvaluateFolderNamePattern(pattern, folder);
        //    folder.MoveTo(name);
        //}

        public static void RenameFile(FileInfo file, int counter, RenameConfig config, int progressPercent)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (file == null) throw new ArgumentNullException("file");
            if (!file.Exists) throw new FileNotFoundException("File doesn't exist!", file.FullName);

            var path = Path.GetDirectoryName(file.FullName);

            var origName = Path.GetFileNameWithoutExtension(file.Name);
            var newName = EvaluateFileNamePattern(config.FilenamePattern, file, counter);
            if (config.FilenameCase == CharacterCasing.lower)
                newName = newName.ToLower();
            else if (config.FilenameCase == CharacterCasing.upper)
                newName = newName.ToUpper();

            var extension = Path.GetExtension(file.Name);
            if (config.ExtensionCase == CharacterCasing.lower)
                extension = extension.ToLower();
            else if (config.ExtensionCase == CharacterCasing.upper)
                extension = extension.ToUpper();

            Trace.WriteLine($"Renaming file {file.Name} to {newName + extension}");
            Common.Progress(progressPercent);
            file.MoveTo(Path.Combine(path, newName + extension));

            if (config.AdditionalFileTypes != null)
            {
                foreach (var type in config.AdditionalFileTypes)
                {
                    extension = type;
                    if (config.ExtensionCase == CharacterCasing.lower)
                        extension = extension.ToLower();
                    else if (config.ExtensionCase == CharacterCasing.upper)
                        extension = extension.ToUpper();

                    var origPath = Path.Combine(path, origName + type);
                    if (File.Exists(origPath))
                    {
                        var newPath = Path.Combine(path, newName + extension);
                        Trace.WriteLine($"Renaming file {origName + type} to {newName + extension}");
                        File.Move(origPath, newPath);
                    }
                }
            }
        }

        public static void RenameFilesInFolder(DirectoryInfo folder, RenameConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();
            if (folder == null) throw new ArgumentNullException("folder");
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var filter = config.Filter;
            Common.Msg($"Renaming {filter} files in {folder.FullName}");
            Trace.Indent();

            if (string.IsNullOrWhiteSpace(filter)) filter = "*.*";

            var files = Common.GetFiles(folder, filter);

            int fileCount = files.Count;
            int counter = 1;
            Common.Progress(0);
            foreach (var file in files.OrderBy(f => f.CreationTimeUtc).ToList())
            {
                try
                {
                    RenameFile(file, counter, config, 100 * counter / fileCount);
                }
                catch (IOException)
                {
                    if (config.StopOnError) throw;
                }
                counter++;
            }

            Trace.Unindent();
        }
    }
}
