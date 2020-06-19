using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                Msg($"    {percent}% ({percent}%)      \r", ConsoleColor.Gray, false);
            else
                Msg($"    {percent}%                   \r", ConsoleColor.Gray, false);
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
                Common.Msg("Folder must be an existing one!", ConsoleColor.Red);
                return false;
            }
            return true;
        }

        internal static List<FileInfo> GetFiles(DirectoryInfo folder, ImportConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();

            return GetFiles(folder, config.FileFilter);
        }

        internal static List<FileInfo> GetFiles(DirectoryInfo folder, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) throw new ArgumentNullException("filter");
            if (folder == null) throw new ArgumentNullException("folder");
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var files = new List<FileInfo>();
            var filters = filter.Split('|');
            var options = new EnumerationOptions();
            options.MatchCasing = MatchCasing.CaseInsensitive;
            options.RecurseSubdirectories = true;
            foreach (var tmpfilter in filters)
            {
                files.AddRange(folder.GetFiles(tmpfilter, options));
            }

            return files.OrderBy(f => f.CreationTimeUtc).ToList();
        }

    }
}
