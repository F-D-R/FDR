using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FDR.Tools.Library
{
    public static class Raw
    {
        public static void CleanupFolder(DirectoryInfo folder)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Common.Msg($"Cleaning up folder {folder}");

            var worker = new CleanupWorker(folder);
            worker.CleanupErrorFiles();
            worker.CleanupHashFiles();
            worker.CleanupRawFiles();

            var time = Common.GetTimeString(stopwatch);
            Common.Msg($"Cleanup of {folder} folder succeeded: {worker.RawCount} raw, {worker.HashCount} hash and {worker.ErrCount} error files were deleted. ({time})", ConsoleColor.Green);
        }

        private sealed class CleanupWorker
        {
            internal CleanupWorker(DirectoryInfo folder) => Folder = folder;

            private const string DEFAULT_RAW_FOLDER = "RAW";

            private DirectoryInfo Folder { get; }

            private int rawCount = 0;
            public int RawCount => rawCount;

            public void IncrementRawCount() { lock (this) rawCount++; }

            private int hashCount = 0;
            public int HashCount => hashCount;

            public void IncrementHashCount() { lock (this) hashCount++; }

            private int errCount = 0;
            public int ErrCount => errCount;

            public void IncrementErrCount() { lock (this) errCount++; }

            public void CleanupRawFiles()
            {
                var files = Common.EnumerateFiles(Folder, "*.CR?|*.DNG", true);

                var jpgOptions = new EnumerationOptions
                {
                    MatchCasing = MatchCasing.CaseInsensitive,
                    RecurseSubdirectories = false
                };

                var i = 0;
                Trace.Indent();

                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = 4
                };

                Parallel.ForEach(files, parallelOptions, (file, token) =>
                {
                    i++;
                    if (i % 10 == 0) Progress(i);

                    // Handle only RAW folder files:
                    if (string.Compare(file.Directory?.Name, DEFAULT_RAW_FOLDER, true) != 0) return;

                    var rawFolder = file.Directory;
                    var jpgFolder = rawFolder?.Parent;
                    if (jpgFolder == null) return;

                    // Exit if JPG folder is empty, i.e. the RAW files are not jet converted:
                    if (!jpgFolder.EnumerateFiles("*.JPG", jpgOptions).Any()) return;

                    // Exit if there is a JPG file:
                    if (Directory.EnumerateFiles(jpgFolder.FullName, Path.GetFileNameWithoutExtension(file.Name) + "*.jpg", jpgOptions).Any()) return;

                    Trace.WriteLine($"Deleting raw file: {file.FullName}");
                    IncrementRawCount();
                    file.Delete();

                    var hashFile = Verify.GetMd5FileName(file);
                    if (File.Exists(hashFile))
                    {
                        Trace.WriteLine($"Deleting raw hash file: {hashFile}");
                        IncrementHashCount();
                        File.Delete(hashFile);
                    }

                    var errFile = Verify.GetErrorFileName(file);
                    if (File.Exists(errFile))
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

            public void CleanupHashFiles()
            {
                var options = new EnumerationOptions
                {
                    MatchCasing = MatchCasing.CaseInsensitive,
                    RecurseSubdirectories = true,
                    AttributesToSkip = FileAttributes.System
                };
                var files = Folder.EnumerateFiles("*.md5", options);

                var i = 0;
                Trace.Indent();

                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = 4
                };

                Parallel.ForEach(files, parallelOptions, (file, token) =>
                {
                    i++;

                    if (!File.Exists(Verify.GetFileNameFromMD5(file)))
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

            public void CleanupErrorFiles()
            {
                var options = new EnumerationOptions
                {
                    MatchCasing = MatchCasing.CaseInsensitive,
                    RecurseSubdirectories = true,
                    AttributesToSkip = FileAttributes.System
                };
                var files = Folder.EnumerateFiles("*.error", options);

                var i = 0;
                Trace.Indent();

                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = 4
                };

                Parallel.ForEach(files, parallelOptions, (file, token) =>
                {
                    i++;

                    if (!File.Exists(Verify.GetFileNameFromError(file)))
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
    }
}
