using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FDR.UI.Models
{
    internal class ImageFile : IDisposable
    {
        public ImageFile(FileInfo imageFileInfo) => ImageFileInfo = imageFileInfo;

        public void Dispose()
        {
            DisposeImage();
            thumbnail?.Dispose();
            thumbnail = null;
        }

        public void DisposeImage()
        {
            image?.Dispose();
            image = null;
        }

        public FileInfo ImageFileInfo { get; }

        public string ImageFileName => ImageFileInfo.Name;

        private Image? image;
        public Image? Image
        {
            get
            {
                if (image == null)
                {
                    var token = new CancellationToken();
                    LoadImageAsync(token)?.Wait(token);
                }
                return image;
            }
        }

        public async Task LoadImageAsync(CancellationToken cancellationToken)
        {
            if (image == null && ImageFileInfo != null)
            {
                var img = await Image.LoadAsync(ImageFileInfo.FullName, cancellationToken);
                //ClearMetadata(img);

                lock (this)
                {
                    image = img;
                }
            }
        }

        private Image? thumbnail;
        public Image? Thumbnail
        {
            get
            {
                if (thumbnail == null)
                {
                    var token = new CancellationToken();
                    LoadThumbnailAsync(token)?.Wait(token);
                }
                return thumbnail;
            }
        }

        public async Task LoadThumbnailAsync(CancellationToken cancellationToken)
        {
            if (thumbnail == null && ImageFileInfo != null)
            {
                var img = await Image.LoadAsync(ImageFileInfo.FullName, cancellationToken);
                img.Mutate(i => i.Resize(new ResizeOptions() { Size = new Size(100, 100), Mode = ResizeMode.Max }));
                ClearMetadata(img);

                lock (this)
                {
                    thumbnail = img;
                }
            }

        }

        private void ClearMetadata(Image image)
        {
            var md = image.Metadata;
            md.ExifProfile = null;
            md.IccProfile = null;
            md.XmpProfile = null;
            md.IptcProfile = null;
        }
    }
}
