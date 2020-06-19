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

        private static string GetTimeString(long startTicks)
        {
            var ms = (DateTime.Now.Ticks - startTicks) / 10000;
            return (ms < 1000) ? $"{ms}ms" : $"{ms / 1000}s";
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
            File.SetAttributes(hashFile, File.GetAttributes(hashFile) | FileAttributes.Hidden);
            File.SetLastWriteTimeUtc(hashFile, fileDateUtc);
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

                i++;
                Common.Progress(100 * i / fileCount);
            }

            var time = GetTimeString(start);
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

            var time = GetTimeString(start);
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
    }
}
