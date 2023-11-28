using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp;

namespace FDR.Tools.Library
{
    public static class Common
    {
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
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();

            return GetFiles(folder, config.FileFilter, true);
        }

        public static List<FileInfo> GetFiles(DirectoryInfo folder, ResizeConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();

            return GetFiles(folder, config.FileFilter, config.Recursive);
        }

        public static List<FileInfo> GetFiles(DirectoryInfo folder, string filter, bool recursive)
        {
            if (string.IsNullOrWhiteSpace(filter)) throw new ArgumentNullException("filter");
            if (folder == null) throw new ArgumentNullException("folder");
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var files = new List<FileInfo>();
            var options = new EnumerationOptions() { MatchCasing = MatchCasing.CaseInsensitive, RecurseSubdirectories = recursive };
            foreach (var tmpfilter in filter.Split('|'))
                files.AddRange(folder.GetFiles(tmpfilter, options));

            return files.OrderBy(f => f.CreationTimeUtc).ToList();
        }

        public static IEnumerable<FileInfo> EnumerateFiles(DirectoryInfo folder, string filter, bool recursive = true)
        {
            if (string.IsNullOrWhiteSpace(filter)) throw new ArgumentNullException("filter");
            if (folder == null) throw new ArgumentNullException("folder");
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

        public static bool GetExifDate(ExifProfile? exif, out DateTime? date)
        {
            date = null;
            if (exif == null) return false;

            try
            {
                IExifValue<string>? dateExif = null;
                if (exif.TryGetValue(ExifTag.DateTimeOriginal, out dateExif)
                    || exif.TryGetValue(ExifTag.DateTime, out dateExif)
                    || exif.TryGetValue(ExifTag.DateTimeDigitized, out dateExif))
                {
                    string? dateString = dateExif?.Value;
                    if (string.IsNullOrWhiteSpace(dateString)) return false;

                    date = DateTime.ParseExact(dateString, "yyyy:MM:dd HH:mm:ss", null);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
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

            DateTime? date;
            if (GetExifDate(imageInfo.Metadata?.ExifProfile, out date) && date !=null) return date.Value;

            if (imageInfo.FrameMetadataCollection != null)
            {
                foreach (var fmeta in imageInfo.FrameMetadataCollection)
                {
                    if (GetExifDate(fmeta?.ExifProfile, out date) && date != null) return date.Value;
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
            private Dictionary<string, DateTime> dates = new Dictionary<string, DateTime>();

            public override int Compare(FileInfo? x, FileInfo? y)
            {
                if (x == null || y == null) return 0;

                DateTime dateX, dateY;
                if (!dates.TryGetValue(x.FullName, out dateX))
                {
                    dateX = x.GetExifDate();
                    dates[x.FullName] = dateX;
                }
                if (!dates.TryGetValue(y.FullName, out dateY))
                {
                    dateY = y.GetExifDate();
                    dates[y.FullName] = dateY;
                }
                return DateTime.Compare(dateX, dateY);
            }
        }

    }
}
