using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FDR.Tools.Library
{
    public static class Raw
    {
        public static void CleanupFolder(DirectoryInfo folder, CancellationToken token)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Common.Msg($"Cleaning up folder {folder}");

            var files = Common.GetFilesWithOutput(folder, "*.*", true);

            var worker = new CleanupWorker(folder, files);
            try
            {
                worker.CleanupErrorFiles(token);
                worker.CleanupHashFiles(token);
                worker.CleanupRawFiles(token);
            }
            catch (OperationCanceledException) { }

            var time = Common.GetTimeString(stopwatch);
            Common.Msg($"Cleanup of {folder} folder succeeded: {worker.RawCount} raw, {worker.HashCount} hash and {worker.ErrCount} error files were deleted. ({time})", ConsoleColor.Green);
        }

        private sealed class CleanupWorker
        {
            internal CleanupWorker(DirectoryInfo folder, List<ExifFile> files)
            {
                Folder = folder;
                Files = files;
            }

            private const string DEFAULT_RAW_FOLDER = "RAW";

            private DirectoryInfo Folder { get; }

            private List<ExifFile> Files { get; }

            private int rawCount = 0;
            public int RawCount => rawCount;

            public void IncrementRawCount() { lock (this) rawCount++; }

            private int hashCount = 0;
            public int HashCount => hashCount;

            public void IncrementHashCount() { lock (this) hashCount++; }

            private int errCount = 0;
            public int ErrCount => errCount;

            public void IncrementErrCount() { lock (this) errCount++; }

            public void CleanupRawFiles(CancellationToken token)
            {
                var files = Common.GetFiles(Files, Folder, "*.CR?|*.DNG", true);

                var i = 0;
                Trace.Indent();
                Common.Msg($"    Raw files to process: {files.Count}", ConsoleColor.Gray);

                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = 4,
                    CancellationToken = token
                };

                var folderContainsJpg = new ConcurrentDictionary<string, bool>();

                Parallel.ForEach(files, parallelOptions, (file, token) =>
                {
                    lock (this) { i++; }
                    if (i % 10 == 0) Progress(i);

                    // Handle only RAW folder files:
                    if (string.Compare(file.Directory?.Name, DEFAULT_RAW_FOLDER, true) != 0) return;

                    var rawFolder = file.Directory;
                    var jpgFolder = rawFolder?.Parent;
                    if (jpgFolder == null) return;

                    // Exit if JPG folder is empty, i.e. the RAW files are not jet converted:
                    lock (this)
                    {
                        if (folderContainsJpg.ContainsKey(jpgFolder.FullName))
                        {
                            if (!folderContainsJpg[jpgFolder.FullName]) return;
                        }
                        else
                        {
                            folderContainsJpg[jpgFolder.FullName] = Common.GetFiles(Files, jpgFolder, "*.JPG", false).Any();
                            if (!folderContainsJpg[jpgFolder.FullName]) return;
                        }
                    }

                    // Exit if there is a JPG file:
                    if (Common.GetFiles(Files, jpgFolder, Path.GetFileNameWithoutExtension(file.Name) + "*.jpg", false).Any()) return;

                    Trace.WriteLine($"Deleting raw file: {file.FullName}");
                    IncrementRawCount();
                    file.Delete();

                    var hashFile = Verify.GetMd5FileName(file.FileInfo);
                    if (Common.GetFiles(Files, rawFolder!, hashFile, false).Any())
                    {
                        Trace.WriteLine($"Deleting raw hash file: {hashFile}");
                        IncrementHashCount();
                        File.Delete(hashFile);
                    }

                    var errFile = Verify.GetErrorFileName(file.FileInfo);
                    if (Common.GetFiles(Files, rawFolder!, errFile, false).Any())
                    {
                        Trace.WriteLine($"Deleting raw error file: {errFile}");
                        IncrementErrCount();
                        File.Delete(errFile);
                    }

                    lock (this)
                    {
                        if (rawFolder != null && rawFolder.Exists && !rawFolder.EnumerateFiles().Any())
                        {
                            Trace.WriteLine($"Deleting raw folder: {rawFolder}");
                            rawFolder.Delete();
                        }
                    }
                });

                Progress(i, true);
                Trace.Unindent();

                void Progress(int done, bool newline = false)
                {
                    Common.Msg($"    Raw files processed: {done}                  \r", ConsoleColor.Gray, newline);
                }
            }

            public void CleanupHashFiles(CancellationToken token)
            {
                var files = Common.GetFiles(Files, Folder, "*.md5", true);

                var i = 0;
                Trace.Indent();
                Common.Msg($"    Hash files to process: {files.Count}", ConsoleColor.Gray);

                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = 4,
                    CancellationToken = token
                };

                Parallel.ForEach(files, parallelOptions, (file, token) =>
                {
                    lock (this) { i++; }

                    if (!File.Exists(Verify.GetFileNameFromMD5(file.FileInfo)))
                    {
                        Trace.WriteLine($"Deleting hash file: {file.FullName}");
                        IncrementHashCount();
                        file.Delete();
                    }

                    if (i % 10 == 0)
                        Progress(i);
                });

                Progress(i, true);
                Trace.Unindent();

                void Progress(int done, bool newline = false)
                {
                    Common.Msg($"    Hash files processed: {done}                  \r", ConsoleColor.Gray, newline);
                }
            }

            public void CleanupErrorFiles(CancellationToken token)
            {
                var files = Common.GetFiles(Files, Folder, "*.error", true);

                var i = 0;
                Trace.Indent();
                Common.Msg($"    Error files to process: {files.Count}", ConsoleColor.Gray);

                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = 4,
                    CancellationToken = token
                };

                Parallel.ForEach(files, parallelOptions, (file, token) =>
                {
                    lock (this) { i++; }

                    if (!File.Exists(Verify.GetFileNameFromError(file.FileInfo)))
                    {
                        Trace.WriteLine($"Deleting error file: {file.FullName}");
                        IncrementErrCount();
                        file.Delete();
                    }

                    if (i % 10 == 0)
                        Progress(i);
                });

                Progress(i, true);
                Trace.Unindent();

                void Progress(int done, bool newline = false)
                {
                    Common.Msg($"    Error files processed: {done}                  \r", ConsoleColor.Gray, newline);
                }
            }
        }

        public static void PrintFolderInfo(DirectoryInfo folder, CancellationToken token)
        {
            Common.Msg($"Printing folder info for {folder}");

            //Stopwatch stopwatch = Stopwatch.StartNew();

            var files = Common.GetFilesWithOutput(folder, "*.*", true);
            Common.Msg("");

            var folderNames = files.Select(f => f.DirectoryName).Distinct().OrderBy(d => d).ToList();
            foreach (var folderName in folderNames)
            {
                Common.Msg($"    {folderName}");
                var folderFiles = files.Where(f => f.DirectoryName == folderName);
                var size = folderFiles.Select(f => f.FileInfo.Length).Sum();
                Common.Msg($"        Sum: {folderFiles.Count()} files {FormatSize(size)}");
                var filesByExt = files.Where(f => f.DirectoryName == folderName).GroupBy(f => f.FileInfo.Extension).Select(g => new { Ext = g.Key, Count = g.Count(), Size = g.Sum(gi => gi.FileInfo.Length) });
                foreach (var extStats in filesByExt)
                {
                    Common.Msg($"        {extStats.Ext}: {extStats.Count} files {FormatSize(extStats.Size)}");
                }
            }
            Common.Msg("");

            Common.Msg($"{folder}");
            Common.Msg($"    Sum: {files.Count} files {FormatSize(files.Select(f => f.FileInfo.Length).Sum())}");
            var allFilesByExt = files.GroupBy(f => f.FileInfo.Extension).Select(g => new { Ext = g.Key, Count = g.Count(), Size = g.Sum(gi => gi.FileInfo.Length) });
            foreach (var extStats in allFilesByExt)
            {
                Common.Msg($"    {extStats.Ext}: {extStats.Count} files {FormatSize(extStats.Size)}");
            }


            string FormatSize(long size)
            {
                if (size < 1024L) return $"{size}B";

                var dblsize = size / 1024.0;
                if (dblsize < 1024) return $"{Math.Round(dblsize, 3)}kB";

                dblsize /= 1024.0;
                if (dblsize < 1024) return $"{Math.Round(dblsize, 3)}MB";

                dblsize /= 1024.0;
                if (dblsize < 1024) return $"{Math.Round(dblsize, 3)}GB";

                dblsize /= 1024.0;
                if (dblsize < 1024) return $"{Math.Round(dblsize, 3)}TB";

                return $"{size}B";
            }
        }
    }
}
