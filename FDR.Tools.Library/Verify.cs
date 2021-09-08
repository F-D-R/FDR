using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;

namespace FDR.Tools.Library
{
    public static class Verify
    {
        private const string DEFAULT_FILTER = "*.CR2|*.CRW|*.JPG|*.TIF|*.MP4|*.AVI|*.MOV";

        private static string ComputeHash(FileInfo file)
        {
            using (var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(fileStream);
                return Convert.ToBase64String(hash);
            }
        }

        internal static string GetMd5FileName(FileInfo file)
        {
            return Path.Combine(file.DirectoryName, "." + file.Name + ".md5");
        }

        internal static string GetFileNameFromMD5(FileInfo file)
        {
            return Path.Combine(file.DirectoryName, Path.GetFileNameWithoutExtension(file.Name.TrimStart('.')));
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

        private static bool IsValidImage(string file)
        {
            try
            {
                //var imgFormat = Image.DetectFormat(file);
                var image = Image.Load(file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static bool IsValidImage(FileInfo file)
        {
            return IsValidImage(file.FullName);
        }

        public static void HashFolder(DirectoryInfo folder, bool force)
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
                if (force || !File.Exists(md5File))
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
                {
                    if (Common.IsImageFile(file) && !IsValidImage(file))
                    {
                        errCount++;
                        Trace.WriteLine($"{file.FullName} - Invalid image!");
                        File.WriteAllText(errFile, $"{DateTime.Now}\tInvalid image!");
                    }
                    else
                        CreateHashFile(md5File, newHash, fileDate);
                }

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
            var diffDate = 0;
            var plus = 0;
            Common.Progress(0);

            Trace.Indent();
            foreach (var file in files)
            {
                var dir = Path.GetDirectoryName(file.FullName);
                var relDir = Path.GetDirectoryName(Path.GetRelativePath(folder.FullName, file.FullName));
                var plusDir = Path.Combine(folder.FullName, "plus", relDir);
                var diffDir = Path.Combine(folder.FullName, "diff", relDir);

                var md5File = GetMd5FileName(file);
                if (File.Exists(md5File))
                {
                    verified++;
                    var hash = File.ReadAllText(md5File).Trim();
                    var date = file.LastWriteTimeUtc;

                    var relPath = Path.GetRelativePath(folder.FullName, md5File);
                    var refMd5 = Path.Combine(reference.FullName, relPath);
                    var refFile = Path.Combine(reference.FullName, Path.GetDirectoryName(relPath), file.Name);

                    if (File.Exists(refMd5))
                    {
                        var refHash = File.ReadAllText(refMd5).Trim();
                        var refDate = File.GetLastWriteTimeUtc(refMd5);

                        var hashDiff = hash != refHash;
                        var dateDiff = date != refDate;

                        if (hashDiff && dateDiff)
                        {
                            diff++;
                            diffDate++;
                            Trace.WriteLine($"{file.FullName} - Date and content has changed!");
                            CopyFiles(diffDir, file.FullName);
                            CopyFiles(diffDir, refFile, "_ref");
                        }
                        else if (hashDiff)
                        {
                            diff++;
                            Trace.WriteLine($"{file.FullName} - Content has changed!");
                            CopyFiles(diffDir, file.FullName);
                            CopyFiles(diffDir, refFile, "_ref");
                        }
                        else if (dateDiff)
                        {
                            diffDate++;
                            Trace.WriteLine($"{file.FullName} - Date has changed!");
                        }
                    }
                    else
                    {
                        plus++;
                        CopyFiles(plusDir, file.FullName);
                    }
                }

                i++;
                Common.Progress(100 * i / fileCount);
            }
            Trace.Unindent();

            var time = Common.GetTimeString(start);
            Common.Msg($"{verified} of {fileCount} files in {folder} folder has been checked against {reference} folder. ({time})");
            if (diff > 0)
                Common.Msg($"{diff} files differ!", ConsoleColor.Red);
            if (plus > 0)
                Common.Msg($"{plus} files don't exist in {reference} folder.", ConsoleColor.Yellow);
            if (diffDate > 0)
                Common.Msg($"{diffDate} files differ in date.", ConsoleColor.Yellow);
            if (diff == 0 && plus == 0 && diffDate == 0)
                Common.Msg($"All the checked files match...", ConsoleColor.Green);
            return;



            void CopyFiles(string destFolder, string refFile, string postFix = "")
            {
                if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);
                var destFile = Path.Combine(destFolder, Path.GetFileNameWithoutExtension(refFile) + postFix + Path.GetExtension(refFile));
                var destMd5 = GetMd5FileName(new FileInfo(destFile));
                File.Copy(refFile, destFile, true);
            }
        }

    }
}
