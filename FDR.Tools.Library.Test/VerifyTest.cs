using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class VerifyTest
    {
        private string tempFolderPath;
        private DirectoryInfo folder;
        private string filePath;
        private string md5Path;
        private string errPath;
        private string missingPath;
        private const string fileContent = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
        private const string hash = "Scs2COKzP61rZd+MuPSWaA==";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolderPath);
            folder = new DirectoryInfo(tempFolderPath);

            filePath = Path.Combine(tempFolderPath, "test.jpg");
            md5Path = Path.Combine(tempFolderPath, ".test.jpg.md5");
            errPath = Path.Combine(tempFolderPath, "test.jpg.error");
            missingPath = Path.Combine(tempFolderPath, "missing.jpg");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath, true);
        }

        [SetUp]
        public void SetUp()
        {
            File.WriteAllText(filePath, fileContent);
            File.SetLastWriteTime(filePath, new DateTime(2000, 12, 31));

            File.WriteAllText(md5Path, hash);
            File.SetLastWriteTime(md5Path, new DateTime(2000, 12, 31));
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            if (File.Exists(md5Path)) File.Delete(md5Path);
            if (File.Exists(errPath)) File.Delete(errPath);
        }

        [Test]
        public void FileNamingTests()
        {
            Verify.GetMd5FileName(new FileInfo(filePath)).Should().Be(md5Path, "Invalid MD5 filename was calculated");
            Verify.GetFileNameFromMD5(new FileInfo(md5Path)).Should().Be(filePath, "Invalid filename was calculated from MD5 filename");
            Verify.GetErrorFileName(new FileInfo(filePath)).Should().Be(errPath, "Invalid error filename was calculated");
            Verify.GetFileNameFromError(new FileInfo(errPath)).Should().Be(filePath, "Invalid filename was calculated from error filename");
        }

        [Test]
        public void ComputeHashTests()
        {
            Verify.ComputeHash(new FileInfo(filePath)).Should().Be(hash);
            Verify.ComputeHash(new FileInfo(md5Path)).Should().NotBe(hash);
            Task<string>.Run(() => Verify.ComputeHashAsync(new FileInfo(filePath))).Result.Should().Be(hash);
            Task<string>.Run(() => Verify.ComputeHashAsync(new FileInfo(md5Path))).Result.Should().NotBe(hash);
        }

        [Test]
        public void VerifyTests()
        {
            Verify.VerifyFolder(folder);
            File.Exists(errPath).Should().BeFalse();

            File.WriteAllText(md5Path, "dummy");
            Verify.VerifyFolder(folder);
            File.Exists(errPath).Should().BeTrue();
        }

        [Test]
        public void CreateHashFileTests()
        {
            File.Delete(md5Path);
            File.Exists(md5Path).Should().BeFalse();
            Verify.CreateHashFile(md5Path, hash, DateTime.UtcNow);
            File.Exists(md5Path).Should().BeTrue();
            File.ReadAllText(md5Path).Should().Be(hash);

            File.Delete(md5Path);
            File.Exists(md5Path).Should().BeFalse();
            Task.Run(() => Verify.CreateHashFileAsync(md5Path, hash, DateTime.UtcNow)).Wait();
            File.Exists(md5Path).Should().BeTrue();
            File.ReadAllText(md5Path).Should().Be(hash);
        }

        [Test]
        public void IsValidImageTests()
        {
            Verify.IsValidImage((string)null).Should().BeFalse();
            Verify.IsValidImage((FileInfo)null).Should().BeFalse();
            Task<bool>.Run(() => Verify.IsValidImageAsync((string)null)).Result.Should().BeFalse();
            Task<bool>.Run(() => Verify.IsValidImageAsync((FileInfo)null)).Result.Should().BeFalse();

            Verify.IsValidImage("dummy").Should().BeFalse();
            Task<bool>.Run(() => Verify.IsValidImageAsync("dummy")).Result.Should().BeFalse();

            Verify.IsValidImage(missingPath).Should().BeFalse();
            Verify.IsValidImage(new FileInfo(missingPath)).Should().BeFalse();
            Task<bool>.Run(() => Verify.IsValidImageAsync(missingPath)).Result.Should().BeFalse();
            Task<bool>.Run(() => Verify.IsValidImageAsync(new FileInfo(missingPath))).Result.Should().BeFalse();

            Verify.IsValidImage(filePath).Should().BeFalse();
            Verify.IsValidImage(new FileInfo(filePath)).Should().BeFalse();
            Task<bool>.Run(() => Verify.IsValidImageAsync(filePath)).Result.Should().BeFalse();
            Task<bool>.Run(() => Verify.IsValidImageAsync(new FileInfo(filePath))).Result.Should().BeFalse();

            var jpgPath = Path.Combine(tempFolderPath, Guid.NewGuid().ToString() + ".jpg");
            using (var image = new Image<Argb32>(100, 100))
            {
                image.Should().NotBeNull();
                image.SaveAsJpeg(jpgPath);
            }
            File.Exists(jpgPath).Should().BeTrue();

            Verify.IsValidImage(jpgPath).Should().BeTrue();
            Verify.IsValidImage(new FileInfo(jpgPath)).Should().BeTrue();
            Task<bool>.Run(() => Verify.IsValidImageAsync(jpgPath)).Result.Should().BeTrue();
            Task<bool>.Run(() => Verify.IsValidImageAsync(new FileInfo(jpgPath))).Result.Should().BeTrue();
        }

        [Test]
        public void HashTests()
        {
            File.Delete(md5Path);
            File.Exists(md5Path).Should().BeFalse();
            Verify.HashFolder(folder);
            File.Exists(md5Path).Should().BeTrue();
            File.ReadAllText(md5Path).Should().Be(hash);
        }

        [Test]
        public void RehashTests()
        {
            var begin = DateTime.UtcNow.AddMilliseconds(-300);
            File.WriteAllText(md5Path, "dummy");
            Verify.HashFolder(folder, true);
            File.Exists(md5Path).Should().BeTrue();
            File.ReadAllText(md5Path).Should().Be(hash);
            File.GetLastWriteTimeUtc(md5Path).Should().Be(File.GetLastWriteTimeUtc(filePath));
            File.GetCreationTimeUtc(md5Path).Should().BeAfter(begin);
        }
    }
}