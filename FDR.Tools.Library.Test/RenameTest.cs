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
            config.FileFilter = "*.PR*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "3.pr2", tempFolderPath, "020304_02.pr2", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "2.pr1", tempFolderPath, "010204_01.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "2.ot1", tempFolderPath, "2.ot1");
            files.Add(tempFolderPath, "1.ot2", tempFolderPath, "1.ot2");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithoutAdditionalToChildFolder()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR1|*.PR2";
            config.FilenamePattern = "child1/{cdate:yyMMdd}_{counter:2}";
            var childFolderPath = Path.Combine(tempFolderPath, "child1");

            files.Add(tempFolderPath, "3.pr2", childFolderPath, "020304_02.pr2", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "2.pr1", childFolderPath, "010204_01.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "2.ot2", tempFolderPath, "2.ot2");
            files.Add(tempFolderPath, "1.ot1", tempFolderPath, "1.ot1");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAdditional()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("SE1");
            config.AdditionalFileTypes.Add(" *.SE2 ");

            files.Add(tempFolderPath, "3.pr3", tempFolderPath, "020304_02.pr3", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "2.pr2", tempFolderPath, "010204_01.pr2", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "2.se1", tempFolderPath, "010204_01.se1");
            files.Add(tempFolderPath, "2.se2", tempFolderPath, "010204_01.se2");
            files.Add(tempFolderPath, "1.oth", tempFolderPath, "1.oth");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalToChildFolder()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.FilenamePattern = "child2/{cdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("SE1");
            config.AdditionalFileTypes.Add(" *.SE2 ");
            var childFolderPath = Path.Combine(tempFolderPath, "child2");

            files.Add(tempFolderPath, "3.pr3", childFolderPath, "020304_02.pr3", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "2.pr2", childFolderPath, "010204_01.pr2", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "2.se1", childFolderPath, "010204_01.se1");
            files.Add(tempFolderPath, "2.se2", childFolderPath, "010204_01.se2");
            files.Add(tempFolderPath, "1.oth", tempFolderPath, "1.oth");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalMatchingFileFilter()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("SE1");
            config.AdditionalFileTypes.Add(" *.SE2 ");

            files.Add(tempFolderPath, "2.pr2", tempFolderPath, "000304_01.pr2", new DateTime(2000, 3, 4));
            files.Add(tempFolderPath, "1.pr1", tempFolderPath, "010204_02.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "1.se1", tempFolderPath, "010204_02.se1");
            files.Add(tempFolderPath, "1.se2", tempFolderPath, "010204_02.se2");
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalMatchingFileFilterPlus()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.AdditionalFileTypes.Add("SE1");
            config.AdditionalFileTypes.Add(" *.SE2 ");

            files.Add(tempFolderPath, "3.pr1", tempFolderPath, "010204_02.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "3.se1", tempFolderPath, "010204_02.se1");
            files.Add(tempFolderPath, "3.se2", tempFolderPath, "010204_02.se2");
            files.Add(tempFolderPath, "2.se1", tempFolderPath, "020405_03.se1", new DateTime(2002, 4, 5));
            files.Add(tempFolderPath, "1.pr2", tempFolderPath, "000304_01.pr2", new DateTime(2000, 3, 4));
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAutoCounterLength1()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";

            files.Add(tempFolderPath, "4.pri", tempFolderPath, "000101_1.pri", new DateTime(2000, 1, 1, 0, 0, 1));
            files.Add(tempFolderPath, "3.pri", tempFolderPath, "000101_2.pri", new DateTime(2000, 1, 1, 0, 0, 2));
            files.Add(tempFolderPath, "2.pri", tempFolderPath, "000101_3.pri", new DateTime(2000, 1, 1, 0, 0, 3));
            files.Add(tempFolderPath, "1.pri", tempFolderPath, "000101_4.pri", new DateTime(2000, 1, 1, 0, 0, 4));
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithAutoCounterLength2()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";

            files.Add(tempFolderPath, "12.pri", tempFolderPath, "000101_01.pri", new DateTime(2000, 1, 1, 0, 0, 1));
            files.Add(tempFolderPath, "11.pri", tempFolderPath, "000101_02.pri", new DateTime(2000, 1, 1, 0, 0, 2));
            files.Add(tempFolderPath, "10.pri", tempFolderPath, "000101_03.pri", new DateTime(2000, 1, 1, 0, 0, 3));
            files.Add(tempFolderPath, "09.pri", tempFolderPath, "000101_04.pri", new DateTime(2000, 1, 1, 0, 0, 4));
            files.Add(tempFolderPath, "08.pri", tempFolderPath, "000101_05.pri", new DateTime(2000, 1, 1, 0, 0, 5));
            files.Add(tempFolderPath, "07.pri", tempFolderPath, "000101_06.pri", new DateTime(2000, 1, 1, 0, 0, 6));
            files.Add(tempFolderPath, "06.pri", tempFolderPath, "000101_07.pri", new DateTime(2000, 1, 1, 0, 0, 7));
            files.Add(tempFolderPath, "05.pri", tempFolderPath, "000101_08.pri", new DateTime(2000, 1, 1, 0, 0, 8));
            files.Add(tempFolderPath, "04.pri", tempFolderPath, "000101_09.pri", new DateTime(2000, 1, 1, 0, 0, 9));
            files.Add(tempFolderPath, "03.pri", tempFolderPath, "000101_10.pri", new DateTime(2000, 1, 1, 0, 0, 10));
            files.Add(tempFolderPath, "02.pri", tempFolderPath, "000101_11.pri", new DateTime(2000, 1, 1, 0, 0, 11));
            files.Add(tempFolderPath, "01.pri", tempFolderPath, "000101_12.pri", new DateTime(2000, 1, 1, 0, 0, 12));
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
        }

        [Test]
        public void RenameFilesInFolderWithSameDate()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:auto}";

            var date = new DateTime(2000, 1, 1, 0, 0, 0);
            files.Add(tempFolderPath, "04.pri", tempFolderPath, "000101_04.pri", date, date, date);
            files.Add(tempFolderPath, "11.pri", tempFolderPath, "000101_11.pri", date, date, date);
            files.Add(tempFolderPath, "03.pri", tempFolderPath, "000101_03.pri", date, date, date);
            files.Add(tempFolderPath, "02.pri", tempFolderPath, "000101_02.pri", date, date, date);
            files.Add(tempFolderPath, "01.pri", tempFolderPath, "000101_01.pri", date, date, date);
            files.Add(tempFolderPath, "05.pri", tempFolderPath, "000101_05.pri", date, date, date);
            files.Add(tempFolderPath, "06.pri", tempFolderPath, "000101_06.pri", date, date, date);
            files.Add(tempFolderPath, "07.pri", tempFolderPath, "000101_07.pri", date, date, date);
            files.Add(tempFolderPath, "08.pri", tempFolderPath, "000101_08.pri", date, date, date);
            files.Add(tempFolderPath, "12.pri", tempFolderPath, "000101_12.pri", date, date, date);
            files.Add(tempFolderPath, "09.pri", tempFolderPath, "000101_09.pri", date, date, date);
            files.Add(tempFolderPath, "10.pri", tempFolderPath, "000101_10.pri", date, date, date);
            files.CreateFiles();

            Rename.RenameFilesInFolder(new DirectoryInfo(tempFolderPath), config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
            files.ForEach(f => File.ReadAllText(f.GetDestPath()).Should().Be(f.GetSourcePath(), f.Name));
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