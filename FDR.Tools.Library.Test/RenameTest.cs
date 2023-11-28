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
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "ccc.cr3", tempFolderPath, "020304_02.cr3", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "bbb.cr2", tempFolderPath, "010204_01.cr2", new DateTime(2001, 2, 4));
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
            config.FilenamePattern = "child1/{cdate:yyMMdd}_{counter:2}";
            var childFolderPath = Path.Combine(tempFolderPath, "child1");

            files.Add(tempFolderPath, "ccc.cr3", childFolderPath, "020304_02.cr3", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "bbb.cr2", childFolderPath, "010204_01.cr2", new DateTime(2001, 2, 4));
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
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");

            files.Add(tempFolderPath, "ccc.cr3", tempFolderPath, "020304_02.cr3", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "bbb.cr2", tempFolderPath, "010204_01.cr2", new DateTime(2001, 2, 4));
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
            config.FilenamePattern = "child2/{cdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");
            var childFolderPath = Path.Combine(tempFolderPath, "child2");

            files.Add(tempFolderPath, "ccc.cr3", childFolderPath, "020304_02.cr3", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "bbb.cr2", childFolderPath, "010204_01.cr2", new DateTime(2001, 2, 4));
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
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");
            config.StopOnError = false;

            files.Add(tempFolderPath, "ccc.cr3", tempFolderPath, "000304_01.cr3", new DateTime(2000, 3, 4));
            files.Add(tempFolderPath, "bbb.cr2", tempFolderPath, "010204_02.cr2", new DateTime(2001, 2, 4));
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
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");
            config.StopOnError = false;

            files.Add(tempFolderPath, "ccc.cr3", tempFolderPath, "000304_01.cr3", new DateTime(2000, 3, 4));
            files.Add(tempFolderPath, "bbb.cr2", tempFolderPath, "010204_02.cr2", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "bbb.jpg", tempFolderPath, "010204_02.jpg");
            files.Add(tempFolderPath, "bbb.png", tempFolderPath, "010204_02.png");
            files.Add(tempFolderPath, "ddd.jpg", tempFolderPath, "020405_03.jpg", new DateTime(2002, 4, 5));
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAutoCounterLength1()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";
            config.StopOnError = false;

            files.Add(tempFolderPath, "a.cr3", tempFolderPath, "000101_1.cr3", new DateTime(2000, 1, 1, 0, 0, 1));
            files.Add(tempFolderPath, "b.cr3", tempFolderPath, "000101_2.cr3", new DateTime(2000, 1, 1, 0, 0, 2));
            files.Add(tempFolderPath, "c.cr3", tempFolderPath, "000101_3.cr3", new DateTime(2000, 1, 1, 0, 0, 3));
            files.Add(tempFolderPath, "d.cr3", tempFolderPath, "000101_4.cr3", new DateTime(2000, 1, 1, 0, 0, 4));
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAutoCounterLength2()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";
            config.StopOnError = false;

            files.Add(tempFolderPath, "a.cr3", tempFolderPath, "000101_01.cr3", new DateTime(2000, 1, 1, 0, 0, 1));
            files.Add(tempFolderPath, "b.cr3", tempFolderPath, "000101_02.cr3", new DateTime(2000, 1, 1, 0, 0, 2));
            files.Add(tempFolderPath, "c.cr3", tempFolderPath, "000101_03.cr3", new DateTime(2000, 1, 1, 0, 0, 3));
            files.Add(tempFolderPath, "d.cr3", tempFolderPath, "000101_04.cr3", new DateTime(2000, 1, 1, 0, 0, 4));
            files.Add(tempFolderPath, "e.cr3", tempFolderPath, "000101_05.cr3", new DateTime(2000, 1, 1, 0, 0, 5));
            files.Add(tempFolderPath, "f.cr3", tempFolderPath, "000101_06.cr3", new DateTime(2000, 1, 1, 0, 0, 6));
            files.Add(tempFolderPath, "g.cr3", tempFolderPath, "000101_07.cr3", new DateTime(2000, 1, 1, 0, 0, 7));
            files.Add(tempFolderPath, "h.cr3", tempFolderPath, "000101_08.cr3", new DateTime(2000, 1, 1, 0, 0, 8));
            files.Add(tempFolderPath, "i.cr3", tempFolderPath, "000101_09.cr3", new DateTime(2000, 1, 1, 0, 0, 9));
            files.Add(tempFolderPath, "j.cr3", tempFolderPath, "000101_10.cr3", new DateTime(2000, 1, 1, 0, 0, 10));
            files.Add(tempFolderPath, "k.cr3", tempFolderPath, "000101_11.cr3", new DateTime(2000, 1, 1, 0, 0, 11));
            files.Add(tempFolderPath, "l.cr3", tempFolderPath, "000101_12.cr3", new DateTime(2000, 1, 1, 0, 0, 12));
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