using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using SixLabors.ImageSharp;
using System.Threading.Tasks;

namespace FDR.Tools.Library
{
    public static class Verify
    {
        private const string DEFAULT_FILTER = "*.CR3|*.CR2|*.CRW|*.DNG|*.JPG|*.TIF|*.MP4|*.AVI|*.MOV";

        internal static string ComputeHash(FileInfo file)
        {
            using var md5 = MD5.Create();
            using var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            var hash = md5.ComputeHash(fileStream);
            return Convert.ToBase64String(hash);
        }

        internal static async Task<string> ComputeHashAsync(FileInfo file)
        {
            using var md5 = MD5.Create();
            using var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            var hash = await md5.ComputeHashAsync(fileStream);
            return Convert.ToBase64String(hash);
        }

        internal static string GetMd5FileName(FileInfo file)
        {
            return Path.Combine(file.DirectoryName??"", "." + file.Name + ".md5");
        }

        internal static string GetFileNameFromMD5(FileInfo file)
        {
            return Path.Combine(file.DirectoryName??"", Path.GetFileNameWithoutExtension(file.Name.TrimStart('.')));
        }

        internal static string GetErrorFileName(FileInfo file)
        {
            return file.FullName + ".error";
        }

        internal static string GetFileNameFromError(FileInfo file)
        {
            return Path.Combine(file.DirectoryName??"", Path.GetFileNameWithoutExtension(file.Name));
        }

        internal static void CreateHashFile(string hashFile, string hash, DateTime fileDateUtc)
        {
            if (File.Exists(hashFile)) File.SetAttributes(hashFile, FileAttributes.Normal);
            File.WriteAllText(hashFile, hash);
            File.SetLastWriteTimeUtc(hashFile, fileDateUtc);
            File.SetAttributes(hashFile, File.GetAttributes(hashFile) | FileAttributes.Hidden);
        }

        internal static async Task CreateHashFileAsync(string hashFile, string hash, DateTime fileDateUtc)
        {
            if (File.Exists(hashFile)) File.SetAttributes(hashFile, FileAttributes.Normal);
            await File.WriteAllTextAsync(hashFile, hash);
            File.SetLastWriteTimeUtc(hashFile, fileDateUtc);
            File.SetAttributes(hashFile, File.GetAttributes(hashFile) | FileAttributes.Hidden);
        }

