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
        public static void ShowResizeHelp()
        {
            Common.Msg("");
            Common.Msg("Resizes the files matching a filter in the folder given after the -resize option based on a ResizeConfig and saves the resized files with a configurable new name.");
            Common.Msg("");
            Common.Msg("The resize function can have the following attributes in a ResizeConfigs object:");
            Common.ShowAttributeHelp(ResizeConfig.GetResizeConfigAttributeList());
            Rename.ShowFilenamePatternHelp();
        }

        public static void ResizeFile(FileInfo file, int counter, ResizeConfig config, int progressPercent)
        {
            if (config == null) throw new ArgumentNullException("config");
            config.Validate();
            if (file == null) throw new ArgumentNullException("file");
            if (!file.Exists) throw new FileNotFoundException("File doesn't exist!", file.FullName);

            var path = Path.GetDirectoryName(file.FullName)??"";
            var newFullName = Rename.CalculateFileName(file, config.GetNewRenameConfig(), counter);
            if (string.Compare(Path.GetExtension(newFullName), ".jpg", true) != 0)
                newFullName = Path.Combine(path, Path.GetFileNameWithoutExtension(newFullName) + ".jpg");

            var destFolder = Path.GetDirectoryName(newFullName);
            if (destFolder != null && !Directory.Exists(destFolder))
            {
                Trace.WriteLine($"Creating destination folder {destFolder}");
                Directory.CreateDirectory(destFolder);
            }

#if RELEASE
            Trace.WriteLine($"Resizing file {file.Name} to {Path.GetFileName(newFullName)}");
#else
            Trace.WriteLine($"Resizing file {file.FullName} to {newFullName}");
#endif
            Common.Progress(progressPercent);

            int maxWidth = config.MaxWidth;
            int maxHeight = config.MaxHeight;

            using (Image image = Image.Load(file.FullName))
            {
                ResizeMode resizeMode = ResizeMode.Max;
                bool resize = true;
                switch (config.ResizeMethod)
                {
                    case ResizeMethod.fit_in:
                        resize = (image.Width != maxWidth && image.Height != maxHeight) || (image.Width == maxWidth && image.Height > maxHeight) || (image.Height == maxHeight && image.Width > maxWidth);
                        break;
                    case ResizeMethod.max_width:
                        resize = image.Width != maxWidth;
                        maxHeight = int.MaxValue;
                        break;
                    case ResizeMethod.max_height:
                        resize = image.Height != maxHeight;
                        maxWidth = int.MaxValue;
                        break;
                    case ResizeMethod.stretch:
                        resize = image.Width != maxWidth || image.Height != maxHeight;
                        resizeMode = ResizeMode.Stretch;
                        break;
                }

                if (resize)
                    image.Mutate(x => x.Resize(new ResizeOptions() { Size = new Size(maxWidth, maxHeight), Mode = resizeMode }));

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

        public static void ResizeFilesInFolder(DirectoryInfo folder, ResizeConfig config)
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
            foreach (var file in files.OrderBy(f => f.FullName).ToList())
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
