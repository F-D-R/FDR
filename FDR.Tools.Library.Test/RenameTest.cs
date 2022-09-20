using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class RenameTest
    {
        private string tempFolderPath;
        private string folderPath;
        private string file1Path;
        private string file2Path;
        private string file2PlusPath;
        private string otherFilePath;
        private DirectoryInfo folder;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolderPath);

            folderPath = Path.Combine(tempFolderPath, "temp", "testfolder");
            Directory.CreateDirectory(folderPath);

            file1Path = Path.Combine(folderPath, "ccc.cr3");
            File.WriteAllText(file1Path, "");
            File.SetCreationTime(file1Path, new DateTime(2001, 2, 3));
            File.SetLastWriteTime(file1Path, new DateTime(2002, 3, 4));

            file2Path = Path.Combine(folderPath, "bbb.cr2");
            File.WriteAllText(file2Path, "");
            File.SetCreationTime(file2Path, new DateTime(2001, 2, 4));
            File.SetLastWriteTime(file2Path, new DateTime(2001, 2, 4));

            file2PlusPath = Path.Combine(folderPath, "bbb.jpg");
            File.WriteAllText(file2PlusPath, "");

            otherFilePath = Path.Combine(folderPath, "aaa.txt");
            File.WriteAllText(otherFilePath, "");

            Directory.SetCreationTime(folderPath, new DateTime(2001, 2, 3));
            Directory.SetLastWriteTime(folderPath, new DateTime(2004, 5, 6));
            folder = new DirectoryInfo(folderPath);
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
            folder.Should().NotBeNull();
            Rename.RenameFilesInFolder(folder, config);

            File.Exists(file1Path).Should().BeFalse();
            File.Exists(file2Path).Should().BeFalse();
            File.Exists(file2PlusPath).Should().BeFalse();
            File.Exists(otherFilePath).Should().BeTrue();
            File.Exists(Path.Combine(folderPath, "020304_01.cr3")).Should().BeTrue();
            File.Exists(Path.Combine(folderPath, "010204_02.cr2")).Should().BeTrue();
            File.Exists(Path.Combine(folderPath, "010204_02.jpg")).Should().BeTrue();
        }

        [Test]
        public void RenameFolderTests()
        {
            var config = new RenameConfig() { FilenamePattern = "{pfolder}" };
            config.Should().NotBeNull();
            folder.Should().NotBeNull();
            folder.Parent.Should().NotBeNull();
            var parentPath = folder.Parent.FullName;
            parentPath.Should().NotBeNullOrWhiteSpace();
            Rename.RenameFolder(folder, config);

            Directory.Exists(folderPath).Should().BeFalse();
            Directory.Exists(Path.Combine(parentPath, Path.GetDirectoryName(parentPath))).Should().BeTrue();
        }
    }
}