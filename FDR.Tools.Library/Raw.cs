using System;
using System.Diagnostics;
using System.IO;

namespace FDR.Tools.Library
{
    public static class Raw
    {
        private const string DEFAULT_RAW_FOLDER = "RAW";

        public static void CleanupFolder(DirectoryInfo folder)
        {
            var start = DateTime.Now.Ticks;

            Common.Msg($"Cleaning up folder {folder}");

            int rawCount = 0;
            int hashCount = 0;
            int errCount = 0;

            var rawFolder = new DirectoryInfo(Path.Combine(folder.FullName, DEFAULT_RAW_FOLDER));
            if (rawFolder.Exists)
                CleanupRawFiles(rawFolder, ref rawCount);

            CleanupHashFiles(folder, ref hashCount);

            CleanupErrorFiles(folder, ref errCount);

            if (rawFolder.Exists && rawFolder.GetFiles().Length == 0)
                rawFolder.Delete();

            var time = Common.GetTimeString(start);
            Common.Msg($"Cleanup of {folder} folder succeeded: {rawCount} raw, {hashCount} hash and {errCount} error files were deleted. ({time})", ConsoleColor.Green);
        }

        private static void CleanupRawFiles(DirectoryInfo folder, ref int rawCount)
        {
            Common.Msg($"Cleaning up raw files in {folder}");

            var parent = folder.Parent;

            var options = new EnumerationOptions
            {
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = false
            };
            var files = folder.GetFiles("*.cr?", options);
            int fileCount = files.Length;

            var i = 0;
            Common.Progress(0);

            Trace.Indent();
            foreach (var file in files)
            {
                if (Directory.GetFiles(parent.FullName, Path.GetFileNameWithoutExtension(file.Name) + "*.jpg", options).Length == 0)
                {
                    Trace.WriteLine($"Deleting raw file: {file.FullName}");
                    rawCount++;
                    file.Delete();
                }

                i++;
                Common.Progress(100 * i / fileCount);
            }
            Trace.Unindent();
        }

        private static void CleanupHashFiles(DirectoryInfo folder, ref int hashCount)
        {
            Common.Msg($"Cleaning up hash files in {folder}");

            var options = new EnumerationOptions
            {
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = true,
                AttributesToSkip = FileAttributes.System
            };
            var files = folder.GetFiles("*.md5", options);
            int fileCount = files.Length;

            var i = 0;
            Common.Progress(0);

            Trace.Indent();
            foreach (var file in files)
            {
                var imageFile = Verify.GetFileNameFromMD5(file);
                if (!File.Exists(imageFile))
                {
                    Trace.WriteLine($"Deleting hash file: {file.FullName}");
                    hashCount++;
                    file.Delete();
                }

                i++;
                Common.Progress(100 * i / fileCount);
            }
            Trace.Unindent();
        }

        private static void CleanupErrorFiles(DirectoryInfo folder, ref int errCount)
        {
            Common.Msg($"Cleaning up error files in {folder}");

            var options = new EnumerationOptions
            {
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = true,
                AttributesToSkip = FileAttributes.System
            };
            var files = folder.GetFiles("*.error", options);
            int fileCount = files.Length;

            var i = 0;
            Common.Progress(0);

            Trace.Indent();
            foreach (var file in files)
            {
                var imageFile = Verify.GetFileNameFromError(file);
                if (!File.Exists(imageFile))
                {
                    Trace.WriteLine($"Deleting error file: {file.FullName}");
                    errCount++;
                    file.Delete();
                }

                i++;
                Common.Progress(100 * i / fileCount);
            }
            Trace.Unindent();
        }

    }
}
