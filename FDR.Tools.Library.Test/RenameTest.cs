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
        public void RenameFilesInFolderToSameName()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FilenamePattern = "samename";

            files.Add(tempFolderPath, "aaa.pri", tempFolderPath, "samename.pri", new DateTime(2000, 1, 1));
            files.Add(tempFolderPath, "bbb.pri", tempFolderPath, "bbb.pri", new DateTime(2000, 1, 2));
            files.CreateFiles();

            System.Action rename = () => Rename.RenameFilesInFolder(tempFolder, config);
            rename.Should().Throw<IOException>();

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderToSameNameStopOnErrorFalse()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FilenamePattern = "samename";
            config.StopOnError = false;

            files.Add(tempFolderPath, "aaa.pri", tempFolderPath, "samename.pri", new DateTime(2000, 1, 1));
            files.Add(tempFolderPath, "bbb.pri", tempFolderPath, "bbb.pri", new DateTime(2000, 1, 2));
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithLowerCaseFileUpperCaseFilter()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "A*.PR*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "aaa.pri", tempFolderPath, "000101_01.pri", new DateTime(2000, 1, 1));
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithUpperCaseFileLowerCaseFilter()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "a*.pr*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "AAA.PRI", tempFolderPath, "000101_01.pri", new DateTime(2000, 1, 1));
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithLowerCaseFilePlusAdditionalUpperCaseFilter()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "1.pri", tempFolderPath, "000101_01.pri", new DateTime(2000, 1, 1));
            files.Add(tempFolderPath, "1.se1", tempFolderPath, "000101_01.se1");
            files.Add(tempFolderPath, "1.se2", tempFolderPath, "000101_01.se2");
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithUpperCaseFilePlusAdditionalLowerCaseFilter()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.pr*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "1.PRI", tempFolderPath, "000101_01.pri", new DateTime(2000, 1, 1));
            files.Add(tempFolderPath, "1.SE1", tempFolderPath, "000101_01.se1");
            files.Add(tempFolderPath, "1.SE2", tempFolderPath, "000101_01.se2");
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithoutAdditional()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "4.pr2", tempFolderPath, "020304_02.pr2", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "3.pr1", tempFolderPath, "010204_01.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "2.ot1", tempFolderPath, "2.ot1");
            files.Add(tempFolderPath, "1.ot2", tempFolderPath, "1.ot2");
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithoutAdditionalToChildFolder()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR1|*.PR2";
            config.FilenamePattern = "child1/{cdate:yyMMdd}_{counter:2}";
            var childFolderPath = Path.Combine(tempFolderPath, "child1");

            files.Add(tempFolderPath, "4.pr2", childFolderPath, "020304_02.pr2", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "3.pr1", childFolderPath, "010204_01.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "2.ot2", tempFolderPath, "2.ot2");
            files.Add(tempFolderPath, "1.ot1", tempFolderPath, "1.ot1");
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithAdditional()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "3.pr3", tempFolderPath, "020304_02.pr3", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "2.pr2", tempFolderPath, "010204_01.pr2", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "2.se1", tempFolderPath, "010204_01.se1");
            files.Add(tempFolderPath, "2.se2", tempFolderPath, "010204_01.se2");
            files.Add(tempFolderPath, "1.oth", tempFolderPath, "1.oth");
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalToChildFolder()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.FilenamePattern = "child2/{cdate:yyMMdd}_{counter:2}";
            var childFolderPath = Path.Combine(tempFolderPath, "child2");

            files.Add(tempFolderPath, "3.pr3", childFolderPath, "020304_02.pr3", new DateTime(2002, 3, 4));
            files.Add(tempFolderPath, "2.pr2", childFolderPath, "010204_01.pr2", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "2.se1", childFolderPath, "010204_01.se1");
            files.Add(tempFolderPath, "2.se2", childFolderPath, "010204_01.se2");
            files.Add(tempFolderPath, "1.oth", tempFolderPath, "1.oth");
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderAdditionalFilesTrue()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.AdditionalFiles = true;
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "1.pr1", tempFolderPath, "010101_01.pr1", new DateTime(2001, 1, 1));
            files.Add(tempFolderPath, "1.se1", tempFolderPath, "010101_01.se1");
            files.Add(tempFolderPath, "2.pr2", tempFolderPath, "020202_02.pr2", new DateTime(2002, 2, 2));
            files.Add(tempFolderPath, "2.se2", tempFolderPath, "020202_02.se2");
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderAdditionalFilesFalse()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.AdditionalFiles = false;
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "1.pr1", tempFolderPath, "010101_01.pr1", new DateTime(2001, 1, 1));
            files.Add(tempFolderPath, "1.se1", tempFolderPath, "1.se1");
            files.Add(tempFolderPath, "2.pr2", tempFolderPath, "020202_02.pr2", new DateTime(2002, 2, 2));
            files.Add(tempFolderPath, "2.se2", tempFolderPath, "2.se2");
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalMatchingFileFilter()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "2.pr2", tempFolderPath, "000304_01.pr2", new DateTime(2000, 3, 4));
            files.Add(tempFolderPath, "1.pr1", tempFolderPath, "010204_02.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "1.se1", tempFolderPath, "010204_02.se1");
            files.Add(tempFolderPath, "1.se2", tempFolderPath, "010204_02.se2");
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalMatchingFileFilterPlus()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "3.pr1", tempFolderPath, "010204_02.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "3.se1", tempFolderPath, "010204_02.se1");
            files.Add(tempFolderPath, "3.se2", tempFolderPath, "010204_02.se2");
            files.Add(tempFolderPath, "2.se1", tempFolderPath, "020405_03.se1", new DateTime(2002, 4, 5));
            files.Add(tempFolderPath, "1.pr2", tempFolderPath, "000304_01.pr2", new DateTime(2000, 3, 4));
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithYoungerAdditionalMatchingFileFilter()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "2.pr2", tempFolderPath, "000304_01.pr2", new DateTime(2000, 3, 4));
            files.Add(tempFolderPath, "1.pr1", tempFolderPath, "010204_02.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "1.se1", tempFolderPath, "010204_02.se1", new DateTime(2001, 2, 6));
            files.Add(tempFolderPath, "1.se2", tempFolderPath, "010204_02.se2", new DateTime(2001, 2, 5));
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithOlderAdditionalMatchingFileFilter()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";

            files.Add(tempFolderPath, "2.pr2", tempFolderPath, "000304_01.pr2", new DateTime(2000, 3, 4));
            files.Add(tempFolderPath, "1.pr1", tempFolderPath, "010202_02.pr1", new DateTime(2001, 2, 4));
            files.Add(tempFolderPath, "1.se1", tempFolderPath, "010202_02.se1", new DateTime(2001, 2, 3));
            files.Add(tempFolderPath, "1.se2", tempFolderPath, "010202_02.se2", new DateTime(2001, 2, 2));
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
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

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
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

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
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

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithPlusInRecursiveFolders()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.AdditionalFiles = false;
            config.Recursive = true;

            files.Add(tempFolderPath, "dir1/1.pr1", tempFolderPath, "dir1/000109_03.pr1", new DateTime(2000, 1, 9));
            files.Add(tempFolderPath, "dir1/1.se1", tempFolderPath, "dir1/1.se1", new DateTime(2000, 1, 7));
            files.Add(tempFolderPath, "dir1/1.se2", tempFolderPath, "dir1/1.se2", new DateTime(2000, 1, 8));
            files.Add(tempFolderPath, "dir2/2.pr2", tempFolderPath, "dir2/000101_01.pr2", new DateTime(2000, 1, 1));
            files.Add(tempFolderPath, "dir2/2.se1", tempFolderPath, "dir2/2.se1", new DateTime(2000, 1, 2));
            files.Add(tempFolderPath, "dir2/2.se2", tempFolderPath, "dir2/2.se2", new DateTime(2000, 1, 3));
            files.Add(tempFolderPath, "dir1/dir3/3.pr3", tempFolderPath, "dir1/dir3/000106_02.pr3", new DateTime(2000, 1, 6));
            files.Add(tempFolderPath, "dir1/dir3/3.se1", tempFolderPath, "dir1/dir3/3.se1", new DateTime(2000, 1, 5));
            files.Add(tempFolderPath, "dir1/dir3/3.se2", tempFolderPath, "dir1/dir3/3.se2", new DateTime(2000, 1, 4));
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalInRecursiveFolders()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.PR*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.Recursive = true;

            files.Add(tempFolderPath, "dir1/1.pr1", tempFolderPath, "dir1/000107_03.pr1", new DateTime(2000, 1, 9));
            files.Add(tempFolderPath, "dir1/1.se1", tempFolderPath, "dir1/000107_03.se1", new DateTime(2000, 1, 7));
            files.Add(tempFolderPath, "dir1/1.se2", tempFolderPath, "dir1/000107_03.se2", new DateTime(2000, 1, 8));
            files.Add(tempFolderPath, "dir2/2.pr2", tempFolderPath, "dir2/000101_01.pr2", new DateTime(2000, 1, 1));
            files.Add(tempFolderPath, "dir2/2.se1", tempFolderPath, "dir2/000101_01.se1", new DateTime(2000, 1, 2));
            files.Add(tempFolderPath, "dir2/2.se2", tempFolderPath, "dir2/000101_01.se2", new DateTime(2000, 1, 3));
            files.Add(tempFolderPath, "dir1/dir3/3.pr3", tempFolderPath, "dir1/dir3/000104_02.pr3", new DateTime(2000, 1, 6));
            files.Add(tempFolderPath, "dir1/dir3/3.se1", tempFolderPath, "dir1/dir3/000104_02.se1", new DateTime(2000, 1, 5));
            files.Add(tempFolderPath, "dir1/dir3/3.se2", tempFolderPath, "dir1/dir3/000104_02.se2", new DateTime(2000, 1, 4));
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithAdditionalMatchingFileFilterInRecursiveFolders()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.Recursive = true;

            files.Add(tempFolderPath, "dir1/1.pr1", tempFolderPath, "dir1/000107_03.pr1", new DateTime(2000, 1, 9));
            files.Add(tempFolderPath, "dir1/1.se1", tempFolderPath, "dir1/000107_03.se1", new DateTime(2000, 1, 7));
            files.Add(tempFolderPath, "dir1/1.se2", tempFolderPath, "dir1/000107_03.se2", new DateTime(2000, 1, 8));
            files.Add(tempFolderPath, "dir2/2.pr2", tempFolderPath, "dir2/000101_01.pr2", new DateTime(2000, 1, 1));
            files.Add(tempFolderPath, "dir2/2.se1", tempFolderPath, "dir2/000101_01.se1", new DateTime(2000, 1, 2));
            files.Add(tempFolderPath, "dir2/2.se2", tempFolderPath, "dir2/000101_01.se2", new DateTime(2000, 1, 3));
            files.Add(tempFolderPath, "dir1/dir3/3.pr3", tempFolderPath, "dir1/dir3/000104_02.pr3", new DateTime(2000, 1, 6));
            files.Add(tempFolderPath, "dir1/dir3/3.se1", tempFolderPath, "dir1/dir3/000104_02.se1", new DateTime(2000, 1, 5));
            files.Add(tempFolderPath, "dir1/dir3/3.se2", tempFolderPath, "dir1/dir3/000104_02.se2", new DateTime(2000, 1, 4));
            files.CreateFiles();

            Rename.RenameFilesInFolder(tempFolder, config);

            files.Validate();
        }

        [Test]
        public void RenameFilesInFolderWithExternalFiles()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.*";
            config.FilenamePattern = "{cdate:yyMMdd}_{counter:2}";
            config.Recursive = true;

            files.Add(tempFolderPath, "dir1/1.pr1", tempFolderPath, "dir1/000107_03.pr1", new DateTime(2000, 1, 9));
            files.Add(tempFolderPath, "dir1/1.se1", tempFolderPath, "dir1/000107_03.se1", new DateTime(2000, 1, 7));
            files.Add(tempFolderPath, "dir1/1.se2", tempFolderPath, "dir1/000107_03.se2", new DateTime(2000, 1, 8));
            files.Add(tempFolderPath, "dir2/2.pr2", tempFolderPath, "dir2/000101_01.pr2", new DateTime(2000, 1, 1));
            files.Add(tempFolderPath, "dir2/2.se1", tempFolderPath, "dir2/000101_01.se1", new DateTime(2000, 1, 2));
            files.Add(tempFolderPath, "dir2/2.se2", tempFolderPath, "dir2/000101_01.se2", new DateTime(2000, 1, 3));
            files.Add(tempFolderPath, "dir1/dir3/3.pr3", tempFolderPath, "dir1/dir3/000104_02.pr3", new DateTime(2000, 1, 6));
            files.Add(tempFolderPath, "dir1/dir3/3.se1", tempFolderPath, "dir1/dir3/000104_02.se1", new DateTime(2000, 1, 5));
            files.Add(tempFolderPath, "dir1/dir3/3.se2", tempFolderPath, "dir1/dir3/000104_02.se2", new DateTime(2000, 1, 4));
            files.CreateFiles();

            var allFiles = Common.GetFiles(tempFolder, "*.*", config.Recursive);

            Rename.RenameFilesInFolder(tempFolder, config, allFiles);

            files.Validate();
        }

        [Test]
        public void RenameFolderWithPattern()
        {
            string pattern = "{pfolder}";

            var folderPath = Path.Combine(tempFolderPath, "folder");
            var newFolderPath = Path.Combine(tempFolderPath, Path.GetFileName(tempFolderPath));
            Directory.CreateDirectory(folderPath);
            Directory.SetCreationTime(folderPath, new DateTime(2001, 2, 3));
            Directory.SetLastWriteTime(folderPath, new DateTime(2004, 5, 6));
            var folder = new DirectoryInfo(folderPath);
            folder.Should().NotBeNull();
            folder.Parent.Should().NotBeNull();
            var parentPath = folder.Parent.FullName;
            parentPath.Should().NotBeNullOrWhiteSpace();
            parentPath.Should().Be(tempFolderPath);

            files.Add(folderPath, "1.aaa", newFolderPath, "1.aaa");
            files.Add(folderPath, "2.bbb", newFolderPath, "2.bbb");
            files.CreateFiles();

            Rename.RenameFolder(folder, pattern);

            files.Validate();
            Directory.Delete(newFolderPath, true);
        }

        [Test]
        public void RenameFolderWithRenameConfig()
        {
            var config = new RenameConfig() { FilenamePattern = "{pfolder}" };
            config.Should().NotBeNull();

            var folderPath = Path.Combine(tempFolderPath, "folder");
            var newFolderPath = Path.Combine(tempFolderPath, Path.GetFileName(tempFolderPath));
            Directory.CreateDirectory(folderPath);
            Directory.SetCreationTime(folderPath, new DateTime(2001, 2, 3));
            Directory.SetLastWriteTime(folderPath, new DateTime(2004, 5, 6));
            var folder = new DirectoryInfo(folderPath);
            folder.Should().NotBeNull();
            folder.Parent.Should().NotBeNull();
            var parentPath = folder.Parent.FullName;
            parentPath.Should().NotBeNullOrWhiteSpace();
            parentPath.Should().Be(tempFolderPath);

            files.Add(folderPath, "1.aaa", newFolderPath, "1.aaa");
            files.Add(folderPath, "2.bbb", newFolderPath, "2.bbb");
            files.CreateFiles();

            Rename.RenameFolder(folder, config);

            files.Validate();
            Directory.Delete(newFolderPath, true);
        }
    }
}