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
        public TestFile() { Created = DateTime.Now; Modified = Created; Exif = Created; }

        public bool Keep = true;
        public bool Create = true;
        public DateTime Created;
        public DateTime Modified;
        public DateTime Exif;
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
        public void Add(string folder, string name, bool keep = true)
        {
            base.Add(new TestFile() { Keep = keep, Create = true, SourceFolder = folder, DestFolder = folder, SourceName = name, DestName = name });
        }

        public void Add(string sourceFolder, string sourceName, string destFolder, string destName, DateTime? created = null, DateTime? modified = null, DateTime? exif = null)
        {
            var tf = new TestFile();
            tf.Keep = true;
            tf.Create = true;
            tf.SourceFolder = sourceFolder;
            tf.SourceName = sourceName;
            tf.DestFolder = destFolder;
            tf.DestName = destName;
            if (created != null) tf.Created = created.Value;
            if (modified != null) tf.Modified = modified.Value;
            if (exif != null) tf.Exif = exif.Value;
            base.Add(tf);
        }

        public void CreateFiles()
        {
            this.Where(f => f.Create).ToList().ForEach(f =>
            {
                if (!Directory.Exists(f.SourceFolder)) Directory.CreateDirectory(f.SourceFolder);
                File.WriteAllText(f.GetSourcePath(), "");
                File.SetCreationTime(f.GetSourcePath(), f.Created);
                File.SetLastWriteTime(f.GetSourcePath(), f.Modified);
            });
        }

        public void DeleteSourceFiles()
        {
            this.ToList().ForEach(f =>
            {
                if (File.Exists(f.GetSourcePath())) File.Delete(f.GetSourcePath());
            });
        }
    }

    internal static class Helper
    {
        public static void CreateJpgFile(string filePath, int width = 200, int height = 200, DateTime? exifdate = null)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

            using (var image = new Image<Argb32>(width, height))
            {
                image.Metadata.ExifProfile = new ExifProfile();
                var dt = DateTime.Now;
                if (exifdate != null) dt = exifdate.Value;
                var dts = dt.ToString("yyyy:MM:dd HH:mm:ss");
                image.Metadata.ExifProfile.SetValue(ExifTag.DateTimeOriginal, dts);
                image.SaveAsJpeg(filePath);
            }
        }
    }
}
