using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace FDR.Tools.Library
{
    public static class Verify
    {
        private const string DEFAULT_FILTER = "*.CR2|*.CRW|*.JPG|*.MP4|*.AVI|*.MOV";

        private static string ComputeHash(FileInfo file)
        {
            using (var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(fileStream);
                return Convert.ToBase64String(hash);
            }
        }

        private static string GetMd5FileName(FileInfo file)
        {
            return Path.Combine(file.DirectoryName, "." + file.Name + ".md5");
        }

        private static string GetErrorFileName(FileInfo file)
        {
            return file.FullName + ".error";
        }

        private static void CreateHashFile(string hashFile, string hash, DateTime fileDateUtc)
        {
            File.WriteAllText(hashFile, hash);
            File.SetLastWriteTimeUtc(hashFile, fileDateUtc);
            File.SetAttributes(hashFile, File.GetAttributes(hashFile) | FileAttributes.Hidden);
        }

        public static void HashFolder(DirectoryInfo folder)
        {
            var start = DateTime.Now.Ticks;

            Common.Msg($"Creating hash files for folder {folder}");

            var files = Common.GetFiles(folder, DEFAULT_FILTER);
            int fileCount = files.Count;

            var i = 0;
            var hashCount = 0;
            Common.Progress(0);

            foreach (var file in files)
            {
                var md5File = GetMd5FileName(file);
                if (!File.Exists(md5File))
                {
                    CreateHashFile(md5File, ComputeHash(file), file.LastWriteTimeUtc);
                    hashCount++;
                }
                else
                    File.SetAttributes(md5File, FileAttributes.Hidden);

                i++;
                Common.Progress(100 * i / fileCount);
            }

            var time = Common.GetTimeString(start);
            Common.Msg($"{hashCount} new hash files were created for {fileCount} files in {folder} folder... ({time})", ConsoleColor.Green);
        }

        public static void VerifyFolder(DirectoryInfo folder)
        {
            var start = DateTime.Now.Ticks;

            Common.Msg($"Verifying folder {folder}");

            var files = Common.GetFiles(folder, DEFAULT_FILTER);
            int fileCount = files.Count;
            int errCount = 0;
            int warnCount = 0;

            var i = 0;
            Common.Progress(0);

            Trace.Indent();
            foreach (var file in files)
            {
                var md5File = GetMd5FileName(file);
                var errFile = GetErrorFileName(file);
                var fileDate = file.LastWriteTimeUtc;

                var newHash = ComputeHash(file);

                if (File.Exists(md5File))
                {
                    var oldHash = File.ReadAllText(md5File).Trim();
                    if (string.Compare(oldHash, newHash, true) != 0)
                    {
                        errCount++;
                        Trace.WriteLine($"{file.FullName} - Invalid hash!");
                        File.WriteAllText(errFile, $"{DateTime.Now}\tInvalid hash! It has changed from {oldHash} to {newHash}");
                    }
                    else
                    {
                        var md5Date = File.GetLastWriteTimeUtc(md5File);
                        if (fileDate != md5Date)
                        {
                            warnCount++;
                            Trace.WriteLine($"{file.FullName} - Date has changed from {md5Date:yyyy-MM-dd} to {fileDate:yyyy-MM-dd}");
                        }
                    }
                }
                else
                    CreateHashFile(md5File, newHash, fileDate);

                i++;
                Common.Progress(100 * i / fileCount);
            }
            Trace.Unindent();

            var time = Common.GetTimeString(start);
            if (errCount > 0)
            {
                if (warnCount > 0)
                    Common.Msg($"Verification of {fileCount} files in {folder} folder resulted in {errCount} errors and {warnCount} warnings! ({time})", ConsoleColor.Red);
                else
                    Common.Msg($"Verification of {fileCount} files in {folder} folder resulted in {errCount} errors! ({time})", ConsoleColor.Red);
            }
            else if (warnCount > 0)
                Common.Msg($"Verification of {fileCount} files in {folder} folder succeeded with {warnCount} warnings... ({time})", ConsoleColor.Yellow);
            else
                Common.Msg($"Verification of {fileCount} files in {folder} folder succeeded... ({time})", ConsoleColor.Green);
        }

        public static void DiffFolder(DirectoryInfo folder, DirectoryInfo reference)
        {
            var start = DateTime.Now.Ticks;

            Common.Msg($"Comparing hashes of files in {folder} folder to {reference} folder");

            var files = Common.GetFiles(folder, DEFAULT_FILTER);
            int fileCount = files.Count;

            var i = 0;
            var verified = 0;
            var diff = 0;
            var plus = 0;
            Common.Progress(0);

            Trace.Indent();
            foreach (var file in files)
            {
                var md5File = GetMd5FileName(file);
                if (File.Exists(md5File))
                {
                    verified++;
                    var hash = File.ReadAllText(md5File).Trim();
                    var date = file.LastWriteTimeUtc;

                    var relPath = Path.GetRelativePath(folder.FullName, md5File);
                    var refPath = Path.Combine(reference.FullName, relPath);

                    if (File.Exists(refPath))
                    {
                        var refHash = File.ReadAllText(refPath).Trim();
                        var refDate = File.GetLastWriteTimeUtc(refPath);

                        var hashDiff = hash != refHash;
                        var dateDiff = date != refDate;

                        if (hashDiff && dateDiff)
                        {
                            diff++;
                            Trace.WriteLine($"{file.FullName} - Date and content has changed!");
                        }
                        else if (hashDiff)
                        {
                            diff++;
                            Trace.WriteLine($"{file.FullName} - Content has changed!");
                        }
                        else if (dateDiff)
                        {
                            diff++;
                            Trace.WriteLine($"{file.FullName} - Date has changed!");
                        }
                    }
                    else
                        plus++;
                }

                i++;
                Common.Progress(100 * i / fileCount);
            }
            Trace.Unindent();

            var time = Common.GetTimeString(start);
            Common.Msg($"{verified} of {fileCount} files in {folder} folder has been checked against {reference} folder. ({time})");
            if (diff > 0 && plus > 0)
                Common.Msg($"{diff} files differ, and {plus} files don't exist in {reference} folder.", ConsoleColor.Red);
            else if (diff > 0)
                Common.Msg($"{diff} files differ!", ConsoleColor.Red);
            else if (plus > 0)
                Common.Msg($"All the checked files match, but {plus} files don't exist in {reference} folder.", ConsoleColor.Yellow);
            else
                Common.Msg($"All the checked files match...", ConsoleColor.Green);
        }

    }
}
