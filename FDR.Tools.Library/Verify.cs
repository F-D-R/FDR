﻿using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace FDR.Tools.Library
{
    public static class Verify
    {
        private static string ComputeHash(FileInfo file)
        {
            using (var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(fileStream);
                return Convert.ToBase64String(hash);
            }
        }

        public static void HashFolder(string folder)
        {

        }

        public static void VerifyFolder(string folder)
        {
            var start = DateTime.Now.Ticks;

            var di = new DirectoryInfo(folder);
            var files = Common.GetFiles(di, "*.CR2|*.CRW|*.JPG|*.MP4|*.AVI|*.MOV");
            int fileCount = files.Count;
            int errCount = 0;
            int warnCount = 0;

            Common.Msg($"Verifying folder {folder}");

            var i = 0;
            Common.Progress(0);

            Trace.Indent();
            foreach (var file in files)
            {
                var md5File = Path.Combine(file.DirectoryName, "." + file.Name + ".md5");
                var errFile = file.FullName + ".error";
                var fileDate = file.LastWriteTimeUtc;

                var newHash = ComputeHash(file);

                if (File.Exists(md5File))
                {
                    var oldHash = File.ReadAllText(md5File);
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
                    File.WriteAllText(md5File, newHash);
                    File.SetAttributes(md5File, File.GetAttributes(md5File) | FileAttributes.Hidden);
                    File.SetLastWriteTimeUtc(md5File, fileDate);
                }

                i++;
                Common.Progress(100 * i / fileCount);
            }
            Trace.Unindent();

            var ms = (DateTime.Now.Ticks - start) / 10000;
            string time = (ms < 1000) ? $"{ms}ms" : $"{ms / 1000}s";

            if (errCount > 0)
            {
                if (warnCount > 0)
                    Common.Msg($"Verification of {fileCount} files in {folder} folder resulted in {errCount} errors and {warnCount} warnings! ({time})", ConsoleColor.Red);
                else
                    Common.Msg($"Verification of {fileCount} files in {folder} folder resulted in {errCount} errors! ({time})", ConsoleColor.Red);
            }
            else if (warnCount > 0)
            {
                Common.Msg($"Verification of {fileCount} files in {folder} folder succeeded with {warnCount} warnings... ({time})", ConsoleColor.Yellow);
            }
            else
            {
                Common.Msg($"Verification of {fileCount} files in {folder} folder succeeded... ({time})", ConsoleColor.Green);
            }
        }
    }
}
