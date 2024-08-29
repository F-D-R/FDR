using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Globalization;
using System.Reflection;
using MetadataExtractor;
using System.Text.RegularExpressions;

namespace FDR.Tools.Library
{
    public static class Common
    {
        public const string param_import = "-import";
        public const string param_noactions = "-noactions";
        public const string param_force = "-force";
        public const string param_hash = "-hash";
        public const string param_rehash = "-rehash";
        public const string param_verify = "-verify";
        public const string param_diff = "-diff";
        public const string param_cleanup = "-cleanup";
        public const string param_verbose = "-verbose";
        public const string param_auto = "-auto";
        public const string param_file = "-file";
        public const string param_reference = "-reference";
        public const string param_rename = "-rename";
        public const string param_resize = "-resize";
        public const string param_web = "-web";
        public const string param_folderinfo = "-folderinfo";
        public const string param_config = "-config";
        public const string param_configfile = "-configfile";
        public const string param_help = "-help";

        private static int prevPercent = -1;
        private static int prevOverall = -1;

        public static void Msg(string msg, ConsoleColor color = ConsoleColor.White, bool newline = true)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = color;
            if (newline)
                Console.WriteLine(msg);
            else
                Console.Write(msg);
            Console.ResetColor();
        }

        public static void Progress(int percent, int? overall = null)
        {
            if (percent != prevPercent || overall.HasValue && overall.Value != prevOverall)
            {
                prevPercent = percent;

                if (overall.HasValue)
                {
                    prevOverall = overall.Value;
                    Msg($"    {percent}% ({overall}%)      \r", ConsoleColor.Gray, false);
                }
                else
                {
                    Msg($"    {percent}%                   \r", ConsoleColor.Gray, false);
                }
            }
        }

        public static void ShowAttributeHelp(Dictionary<string, string> attributes, bool quoteKey = true)
        {
            var windowWidth = Console.WindowWidth;
            var maxKeyLength = attributes.Aggregate<KeyValuePair<string, string>, int>(0, (max, cur) => max > cur.Key.Length ? max : cur.Key.Length);

            const char space = ' ';
            const char quote = '"';
            const int indentLength = 2;
            const int separatorLength = 1;
            const string bulleting = "- ";
            var attrWidth = indentLength + maxKeyLength + ((quoteKey) ? 2 : 0) + separatorLength;
            var textWidth = windowWidth - attrWidth - bulleting.Length;

            foreach (var a in attributes)
            {
                var attrLine = (new string(space, indentLength) + ((quoteKey) ? quote : String.Empty) + a.Key + ((quoteKey) ? quote : String.Empty) + new string(space, separatorLength)).PadRight(attrWidth, space) + bulleting;
                var lines = new List<string>();
                string line = String.Empty;
                foreach (var word in a.Value.Split(space))
                {
                    if ((line.Length + word.Length) >= textWidth)
                    {
                        lines.Add(line.TrimEnd());
                        line = String.Empty;
                    }
                    line += word + space;
                }
                if (line.Length > 0) lines.Add(line.TrimEnd());

                Common.Msg(attrLine + (lines.Count > 0 ? lines[0] : String.Empty));
                for (int i = 1; i < lines.Count; i++)
                    Common.Msg(new string(space, attrWidth + bulleting.Length) + lines[i]);
            }
        }

