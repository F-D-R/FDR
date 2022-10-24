using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class MoveTest : TempFolderTestBase
    {
        [Test]
        public void RenameFilesInFolderWithoutAdditionalTests()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.FilenamePattern = "{mdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Clear();

            var files = new TestFiles();
            files.Add(new DateTime(2002, 3, 4), tempFolderPath, "ccc.cr3", tempFolderPath, "020304_02.cr3");
            files.Add(new DateTime(2001, 2, 4), tempFolderPath, "bbb.cr2", tempFolderPath, "010204_01.cr2");
            files.Add(tempFolderPath, "bbb.jpg", tempFolderPath, "bbb.jpg");
            files.Add(tempFolderPath, "aaa.txt", tempFolderPath, "aaa.txt");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalTests()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.FilenamePattern = "{mdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Clear();
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");

            var files = new TestFiles();
            files.Add(new DateTime(2002, 3, 4), tempFolderPath, "ccc.cr3", tempFolderPath, "020304_02.cr3");
            files.Add(new DateTime(2001, 2, 4), tempFolderPath, "bbb.cr2", tempFolderPath, "010204_01.cr2");
            files.Add(tempFolderPath, "bbb.jpg", tempFolderPath, "010204_01.jpg");
            files.Add(tempFolderPath, "bbb.png", tempFolderPath, "010204_01.png");
            files.Add(tempFolderPath, "aaa.txt", tempFolderPath, "aaa.txt");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalMatchingFileFilterTests()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{mdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Clear();
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");
            config.StopOnError = false;

            var files = new TestFiles();
            files.Add(new DateTime(2000, 3, 4), tempFolderPath, "ccc.cr3", tempFolderPath, "000304_01.cr3");
            files.Add(new DateTime(2001, 2, 4), tempFolderPath, "bbb.cr2", tempFolderPath, "010204_02.cr2");
            files.Add(tempFolderPath, "bbb.jpg", tempFolderPath, "010204_02.jpg");
            files.Add(tempFolderPath, "bbb.png", tempFolderPath, "010204_02.png");
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