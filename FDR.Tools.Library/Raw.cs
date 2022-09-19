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

        private class CleanupWorker
        {
            internal CleanupWorker(DirectoryInfo folder) => Folder = folder;

            private const string DEFAULT_RAW_FOLDER = "RAW";

            private DirectoryInfo Folder { get; }

            public int RawCount { get; private set; } = 0;

            public int HashCount { get; private set; } = 0;

            public int ErrCount { get; private set; } = 0;

            public void CleanupRawFiles()
            {
                var rawOptions = new EnumerationOptions
                {
                    MatchCasing = MatchCasing.CaseInsensitive,
                    RecurseSubdirectories = true
                };
                var files = Folder.EnumerateFiles("*.CR?", rawOptions);

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

                    if (string.Compare(file.Directory?.Name, DEFAULT_RAW_FOLDER, true) == 0)
                    {
                        var rawFolder = file.Directory;
                        var jpgFolder = rawFolder?.Parent;

                        if (jpgFolder != null)
                        {
                            if (jpgFolder.EnumerateFiles().Any() && !Directory.EnumerateFiles(jpgFolder.FullName, Path.GetFileNameWithoutExtension(file.Name) + "*.jpg", jpgOptions).Any())
                            {
                                Trace.WriteLine($"Deleting raw file: {file.FullName}");
                                RawCount++;
                                file.Delete();

                                var hashFile = Verify.GetMd5FileName(file);
                                if (File.Exists(hashFile))
                                {
                                    Trace.WriteLine($"Deleting raw hash file: {hashFile}");
                                    HashCount++;
                                    File.Delete(hashFile);
                                }

                                var errFile = Verify.GetErrorFileName(file);
                                if (File.Exists(errFile))
                                {
                                    Trace.WriteLine($"Deleting raw error file: {errFile}");
                                    ErrCount++;
                                    File.Delete(errFile);
                                }

                                if (rawFolder != null && rawFolder.Exists && !rawFolder.EnumerateFiles().Any())
                                {
                                    Trace.WriteLine($"Deleting raw folder: {rawFolder}");
                                    rawFolder.Delete();
                                }
                            }
                        }
                    }

                    if (i % 10 == 0)
                        Progress(i);
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

                    var imageFile = Verify.GetFileNameFromMD5(file);
                    if (!File.Exists(imageFile))
                    {
                        Trace.WriteLine($"Deleting hash file: {file.FullName}");
                        HashCount++;
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

                    var imageFile = Verify.GetFileNameFromError(file);
                    if (!File.Exists(imageFile))
                    {
                        Trace.WriteLine($"Deleting error file: {file.FullName}");
                        ErrCount++;
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