        internal static bool IsValidImage(string file)
        {
            try
            {
                var image = Image.Load(file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static bool IsValidImage(FileInfo file)
        {
            if (file == null) return false;
            return IsValidImage(file.FullName);
        }

        internal static async Task<bool> IsValidImageAsync(string file)
        {
            try
            {
                var image = await Image.LoadAsync(file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static async Task<bool> IsValidImageAsync(FileInfo file)
        {
            if (file == null) return false;
            return await IsValidImageAsync(file.FullName);
        }

        internal static bool ValidateImage(FileInfo file)
        {
            if (Common.IsImageFile(file) && !IsValidImage(file))
            {
                Trace.WriteLine($"{file.FullName} - Invalid image!");
                File.WriteAllText(GetErrorFileName(file), $"{DateTime.Now}\tInvalid image!");
                return false;
            }
            return true;
        }

        internal static async Task<bool> ValidateImageAsync(FileInfo file)
        {
            if (Common.IsImageFile(file) && !(await IsValidImageAsync(file)))
            {
                Trace.WriteLine($"{file.FullName} - Invalid image!");
                await File.WriteAllTextAsync(GetErrorFileName(file), $"{DateTime.Now}\tInvalid image!");
                return false;
            }
            return true;
        }

        public static void HashFolder(DirectoryInfo folder, bool force = false)
        {
            Common.Msg($"Creating hash files for folder {folder}");

            var watch = Stopwatch.StartNew();

            var files = Common.GetFiles(folder, DEFAULT_FILTER, true);
            int fileCount = files.Count;
            int errCount = 0;

            var i = 0;
            var hashCount = 0;
            Common.Progress(0);
            Trace.Indent();

            ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 8 };
            var task = Parallel.ForEachAsync(files, parallelOptions, async (file, token) =>
            {
                i++;

                var md5File = GetMd5FileName(file.FileInfo);
                if (force || !File.Exists(md5File))
                {
                    if (!await ValidateImageAsync(file.FileInfo)) errCount++;
                    await CreateHashFileAsync(md5File, await ComputeHashAsync(file.FileInfo), file.LastWriteTimeUtc);
                    hashCount++;
                }
                else
                    File.SetAttributes(md5File, FileAttributes.Hidden);

                Common.Progress(100 * i / fileCount);
            });
            task.Wait();
            Trace.Unindent();

            var time = Common.GetTimeString(watch);
            if (errCount > 0)
                Common.Msg($"{hashCount} new hash files were created for {fileCount} files in {folder} folder, but there were {errCount} invalid images! ({time})", ConsoleColor.Red);
            else
                Common.Msg($"{hashCount} new hash files were created for {fileCount} files in {folder} folder... ({time})", ConsoleColor.Green);
        }

        public static void VerifyFolder(DirectoryInfo folder)
        {
            Common.Msg($"Verifying folder {folder}");

            var watch = Stopwatch.StartNew();

            var files = Common.GetFiles(folder, DEFAULT_FILTER, true);
            int fileCount = files.Count;
            int errCount = 0;
            int warnCount = 0;

            var i = 0;
            Common.Progress(0);
            Trace.Indent();

            ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 8 };
            var task = Parallel.ForEachAsync(files, parallelOptions, async (file, token) =>
            {
                i++;

                var md5File = GetMd5FileName(file.FileInfo);
                var errFile = GetErrorFileName(file.FileInfo);
                var fileDate = file.LastWriteTimeUtc;

                var newHash = await ComputeHashAsync(file.FileInfo);

                if (File.Exists(md5File))
                {
                    var oldHash = (await File.ReadAllTextAsync(md5File)).Trim();
                    if (string.Compare(oldHash, newHash, true) != 0)
                    {
                        errCount++;
                        Trace.WriteLine($"{file.FullName} - Invalid hash!");
                        await File.WriteAllTextAsync(errFile, $"{DateTime.Now}\tInvalid hash! It has changed from {oldHash} to {newHash}");
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
                    if (!await ValidateImageAsync(file.FileInfo)) errCount++;
                    await CreateHashFileAsync(md5File, newHash, fileDate);
                }

                Common.Progress(100 * i / fileCount);
            });
            task.Wait();
            Trace.Unindent();

            var time = Common.GetTimeString(watch);
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
            Common.Msg($"Comparing hashes of files in {folder} folder to {reference} folder");

            var watch = Stopwatch.StartNew();

            var files = Common.GetFiles(folder, DEFAULT_FILTER, true);
            int fileCount = files.Count;

            var i = 0;
            var verified = 0;
            var diff = 0;
            var diffDate = 0;
            var plus = 0;
            Common.Progress(0);
            Trace.Indent();

            ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 4 };
            var task = Parallel.ForEachAsync(files, parallelOptions, async (file, token) =>
            {
                i++;

                var dir = Path.GetDirectoryName(file.FullName);
                var relDir = Path.GetDirectoryName(Path.GetRelativePath(folder.FullName, file.FullName));
                var plusDir = Path.Combine(folder.FullName, "plus", relDir??"");
                var diffDir = Path.Combine(folder.FullName, "diff", relDir??"");

                var md5File = GetMd5FileName(file.FileInfo);
                if (File.Exists(md5File))
                {
                    verified++;
                    var hash = await File.ReadAllTextAsync(md5File);
                    var date = file.LastWriteTimeUtc;

                    var relPath = Path.GetRelativePath(folder.FullName, md5File);
                    var refMd5 = Path.Combine(reference.FullName, relPath);
                    var refFile = Path.Combine(reference.FullName, Path.GetDirectoryName(relPath)??"", file.Name);

                    if (File.Exists(refMd5))
                    {
                        var refHash = await File.ReadAllTextAsync(refMd5);
                        var refDate = File.GetLastWriteTimeUtc(refMd5);

                        var hashDiff = hash.Trim() != refHash.Trim();
                        var dateDiff = date != refDate;

                        if (hashDiff && dateDiff)
                        {
                            diff++;
                            diffDate++;
                            Trace.WriteLine($"{file.FullName} - Date and content has changed!");
                            var task1 = CopyFileToFolderAsync(diffDir, file.FullName);
                            var task2 = CopyFileToFolderAsync(diffDir, refFile, "_ref");
                            Task.WaitAll(task1, task2);
                        }
                        else if (hashDiff)
                        {
                            diff++;
                            Trace.WriteLine($"{file.FullName} - Content has changed!");
                            var task1 = CopyFileToFolderAsync(diffDir, file.FullName);
                            var task2 = CopyFileToFolderAsync(diffDir, refFile, "_ref");
                            Task.WaitAll(task1, task2);
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
                        await CopyFileToFolderAsync(plusDir, file.FullName);
                    }
                }

                Common.Progress(100 * i / fileCount);
            });
            task.Wait();

            Trace.Unindent();

            var time = Common.GetTimeString(watch);
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

            static async Task CopyFileToFolderAsync(string destFolder, string sourceFile, string postFix = "")
            {
                if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);
                var destFile = Path.Combine(destFolder, Path.GetFileNameWithoutExtension(sourceFile) + postFix + Path.GetExtension(sourceFile));
                var destMd5 = GetMd5FileName(new FileInfo(destFile));

                await Common.CopyFileAsync(sourceFile, destFile);
            }
        }
    }
}
