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
    public class ResizeTest : TestBase
    {
        private string tempFolderPath;

        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolderPath);
        }

        [OneTimeTearDown]
        public override void OneTimeTearDown()
        {
            if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath, true);

            base.OneTimeTearDown();
        }

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
            var filePath = Path.Combine(tempFolderPath, Guid.NewGuid().ToString() + ".jpg");
            var newName = Guid.NewGuid().ToString();
            var newPath = Path.Combine(tempFolderPath, newName + ".jpg");

            var config = new ResizeConfig();
            config.Should().NotBeNull();
            config.FilenamePattern = newName;
            config.ResizeMethod = method;
            config.MaxWidth = width;
            config.MaxHeight = height;
            config.ClearMetadata = clearMetadata;
            System.Action validate = () => config.Validate();
            validate.Should().NotThrow();

            Helper.CreateJpgFile(filePath, 200, 100);
            File.Exists(filePath).Should().BeTrue();
            var file = new FileInfo(filePath);
            file.Should().NotBeNull();

            int counter = 1;
            int percent = 0;
            Resize.ResizeFile(file, counter, config, percent);
            File.Exists(newPath).Should().BeTrue();

            var info = Image.Identify(newPath);
            info.Should().NotBeNull();
            info.Width.Should().Be(newWidth, $"Width should be {newWidth}");
            info.Height.Should().Be(newHeight, $"Height should be {newHeight}");
            var exifProfile = info.Metadata.ExifProfile;
            if (clearMetadata)
                exifProfile.Should().BeNull();
            else
            {
                exifProfile.Should().NotBeNull();
                var exif = exifProfile.GetValue<string>(ExifTag.DateTimeOriginal);
                exif.Should().NotBeNull();
            }

            File.Delete(filePath);
            File.Delete(newPath);
        }

        [Test]
        public void ResizeFilesInFolderTests()
        {
            var name1 = Guid.NewGuid().ToString();
            var name2 = Guid.NewGuid().ToString();
            var filePath1 = Path.Combine(tempFolderPath, name1 + ".jpg");
            var filePath2 = Path.Combine(tempFolderPath, name2 + ".jpg");
            var newPath1 = Path.Combine(tempFolderPath, name1 + "_resized.jpg");
            var newPath2 = Path.Combine(tempFolderPath, name2 + "_resized.jpg");

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

            Helper.CreateJpgFile(filePath1, 200, 100);
            File.Exists(filePath1).Should().BeTrue();
            var file1 = new FileInfo(filePath1);
            file1.Should().NotBeNull();

            Helper.CreateJpgFile(filePath2, 300, 400);
            File.Exists(filePath2).Should().BeTrue();
            var file2 = new FileInfo(filePath2);
            file2.Should().NotBeNull();

            Resize.ResizeFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            File.Exists(newPath1).Should().BeTrue();
            var info1 = Image.Identify(newPath1);
            info1.Should().NotBeNull();
            info1.Width.Should().Be(50);
            info1.Height.Should().Be(50);
            info1.Metadata.ExifProfile.Should().BeNull();

            File.Exists(newPath2).Should().BeTrue();
            var info2 = Image.Identify(newPath2);
            info2.Should().NotBeNull();
            info2.Width.Should().Be(50);
            info2.Height.Should().Be(50);
            info2.Metadata.ExifProfile.Should().BeNull();

            File.Delete(filePath1);
            File.Delete(filePath2);
            File.Delete(newPath1);
            File.Delete(newPath2);
        }
    }
}