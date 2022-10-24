using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class RenameTest : TempFolderTestBase
    {
        [Test]
        public void RenameFilesInFolderWithoutAdditional()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.FilenamePattern = "{mdate:yyMMdd}_{counter:2}";

            files.Add(new DateTime(2002, 3, 4), tempFolderPath, "ccc.cr3", tempFolderPath, "020304_02.cr3");
            files.Add(new DateTime(2001, 2, 4), tempFolderPath, "bbb.cr2", tempFolderPath, "010204_01.cr2");
            files.Add(tempFolderPath, "bbb.jpg", tempFolderPath, "bbb.jpg");
            files.Add(tempFolderPath, "aaa.txt", tempFolderPath, "aaa.txt");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithoutAdditionalToChildFolder()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.FilenamePattern = "child1/{mdate:yyMMdd}_{counter:2}";
            var childFolderPath = Path.Combine(tempFolderPath, "child1");

            files.Add(new DateTime(2002, 3, 4), tempFolderPath, "ccc.cr3", childFolderPath, "020304_02.cr3");
            files.Add(new DateTime(2001, 2, 4), tempFolderPath, "bbb.cr2", childFolderPath, "010204_01.cr2");
            files.Add(tempFolderPath, "bbb.jpg", tempFolderPath, "bbb.jpg");
            files.Add(tempFolderPath, "aaa.txt", tempFolderPath, "aaa.txt");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAdditional()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.FilenamePattern = "{mdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");

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
        public void RenameFilesInFolderWithAdditionalToChildFolder()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.FilenamePattern = "child2/{mdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");
            var childFolderPath = Path.Combine(tempFolderPath, "child2");

            files.Add(new DateTime(2002, 3, 4), tempFolderPath, "ccc.cr3", childFolderPath, "020304_02.cr3");
            files.Add(new DateTime(2001, 2, 4), tempFolderPath, "bbb.cr2", childFolderPath, "010204_01.cr2");
            files.Add(tempFolderPath, "bbb.jpg", childFolderPath, "010204_01.jpg");
            files.Add(tempFolderPath, "bbb.png", childFolderPath, "010204_01.png");
            files.Add(tempFolderPath, "aaa.txt", tempFolderPath, "aaa.txt");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalMatchingFileFilter()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{mdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");
            config.StopOnError = false;

            files.Add(new DateTime(2000, 3, 4), tempFolderPath, "ccc.cr3", tempFolderPath, "000304_01.cr3");
            files.Add(new DateTime(2001, 2, 4), tempFolderPath, "bbb.cr2", tempFolderPath, "010204_02.cr2");
            files.Add(tempFolderPath, "bbb.jpg", tempFolderPath, "010204_02.jpg");
            files.Add(tempFolderPath, "bbb.png", tempFolderPath, "010204_02.png");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalMatchingFileFilterPlusJpg()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{mdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");
            config.StopOnError = false;

            files.Add(new DateTime(2000, 3, 4), tempFolderPath, "ccc.cr3", tempFolderPath, "000304_01.cr3");
            files.Add(new DateTime(2001, 2, 4), tempFolderPath, "bbb.cr2", tempFolderPath, "010204_02.cr2");
            files.Add(tempFolderPath, "bbb.jpg", tempFolderPath, "010204_02.jpg");
            files.Add(tempFolderPath, "bbb.png", tempFolderPath, "010204_02.png");
            files.Add(new DateTime(2002, 4, 5), tempFolderPath, "ddd.jpg", tempFolderPath, "020405_03.jpg");
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