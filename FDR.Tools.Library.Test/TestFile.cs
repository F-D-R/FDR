using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using FluentAssertions;

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
        public int Width = 200;
        public int Height = 200;

        public string Name { get { return DestName; } }

        public string GetSourcePath() => System.IO.Path.Combine(SourceFolder, SourceName);

        public string GetDestPath() => System.IO.Path.Combine(DestFolder, DestName);

        public string GetPath() => GetDestPath();

        public void CreateJpgFile()
        {
            using (var image = new Image<Argb32>(Width, Height))
            {
                const string EXIF_DATE_FORMAT = "yyyy:MM:dd HH:mm:ss";
                image.Metadata.ExifProfile = new ExifProfile();

                image.Metadata.ExifProfile.SetValue(ExifTag.DateTime, Exif.ToString(EXIF_DATE_FORMAT));
                image.Metadata.ExifProfile.SetValue(ExifTag.DateTimeOriginal, Exif.ToString(EXIF_DATE_FORMAT));
                image.Metadata.ExifProfile.SetValue(ExifTag.DateTimeDigitized, Exif.ToString(EXIF_DATE_FORMAT));
                image.SaveAsJpeg(GetSourcePath());
                File.SetCreationTime(GetSourcePath(), Created);
                File.SetLastWriteTime(GetSourcePath(), Modified);
            }
        }

        public void CreateFile()
        {
            if (!Directory.Exists(SourceFolder)) Directory.CreateDirectory(SourceFolder);
            if (Path.GetExtension(GetSourcePath())?.ToLower() == ".jpg")
                CreateJpgFile();
            else
            {
                File.WriteAllText(GetSourcePath(), GetSourcePath());
                File.SetCreationTime(GetSourcePath(), Created);
                File.SetLastWriteTime(GetSourcePath(), Modified);
            }
        }
    }

    public class TestFiles : List<TestFile>
    {
        public void Add(string folder, string name, bool keep = true)
        {
            base.Add(new TestFile() { Keep = keep, Create = true, SourceFolder = folder, DestFolder = folder, SourceName = name, DestName = name });
        }

        public void Add(string sourceFolder, string sourceName, string destFolder, string destName, DateTime? created = null, DateTime? modified = null, DateTime? exif = null, int? width = 200, int? height = 200)
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
            if (width != null) tf.Width = width.Value;
            if (height != null) tf.Height = height.Value;
            base.Add(tf);
        }

        public void CreateFiles()
        {
            this.Where(f => f.Create).ToList().ForEach(f => f.CreateFile());
        }

        public void DeleteSourceFiles()
        {
            this.ToList().ForEach(f =>
            {
                if (File.Exists(f.GetSourcePath())) File.Delete(f.GetSourcePath());
            });
        }

        public void ValidateSource()
        {
            this.ForEach(f => File.Exists(f.GetSourcePath()).Should().BeTrue(f.Name));
        }

        public void ValidateExistance()
        {
            this.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        public void ValidateContent()
        {
            this.Where(f => string.Compare(Path.GetExtension(f.GetSourcePath()), ".jpg") != 0).ToList().ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
        }

        public void ValidateImageSize(int width, int height)
        {
            this.ForEach(f =>
            {
                var info = Image.Identify(f.GetDestPath());
                info.Should().NotBeNull();
                info.Width.Should().Be(width);
                info.Height.Should().Be(height);
                info.Metadata.ExifProfile.Should().BeNull();
            });
        }

        public void Validate()
        {
            ValidateExistance();
            ValidateContent();
        }
    }
}
