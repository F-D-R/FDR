using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class RenameTest
    {
        private string tempFolderPath;
        private readonly TestFiles files = new();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolderPath);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath, true);
        }

        [Test]
        public void RenameFilesInFolderTests()
        {
            var config = new BatchRenameConfig() { FilenamePattern = "{mdate:yyMMdd}_{counter:2}" };
            config.Should().NotBeNull();

            files.Add(new DateTime(2002, 3, 4), tempFolderPath, "ccc.cr3", tempFolderPath, "020304_02.cr3");
            files.Add(new DateTime(2001, 2, 4), tempFolderPath, "bbb.cr2", tempFolderPath, "010204_01.cr2");
            files.Add(tempFolderPath, "bbb.jpg", tempFolderPath, "010204_01.jpg");
            files.Add(tempFolderPath, "aaa.txt", tempFolderPath, "aaa.txt");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFolderTests()
        {
            var config = new RenameConfig() { FilenamePattern = "{pfolder}" };
            config.Should().NotBeNull();

            var folderPath = Path.Combine(tempFolderPath, "folder");
            Directory.CreateDirectory(folderPath);
            Directory.SetCreationTime(folderPath, new DateTime(2001, 2, 3));
            Directory.SetLastWriteTime(folderPath, new DateTime(2004, 5, 6));
            var folder = new DirectoryInfo(folderPath);
            folder.Should().NotBeNull();
            folder.Parent.Should().NotBeNull();
            var parentPath = folder.Parent.FullName;
            parentPath.Should().NotBeNullOrWhiteSpace();
            parentPath.Should().Be(tempFolderPath);

            Rename.RenameFolder(folder, config);

            Directory.Exists(folderPath).Should().BeFalse();
            Directory.Exists(Path.Combine(parentPath, Path.GetDirectoryName(parentPath))).Should().BeTrue();
        }
    }
}