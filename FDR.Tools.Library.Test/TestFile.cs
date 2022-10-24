using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace FDR.Tools.Library.Test
{
    public class TestFile
    {
        public TestFile() { Created = DateTime.Now; }

        public bool Keep = true;
        public bool Create = true;
        public DateTime Created;
        public string SourceFolder;
        public string SourceName;
        public string DestFolder;
        public string DestName;

        public string Name { get { return DestName; } }

        public string GetSourcePath() => System.IO.Path.Combine(SourceFolder, SourceName);

        public string GetDestPath() => System.IO.Path.Combine(DestFolder, DestName);

        public string GetPath() => GetDestPath();
    }

    public class TestFiles : List<TestFile>
    {
        public void Add(string sourceFolder, string sourceName, string destFolder, string destName)
        {
            base.Add(new TestFile() { Keep = true, Create = true, SourceFolder = sourceFolder, SourceName = sourceName, DestFolder = destFolder, DestName = destName });
        }

        public void Add(DateTime created, string sourceFolder, string sourceName, string destFolder, string destName)
        {
            base.Add(new TestFile() { Keep = true, Create = true, Created = created, SourceFolder = sourceFolder, SourceName = sourceName, DestFolder = destFolder, DestName = destName });
        }

        public void Add(string folder, string name, bool keep = true)
        {
            base.Add(new TestFile() { Keep = keep, Create = true, SourceFolder = folder, DestFolder = folder, SourceName = name, DestName = name });
        }

        public void CreateFiles()
        {
            this.Where(f => f.Create).ToList().ForEach(f => { File.WriteAllText(f.GetSourcePath(), ""); File.SetCreationTimeUtc(f.GetSourcePath(), f.Created); File.SetLastWriteTimeUtc(f.GetSourcePath(), f.Created); });
        }
    }

    internal static class Helper
    {
        public static void CreateJpgFile(string filePath, int width = 200, int height = 200)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

            using (var image = new Image<Argb32>(width, height))
            {
                image.Metadata.ExifProfile = new ExifProfile();
                var dt = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
                image.Metadata.ExifProfile.SetValue(ExifTag.DateTimeOriginal, dt);
                image.SaveAsJpeg(filePath);
            }
        }
    }
}
