using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public static async Task ResizeFileAsync(FileInfo file, string destFullName, ResizeConfig config)
        {
            ArgumentNullException.ThrowIfNull(config);
            config.Validate();
            ArgumentNullException.ThrowIfNull(file);
            if (!file.Exists) throw new FileNotFoundException("File doesn't exist!", file.FullName);

            var destFolder = Path.GetDirectoryName(destFullName);
            if (destFolder != null && !Directory.Exists(destFolder))
            {
                Trace.WriteLine($"Creating destination folder {destFolder}");
                Directory.CreateDirectory(destFolder);
            }

#if RELEASE
            Trace.WriteLine($"Resizing file {file.Name} to {Path.GetFileName(destFullName)}");
#else
            Trace.WriteLine($"Resizing file {file.FullName} to {destFullName}");
#endif

            int maxWidth = config.MaxWidth;
            int maxHeight = config.MaxHeight;

            using (Image image = await Image.LoadAsync(file.FullName))
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
                await image.SaveAsync(destFullName, encoder);
            }

            static void ClearMetadata(Image image)
            {
                var md = image.Metadata;
                md.ExifProfile = null;
                md.IccProfile = null;
                md.XmpProfile = null;
                md.IptcProfile = null;
            }
        }

        public static void ResizeFilesInFolder(DirectoryInfo folder, ResizeConfig config, List<ExifFile>? allFiles = null)
        {
            ArgumentNullException.ThrowIfNull(config);
            config.Validate();
            ArgumentNullException.ThrowIfNull(folder);
            if (!folder.Exists) throw new DirectoryNotFoundException($"Folder doesn't exist! ({folder.FullName})");

            var filter = config.FileFilter;
            if (string.IsNullOrWhiteSpace(filter)) filter = "*.*";
            Common.Msg($"Resizing {filter} files in {folder.FullName}");
            Trace.Indent();

            var dirFiles = Common.GetFilesWithOutput(folder, "*.*", false);
            if (allFiles == null)
                allFiles = dirFiles;
            else
                allFiles.Merge(dirFiles);

            var files = Common.GetFiles(allFiles, folder, filter, false);
            int fileCount = files.Count;

            //TODO: parallel exif loading if necessary
            //TODO: configurable order

            int counter = 1;
            Common.Progress(0);
            foreach (var file in files.OrderBy(f => f.FullName).ToList())
            {
                try
                {
                    var path = Path.GetDirectoryName(file.FullName)??"";
                    var newFullName = Rename.CalculateFileName(file, config.GetNewRenameConfig(), counter);
                    if (string.Compare(Path.GetExtension(newFullName), ".jpg", true) != 0)
                        newFullName = Path.Combine(path, Path.GetFileNameWithoutExtension(newFullName) + ".jpg");

                    //ResizeFileAsync(file.FileInfo, newFullName, config);
                    ResizeFileAsync(file.FileInfo, newFullName, config).Wait();
                    //Task.Run(async () => await Resize.ResizeFileAsync(file.FileInfo, newFullName, config)).Wait();

                    Common.Progress(100 * counter / fileCount);
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
