using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    public class VerifyTest : TestFixtureBase
    {
        private string tempFolderPath;
        private DirectoryInfo folder;
        private string filePath;
        private string md5Path;
        private string errPath;
        private const string fileContent = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
        private const string hash = "Scs2COKzP61rZd+MuPSWaA==";

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolderPath);
            folder = new DirectoryInfo(tempFolderPath);

            filePath = Path.Combine(tempFolderPath, "test.jpg");
            md5Path = Path.Combine(tempFolderPath, ".test.jpg.md5");
            errPath = Path.Combine(tempFolderPath, "test.jpg.error");
        }

        public override void OneTimeTearDown()
        {
            if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath, true);

            base.OneTimeTearDown();
        }

        public override void SetUp()
        {
            base.SetUp();

            File.WriteAllText(filePath, fileContent);
            File.SetLastWriteTime(filePath, new DateTime(2000, 12, 31));

            File.WriteAllText(md5Path, hash);
            File.SetLastWriteTime(md5Path, new DateTime(2000, 12, 31));
        }

        public override void TearDown()
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            if (File.Exists(md5Path)) File.Delete(md5Path);
            if (File.Exists(errPath)) File.Delete(errPath);

            base.TearDown();
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
        public void VerifyTests()
        {
            Verify.VerifyFolder(folder);
            File.Exists(errPath).Should().BeFalse("Error file shouldn't exist");
        }

        [Test]
        public void VerifyInvalidTests()
        {
            File.WriteAllText(md5Path, "dummy");
            Verify.VerifyFolder(folder);
            File.Exists(errPath).Should().BeTrue("Error file should exist");
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
            File.WriteAllText(md5Path, "dummy");
            Verify.HashFolder(folder, true);
            File.Exists(md5Path).Should().BeTrue();
            File.ReadAllText(md5Path).Should().Be(hash);
        }
    }
}