        public static bool IsFolderValid(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                Common.Msg("Folder name is missing!", ConsoleColor.Red);
                return false;
            }
            if (!System.IO.Directory.Exists(folder))
            {
                Common.Msg("Folder doesn't exist!", ConsoleColor.Red);
                return false;
            }
            return true;
        }

        public static string WildcardToRegex(string value)
        {
            value = Regex.Escape(value);
            var escSep = Regex.Escape(Path.DirectorySeparatorChar.ToString());
            return value.Replace("\\?", $"[^{escSep}]").Replace("\\*", $"[^{escSep}]*");
        }

        public static List<ExifFile> GetFiles(DirectoryInfo folder, ImportConfig config)
        {
            ArgumentNullException.ThrowIfNull(config);
            config.Validate();

            return GetFiles(folder, config.FileFilter, true);
        }

        public static List<ExifFile> GetFiles(DirectoryInfo folder, string filter, bool recursive)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filter);
            ArgumentNullException.ThrowIfNull(folder);
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var files = new List<ExifFile>();
            var options = new EnumerationOptions() { MatchCasing = MatchCasing.CaseInsensitive, RecurseSubdirectories = recursive, AttributesToSkip = FileAttributes.System };
            foreach (var tmpfilter in filter.Split('|'))
                files.AddRange(folder.GetFiles(tmpfilter, options).Select(fi => new ExifFile(fi)));

            return files.OrderBy(f => f.FullName).ToList();
        }

        public static List<ExifFile> GetFilesWithOutput(DirectoryInfo folder, string filter, bool recursive)
        {
            Msg($"Loading files from {folder}...\r", ConsoleColor.DarkGray, false);
            var stopwatch = Stopwatch.StartNew();
            var files = GetFiles(folder, filter, recursive);
            var time = GetTimeString(stopwatch);
            Msg($"Loaded {files.Count} files from {folder} ({time})       ", ConsoleColor.DarkGray);
            return files;
        }

        public static List<ExifFile> GetFiles(List<ExifFile> files, DirectoryInfo folder, string filter, bool recursive)
        {
            ArgumentNullException.ThrowIfNull(files);
            ArgumentException.ThrowIfNullOrWhiteSpace(filter);
            ArgumentNullException.ThrowIfNull(folder);
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            Trace.WriteLine($"{nameof(GetFiles)} (Filter: \"{filter}\", Recursive: {recursive})");

            var result = new List<ExifFile>();
            foreach (var tmpfilter in filter.Split('|'))
            {
                if (recursive)
                {
                    var regex = new Regex("^" + Common.WildcardToRegex(folder.FullName.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar) + ".*" + Common.WildcardToRegex(tmpfilter) + "$", RegexOptions.IgnoreCase);
                    Trace.WriteLine($"Regex (recursive): {regex}");
                    result.AddRange(files.Where(f => regex.IsMatch(f.FullName)));
                }
                else
                {
                    if (Path.IsPathRooted(tmpfilter))
                    {
                        var regex = new Regex("^" + Common.WildcardToRegex(tmpfilter) + "$", RegexOptions.IgnoreCase);
                        Trace.WriteLine($"Regex (non-recursive): {regex}");
                        result.AddRange(files.Where(f => regex.IsMatch(f.FullName)));
                    }
                    else
                    {
                        var regex = new Regex("^" + Common.WildcardToRegex(folder.FullName.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + tmpfilter) + "$", RegexOptions.IgnoreCase);
                        Trace.WriteLine($"Regex (non-recursive): {regex}");
                        result.AddRange(files.Where(f => regex.IsMatch(f.FullName)));
                    }
                }
            }
            return result.OrderBy(f => f.FullName).ToList();
        }

        public static IEnumerable<FileInfo> EnumerateFiles(DirectoryInfo folder, string filter, bool recursive = true)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filter);
            ArgumentNullException.ThrowIfNull(folder);
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var options = new EnumerationOptions() { MatchCasing = MatchCasing.CaseInsensitive, RecurseSubdirectories = recursive };
            foreach (var tmpfilter in filter.Split('|'))
                foreach (var fi in folder.EnumerateFiles(tmpfilter, options))
                    yield return fi;
        }

        public static string GetTimeString(Stopwatch stopwatch)
        {
            stopwatch.Stop();
            var ms = stopwatch.ElapsedMilliseconds;
            return (ms < 1000) ? $"{ms}ms" : $"{ms / 1000}s";
        }

        public static bool IsImageFile(string file)
        {
            return ".JPG|.JPEG|.TIF|.TIFF".Contains(Path.GetExtension(file), StringComparison.InvariantCultureIgnoreCase);
        }
        public static bool IsImageFile(FileInfo file)
        {
            return IsImageFile(file.Name);
        }

        public static async Task CopyFileAsync(string sourceFilePath, string destFilePath)
        {
            var destFolder = Path.GetDirectoryName(destFilePath);
            if (destFolder != null && !System.IO.Directory.Exists(destFolder)) System.IO.Directory.CreateDirectory(destFolder);

            using (Stream sourceStream = File.Open(sourceFilePath, FileMode.Open, FileAccess.Read))
            using (Stream destStream = File.Create(destFilePath))
                await sourceStream.CopyToAsync(destStream);

            File.SetAttributes(destFilePath, File.GetAttributes(sourceFilePath));
            File.SetCreationTimeUtc(destFilePath, File.GetCreationTimeUtc(sourceFilePath));
            File.SetLastWriteTimeUtc(destFilePath, File.GetLastWriteTimeUtc(sourceFilePath));
            File.SetLastAccessTimeUtc(destFilePath, File.GetLastAccessTimeUtc(sourceFilePath));
        }

        public static DateTime? GetExifDate(IEnumerable<MetadataExtractor.Directory>? exif)
        {
            if (exif == null) return null;

            const string EIFD0 = "Exif IFD0";
            const int EIFD0_DateTime = 306;

            const string ESIFD = "Exif SubIFD";
            const int ESIFD_DateTimeOriginal = 36867;
            const int ESIFD_DateTimeDigitized = 36868;

            const string QTMH = "QuickTime Movie Header";
            const int QTMH_Created = 3;
            const int QTMH_Modified = 4;

            const string QTTH = "QuickTime Track Header";
            const int QTTH_Created = 3;
            const int QTTH_Modified = 4;

            const string IPTC = "IPTC";
            const int IPTC_DateCreated = 567;
            const int IPTC_TimeCreated = 572;
            const int IPTC_DigitalDateCreated = 574;
            const int IPTC_DigitalTimeCreated = 575;

            const string AVI = "AVI";
            const int AVI_DateTimeOriginal = 320;

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == EIFD0 && directory.ContainsTag(EIFD0_DateTime))
                        return DateTime.ParseExact(directory.GetString(EIFD0_DateTime)!, "yyyy:MM:dd HH:mm:ss", null);
            }
            catch { }

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == ESIFD && directory.ContainsTag(ESIFD_DateTimeOriginal))
                        return DateTime.ParseExact(directory.GetString(ESIFD_DateTimeOriginal)!, "yyyy:MM:dd HH:mm:ss", null);
            }
            catch { }

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == ESIFD && directory.ContainsTag(ESIFD_DateTimeDigitized))
                        return DateTime.ParseExact(directory.GetString(ESIFD_DateTimeDigitized)!, "yyyy:MM:dd HH:mm:ss", null);
            }
            catch { }

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == QTMH && directory.ContainsTag(QTMH_Created))
                        return directory.GetDateTime(QTMH_Created).ToLocalTime();
            }
            catch { }

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == QTMH && directory.ContainsTag(QTMH_Modified))
                        return directory.GetDateTime(QTMH_Modified).ToLocalTime();
            }
            catch { }

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == QTTH && directory.ContainsTag(QTTH_Created))
                        return directory.GetDateTime(QTTH_Created).ToLocalTime();
            }
            catch { }

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == QTTH && directory.ContainsTag(QTTH_Modified))
                        return directory.GetDateTime(QTTH_Modified).ToLocalTime();
            }
            catch { }

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == IPTC && directory.ContainsTag(IPTC_DateCreated) && directory.ContainsTag(IPTC_TimeCreated))
                        return DateTime.ParseExact(directory.GetString(IPTC_DateCreated) + " " + directory.GetString(IPTC_TimeCreated), "yyyyMMdd HHmmss", null);
            }
            catch { }

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == IPTC && directory.ContainsTag(IPTC_DigitalDateCreated) && directory.ContainsTag(IPTC_DigitalTimeCreated))
                        return DateTime.ParseExact(directory.GetString(IPTC_DigitalDateCreated) + " " + directory.GetString(IPTC_DigitalTimeCreated), "yyyyMMdd HHmmss", null);
            }
            catch { }

            try
            {
                foreach (var directory in exif)
                    if (directory.Name == AVI && directory.ContainsTag(AVI_DateTimeOriginal))
                        return DateTime.ParseExact(directory.GetString(AVI_DateTimeOriginal)!, "ddd MMM dd HH:mm:ss yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch { }

            return null;
        }

        public static DateTime? GetExifDateOnly(this FileInfo file)
        {
            try
            {
                IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file.FullName);
                return GetExifDate(directories);
            }
            catch { }

            return null;
        }

        public static DateTime GetExifDate(this FileInfo file, DateTime defaultDate)
        {
            DateTime? date = GetExifDateOnly(file);
            return date ?? defaultDate;
        }

        public static DateTime GetExifDate(this FileInfo file)
        {
            return GetExifDate(file, file.CreationTime < file.LastWriteTime ? file.CreationTime : file.LastWriteTime);
        }


        #region Get property name from attribute

        public static T? GetAttribute<T>(this MemberInfo member, bool isRequired = false) where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The {0} attribute must be defined on member {1}", typeof(T).Name, member.Name));
            }

            if (attribute != null) return (T)attribute;
            return null;
        }

        public static T? GetAttribute<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static string GetDisplayName(this MemberInfo member)
        {
            var displayAttribute = member.GetAttribute<DisplayAttribute>();
            if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Name))
                return displayAttribute.Name;

            var displayNameAttribute = member.GetAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null && !string.IsNullOrWhiteSpace(displayNameAttribute.DisplayName))
                return displayNameAttribute.DisplayName;

            return member.Name;
        }

        public static string GetDisplayName(this Enum enumVal)
        {
            var displayAttribute = enumVal.GetAttribute<DisplayAttribute>();
            if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Name))
                return displayAttribute.Name;

            return enumVal.ToString();
        }

        public static string GetDisplayNameFor(this Type type, string memberName)
        {
            var property = type.GetProperty(memberName);
            if (property != null)
                return property.GetDisplayName();

            var field = type.GetField(memberName);
            if (field != null)
                return field.GetDisplayName();

            return memberName;
        }


        public static MemberInfo? GetPropertyInformation(Expression propertyExpression)
        {
            ArgumentNullException.ThrowIfNull(propertyExpression);
            MemberExpression? memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {

                UnaryExpression? unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }

        public static string? GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);

            if (memberInfo == null) throw new ArgumentException("No property reference expression was found.", nameof(propertyExpression));

            var displayAttribute = memberInfo.GetAttribute<DisplayAttribute>(false);
            if (displayAttribute != null)
                return displayAttribute.Name;

            var displayNameAttribute = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (displayNameAttribute != null)
                return displayNameAttribute.DisplayName;

            return memberInfo.Name;
        }

        #endregion

        public static Dictionary<string, string> GetProgramParameterList()
        {
            var attributes = new Dictionary<string, string>()
            {
                { Common.param_help + " [<function>]", "Generic help (this screen) or optionally help about a given function" },
                { Common.param_import + " [<function>]", "Import memory card content or optionally the content of a folder" },
                { Common.param_hash + " <folder>", "Create hash of files in a folder" },
                { Common.param_rehash + " <folder>", "Recreate hashes of all files in a folder" },
                { Common.param_verify + " <folder>", "Verify the files in a folder against their saved hash" },
                { Common.param_diff + " <folder>", "Compare the files of a folder to a reference one" },
                { Common.param_reference + " <folder>", "Reference folder for the diff function" },
                { Common.param_cleanup + " <folder>", "Delete unnecessary raw, hash and err files" },
                { Common.param_rename + " <folder>", "Rename image files based on a given configuration" },
                { Common.param_resize + " <folder>", "Resize image files based on a given configuration" },
                { Common.param_folderinfo + " <folder>", "Print folder info to the output" },
                { Common.param_configfile + " <file>", "Path of the configuration file to use instead of appsettings.json" },
                { Common.param_config + " <config>", "Named configuration for some functions like renaming and resizing" },
                { Common.param_web, "Start the web application" },
                { Common.param_auto, "Start the import automatically" },
                { Common.param_noactions, "Skip the actions during import to enable importing from multiple sources" },
                { Common.param_force, "Force importing existing folders" },
                { Common.param_verbose, "More detailed output" }
            };
            return attributes;
        }

    }

    public class TimedScope : IDisposable
    {
        private Stopwatch stopwatch;
        private (int Left, int Top) pos;
        private ConsoleColor color;
        private string msg;

        public TimedScope(string beginMsg, string? endMsg = null, ConsoleColor color = ConsoleColor.DarkGray)
        {
            msg = endMsg ?? beginMsg;
            this.color=color;
            stopwatch = Stopwatch.StartNew();

            if (!Console.IsOutputRedirected)
                pos = Console.GetCursorPosition();
            Common.Msg(beginMsg, color);
        }

        public void Dispose()
        {
            var time = Common.GetTimeString(stopwatch);
            if (msg.IndexOf("{time}", StringComparison.InvariantCultureIgnoreCase) >= 0)
                msg.Replace("{time}", time, StringComparison.InvariantCultureIgnoreCase);
            else
                msg += $" ({time})";
            if (!Console.IsOutputRedirected && Console.GetCursorPosition().Top == pos.Top + 1)
                Console.SetCursorPosition(pos.Left, pos.Top);
            Common.Msg(msg + "                ", color);
            Common.Msg("                                                        \r", color, newline: false);
        }
    }
}
