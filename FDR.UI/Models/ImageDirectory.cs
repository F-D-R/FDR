using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using SixLabors.ImageSharp;
using FDR.Tools.Library;

namespace FDR.UI.Models
{
    internal class ImageDirectory : IDisposable
    {
        public ImageDirectory(DirectoryInfo imageDirectoryInfo) => ImageDirectoryInfo = imageDirectoryInfo;

        public void Dispose()
        {
            DisposeImageFiles();
        }

        private void DisposeImageFiles()
        {
            ImageFiles?.ToList().ForEach(i => i.Value?.Dispose());
            ImageFiles?.Clear();
            ImageFiles = null;
        }

        private DirectoryInfo? imageDirectoryInfo;

        public DirectoryInfo? ImageDirectoryInfo
        {
            get
            {
                return imageDirectoryInfo;
            }
            set
            {
                if (imageDirectoryInfo?.FullName != value?.FullName)
                {
                    imageDirectoryInfo = value;
                    DisposeImageFiles();
                    //TODO: cancel previous loading
                }
            }
        }

        public SortedDictionary<string, ImageFile>? ImageFiles { get; private set; }

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
            if (ImageDirectoryInfo != null)
            {
                if (ImageFiles == null)
                {
                    ImageFiles = new SortedDictionary<string, ImageFile>();
                    var files = Common.GetFiles(ImageDirectoryInfo, "*.JPG", false);
                    files.ForEach(f => ImageFiles.Add(f.Name, new ImageFile(f)));
                }

                var tasks = ImageFiles.Where(i => i.Value.Thumbnail == null)
                                      .Select(i => i.Value.LoadThumbnailAsync(cancellationToken));
                await Task.WhenAll(tasks);
            }
        }

    }
}
