using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class ResizeTest : TempFolderTestBase
    {
        [TestCase(ResizeMethod.fit_in, 200, 200, 200, 100, false)]
        [TestCase(ResizeMethod.fit_in, 200, 200, 200, 100, true)]
        [TestCase(ResizeMethod.fit_in, 400, 100, 200, 100, false)]
        [TestCase(ResizeMethod.fit_in, 400, 100, 200, 100, true)]
        [TestCase(ResizeMethod.fit_in, 200, 100, 200, 100, false)]
        [TestCase(ResizeMethod.fit_in, 200, 100, 200, 100, true)]
        [TestCase(ResizeMethod.fit_in, 400, 400, 400, 200, false)]
        [TestCase(ResizeMethod.fit_in, 400, 400, 400, 200, true)]
        [TestCase(ResizeMethod.fit_in, 100, 100, 100, 50, false)]
        [TestCase(ResizeMethod.fit_in, 100, 100, 100, 50, true)]
        [TestCase(ResizeMethod.fit_in, 400, 50, 100, 50, false)]
        [TestCase(ResizeMethod.fit_in, 400, 50, 100, 50, true)]
        [TestCase(ResizeMethod.max_width, 200, 1, 200, 100, false)]
        [TestCase(ResizeMethod.max_width, 200, 1, 200, 100, true)]
        [TestCase(ResizeMethod.max_width, 400, 400, 400, 200, false)]
        [TestCase(ResizeMethod.max_width, 400, 400, 400, 200, true)]
        [TestCase(ResizeMethod.max_width, 100, 100, 100, 50, false)]
        [TestCase(ResizeMethod.max_width, 100, 100, 100, 50, true)]
        [TestCase(ResizeMethod.max_height, 1, 100, 200, 100, false)]
        [TestCase(ResizeMethod.max_height, 1, 100, 200, 100, true)]
        [TestCase(ResizeMethod.max_height, 400, 400, 800, 400, false)]
        [TestCase(ResizeMethod.max_height, 400, 400, 800, 400, true)]
        [TestCase(ResizeMethod.max_height, 50, 50, 100, 50, false)]
        [TestCase(ResizeMethod.max_height, 50, 50, 100, 50, true)]
        [TestCase(ResizeMethod.stretch, 400, 400, 400, 400, false)]
        [TestCase(ResizeMethod.stretch, 400, 400, 400, 400, true)]
        [TestCase(ResizeMethod.stretch, 50, 50, 50, 50, false)]
        [TestCase(ResizeMethod.stretch, 50, 50, 50, 50, true)]
        public void ResizeFileTests(ResizeMethod method, int width, int height, int newWidth, int newHeight, bool clearMetadata)
        {
            var newName = Guid.NewGuid().ToString();

            files.Add(tempFolderPath, Guid.NewGuid().ToString() + ".jpg", tempFolderPath, newName + ".jpg", null, null, null, 200, 100);

            files.CreateFiles();
            files.ForEach(f => File.Exists(f.GetSourcePath()).Should().BeTrue(f.Name));

            var config = new ResizeConfig();
            config.Should().NotBeNull();
            config.FilenamePattern = newName;
            config.ResizeMethod = method;
            config.MaxWidth = width;
            config.MaxHeight = height;
            config.ClearMetadata = clearMetadata;
            System.Action validate = () => config.Validate();
            validate.Should().NotThrow();

            var file = new FileInfo(files[0].GetSourcePath());
            file.Should().NotBeNull();

            int counter = 1;
            int percent = 0;
            Resize.ResizeFile(file, counter, config, percent);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));

            var info = Image.Identify(files[0].GetDestPath());
            info.Should().NotBeNull();
            info.Width.Should().Be(newWidth, $"Width should be {newWidth}");
            info.Height.Should().Be(newHeight, $"Height should be {newHeight}");
            var exifProfile = info.Metadata.ExifProfile;
            if (clearMetadata)
                exifProfile.Should().BeNull();
            else
            {
                exifProfile.Should().NotBeNull();
                IExifValue<string> exif;
                exifProfile.TryGetValue<string>(ExifTag.DateTimeOriginal, out exif).Should().BeTrue();
                exif.Should().NotBeNull();
            }
        }

        [Test]
        public void ResizeFilesInFolderTests()
        {
            var name1 = Guid.NewGuid().ToString();
            var name2 = Guid.NewGuid().ToString();

            files.Add(tempFolderPath, name1 + ".jpg", tempFolderPath, name1 + "_resized.jpg", null, null, null, 200, 100);
            files.Add(tempFolderPath, name2 + ".jpg", tempFolderPath, name2 + "_resized.jpg", null, null, null, 300, 400);

            files.CreateFiles();
            files.ForEach(f => File.Exists(f.GetSourcePath()).Should().BeTrue(f.Name));

            var config = new ResizeConfig();
            config.Should().NotBeNull();
            System.Action validate = () => config.Validate();
            config.FileFilter = "*.jpg";
            config.FilenamePattern = "{name}_resized";
            config.ResizeMethod = ResizeMethod.stretch;
            config.MaxWidth = 50;
            config.MaxHeight = 50;
            config.ClearMetadata = true;
            validate.Should().NotThrow();

            Resize.ResizeFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f =>
            {
                var info = Image.Identify(f.GetDestPath());
                info.Should().NotBeNull();
                info.Width.Should().Be(50);
                info.Height.Should().Be(50);
                info.Metadata.ExifProfile.Should().BeNull();
            });
        }

        [Test]
        public void ResizeFilesInFolderTests2()
        {
            var name1 = Guid.NewGuid().ToString();
            var name2 = Guid.NewGuid().ToString();

            files.Add(tempFolderPath, name1 + ".jpg", tempFolderPath + "/resized2", name1 + ".jpg", null, null, null, 200, 100);
            files.Add(tempFolderPath, name2 + ".jpg", tempFolderPath + "/resized2", name2 + ".jpg", null, null, null, 300, 400);

            files.CreateFiles();
            files.ForEach(f => File.Exists(f.GetSourcePath()).Should().BeTrue(f.Name));

            var config = new ResizeConfig();
            config.Should().NotBeNull();
            System.Action validate = () => config.Validate();
            config.FileFilter = "*.jpg";
            config.FilenamePattern = "{name}";
            config.RelativeFolder = "resized2";
            config.ResizeMethod = ResizeMethod.stretch;
            config.MaxWidth = 50;
            config.MaxHeight = 50;
            config.ClearMetadata = true;
            validate.Should().NotThrow();

            Resize.ResizeFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f =>
            {
                var info = Image.Identify(f.GetDestPath());
                info.Should().NotBeNull();
                info.Width.Should().Be(50);
                info.Height.Should().Be(50);
                info.Metadata.ExifProfile.Should().BeNull();
            });
        }

        [Test]
        public void ResizeFilesInFolderTests3()
        {
            var name1 = Guid.NewGuid().ToString();
            var name2 = Guid.NewGuid().ToString();

            files.Add(tempFolderPath, name1 + ".jpg", tempFolderPath + "/resized3", name1 + "_small.jpg", null, null, null, 200, 100);
            files.Add(tempFolderPath, name2 + ".jpg", tempFolderPath + "/resized3", name2 + "_small.jpg", null, null, null, 300, 400);

            files.CreateFiles();
            files.ForEach(f => File.Exists(f.GetSourcePath()).Should().BeTrue(f.Name));

            var config = new ResizeConfig();
            config.Should().NotBeNull();
            System.Action validate = () => config.Validate();
            config.FileFilter = "*.jpg";
            config.FilenamePattern = "resized3/{name}_small";
            config.ResizeMethod = ResizeMethod.stretch;
            config.MaxWidth = 50;
            config.MaxHeight = 50;
            config.ClearMetadata = true;
            validate.Should().NotThrow();

            Resize.ResizeFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f =>
            {
                var info = Image.Identify(f.GetDestPath());
                info.Should().NotBeNull();
                info.Width.Should().Be(50);
                info.Height.Should().Be(50);
                info.Metadata.ExifProfile.Should().BeNull();
            });
        }
    }
}