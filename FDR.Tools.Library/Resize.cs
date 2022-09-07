using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace FDR.Tools.Library
{
    public static class Resize
    {
        public static void ResizeFile(FileInfo file, int counter, BatchResizeConfig config, int progressPercent)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();
            if (file == null) throw new ArgumentNullException("file");
            if (!file.Exists) throw new FileNotFoundException("File doesn't exist!", file.FullName);

            var path = Path.GetDirectoryName(file.FullName)??"";
            var newFullName = Rename.CalculateFileName(file, config, counter);

            Trace.WriteLine($"Resizing file {file.Name} to {Path.GetFileName(newFullName)}");
            Common.Progress(progressPercent);

            int maxWidth = config.MaxWidth;
            int maxHeight = config.MaxHeight;

            ResizeMode resizeMode = ResizeMode.Max;
            switch (config.ResizeMethod)
            {
                case ResizeMethod.max_width:
                    maxHeight = int.MaxValue;
                    resizeMode = ResizeMode.Min;
                    break;
                case ResizeMethod.max_height:
                    maxWidth = int.MaxValue;
                    resizeMode = ResizeMode.Min;
                    break;
                case ResizeMethod.exact:
                    resizeMode = ResizeMode.Manual;
                    break;
            }

            using (Image image = Image.Load(file.FullName))
            {
                image.Mutate(x => x.Resize(new ResizeOptions() { Size = new Size(config.MaxWidth, config.MaxHeight), Mode = resizeMode }));
                if (config.ClearMetadata) ClearMetadata(image);

                var encoder = new JpegEncoder() { Quality = config.JpgQuality };
                image.Save(newFullName, encoder);
            }

            void ClearMetadata(Image image)
            {
                var md = image.Metadata;
                md.ExifProfile = null;
                md.IccProfile = null;
                md.XmpProfile = null;
                md.IptcProfile = null;
            }
        }

        public static void ResizeFilesInFolder(DirectoryInfo folder, BatchResizeConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();
            if (folder == null) throw new ArgumentNullException("folder");
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var filter = config.FileFilter;
            Common.Msg($"Resizing {filter} files in {folder.FullName}");
            Trace.Indent();

            if (string.IsNullOrWhiteSpace(filter)) filter = "*.*";
            var files = Common.GetFiles(folder, filter, false);
            int fileCount = files.Count;

            int counter = 1;
            Common.Progress(0);
            foreach (var file in files.OrderBy(f => f.CreationTimeUtc).ToList())
            {
                try
                {
                    ResizeFile(file, counter, config, 100 * counter / fileCount);
                }
                catch (IOException)
                {
                    if (config.StopOnError) throw;
                }
                counter++;
            }

            Trace.Unindent();
        }
    }
}
