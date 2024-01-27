using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Globalization;
using System.Reflection;

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
        public const string param_config = "-config";
        public const string param_configfile = "-configfile";
        public const string param_help = "-help";

        private static FileDateComparer? comparer;

        public static FileDateComparer FileComparer
        {
            get
            {
                comparer ??= new FileDateComparer();
                return comparer;
            }
            set { comparer = value; }
        }

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
            if (overall.HasValue)
                Msg($"    {percent}% ({overall}%)      \r", ConsoleColor.Gray, false);
            else
                Msg($"    {percent}%                   \r", ConsoleColor.Gray, false);
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
            if (!Directory.Exists(folder))
            {
                Common.Msg("Folder doesn't exist!", ConsoleColor.Red);
                return false;
            }
            return true;
        }

        public static List<FileInfo> GetFiles(DirectoryInfo folder, ImportConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            config.Validate();

            return GetFiles(folder, config.FileFilter, true);
        }

        public static List<FileInfo> GetFiles(DirectoryInfo folder, ResizeConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            config.Validate();

            return GetFiles(folder, config.FileFilter, config.Recursive);
        }

        public static List<FileInfo> GetFiles(DirectoryInfo folder, string filter, bool recursive)
        {
            if (string.IsNullOrWhiteSpace(filter)) throw new ArgumentNullException(nameof(filter));
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var files = new List<FileInfo>();
            var options = new EnumerationOptions() { MatchCasing = MatchCasing.CaseInsensitive, RecurseSubdirectories = recursive };
            foreach (var tmpfilter in filter.Split('|'))
                files.AddRange(folder.GetFiles(tmpfilter, options));

            return files.OrderBy(f => f.FullName).ToList();
        }

        public static IEnumerable<FileInfo> EnumerateFiles(DirectoryInfo folder, string filter, bool recursive = true)
        {
            if (string.IsNullOrWhiteSpace(filter)) throw new ArgumentNullException(nameof(filter));
            if (folder == null) throw new ArgumentNullException(nameof(folder));
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
            if (destFolder != null && !Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

            using (Stream sourceStream = File.Open(sourceFilePath, FileMode.Open, FileAccess.Read))
            using (Stream destStream = File.Create(destFilePath))
                await sourceStream.CopyToAsync(destStream);

            File.SetAttributes(destFilePath, File.GetAttributes(sourceFilePath));
            File.SetCreationTimeUtc(destFilePath, File.GetCreationTimeUtc(sourceFilePath));
            File.SetLastWriteTimeUtc(destFilePath, File.GetLastWriteTimeUtc(sourceFilePath));
            File.SetLastAccessTimeUtc(destFilePath, File.GetLastAccessTimeUtc(sourceFilePath));
        }

        public static DateTime? GetExifDate(ExifProfile? exif)
        {
            if (exif == null) return null;

            try
            {
                if (exif.TryGetValue(ExifTag.DateTimeOriginal, out IExifValue<string>? dateExif)
                    || exif.TryGetValue(ExifTag.DateTime, out dateExif)
                    || exif.TryGetValue(ExifTag.DateTimeDigitized, out dateExif))
                {
                    string? dateString = dateExif?.Value;
                    if (string.IsNullOrWhiteSpace(dateString)) return null;

                    return DateTime.ParseExact(dateString, "yyyy:MM:dd HH:mm:ss", null);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DateTime? GetExifDateOnly(this FileInfo file)
        {
            ImageInfo imageInfo;
            try
            {
                imageInfo = Image.Identify(file.FullName);
                if (imageInfo == null) return null;
            }
            catch (Exception)
            {
                return null;
            }

            DateTime? date = GetExifDate(imageInfo.Metadata?.ExifProfile);
            if (date !=null) return date.Value;

            if (imageInfo.FrameMetadataCollection != null)
            {
                foreach (var fmeta in imageInfo.FrameMetadataCollection)
                {
                    date = GetExifDate(fmeta?.ExifProfile);
                    if (date != null) return date.Value;
                }
            }

            return null;
        }

        public static DateTime GetExifDate(this FileInfo file, DateTime defaultDate)
        {
            ImageInfo imageInfo;
            try
            {
                imageInfo = Image.Identify(file.FullName);
                if (imageInfo == null) return defaultDate;
            }
            catch (Exception)
            {
                return defaultDate;
            }

            DateTime? date = GetExifDate(imageInfo.Metadata?.ExifProfile);
            if (date !=null) return date.Value;

            if (imageInfo.FrameMetadataCollection != null)
            {
                foreach (var fmeta in imageInfo.FrameMetadataCollection)
                {
                    date = GetExifDate(fmeta?.ExifProfile);
                    if (date != null) return date.Value;
                }
            }

            return defaultDate;
        }

        public static DateTime GetExifDate(this FileInfo file)
        {
            return GetExifDate(file, file.CreationTime < file.LastWriteTime ? file.CreationTime : file.LastWriteTime);
        }

        public class FileDateComparer : Comparer<FileInfo>
        {
            private readonly ConcurrentDictionary<string, DateTime> dates = new();

            public override int Compare(FileInfo? x, FileInfo? y)
            {
                if (x == null || y == null) return 0;

                if (!dates.TryGetValue(x.FullName, out DateTime dateX))
                {
                    dateX = x.GetExifDate();
                    dates[x.FullName] = dateX;
                }
                if (!dates.TryGetValue(y.FullName, out DateTime dateY))
                {
                    dateY = y.GetExifDate();
                    dates[y.FullName] = dateY;
                }
                return DateTime.Compare(dateX, dateY);
            }
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
            if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));
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
}
