using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class CommonTest : TempFolderTestBase
    {
        [Test]
        public void IsFolderValidTests()
        {
            string tempFolderPath = Path.GetTempPath();
            tempFolderPath.Should().NotBeNullOrWhiteSpace();
            Common.IsFolderValid(tempFolderPath).Should().BeTrue();
            Common.IsFolderValid(Path.Combine(tempFolderPath, Guid.NewGuid().ToString())).Should().BeFalse();
            Common.IsFolderValid("").Should().BeFalse();
        }

        [TestCase(".CR3", false)]
        [TestCase(".CR2", false)]
        [TestCase(".CRW", false)]
        [TestCase(".DNG", false)]
        [TestCase(".JPG", true)]
        [TestCase(".JPEG", true)]
        [TestCase(".TIF", true)]
        [TestCase(".TIFF", true)]
        [TestCase(".EXE", false)]
        [TestCase(".MD5", false)]
        [TestCase(".AVI", false)]
        [TestCase(".MP4", false)]
        [TestCase(".MOV", false)]
        public void IsImageFileTests(string file, bool result)
        {
            Common.IsImageFile(file).Should().Be(result);
        }

        [Test]
        public void GetExifDateOnly()
        {
            const string txtFile = "no_exif.txt";
            const string jpgFile = "exif.jpg";
            var exif = new DateTime(2001, 1, 1, 1, 1, 1);

            // TXT, because a JPG is always created with EXIF date
            var txt = files.Add(tempFolderPath, txtFile, tempFolderPath, txtFile, null, null, null);
            var jpg = files.Add(tempFolderPath, jpgFile, tempFolderPath, jpgFile, null, null, exif);
            files.CreateFiles();

            txt.GetExifDateOnly().Should().BeNull();
            jpg.GetExifDateOnly().Should().Be(exif);
        }

        [Test]
        public void GetExifDateWithDefault()
        {
            const string txtFile = "no_exif.txt";
            const string jpgFile = "exif.jpg";
            var exif = new DateTime(2001, 1, 1, 1, 1, 1);
            var defaultDate = new DateTime(2002, 2, 2, 2, 2, 2);

            // TXT, because a JPG is always created with EXIF date
            var txt = files.Add(tempFolderPath, txtFile, tempFolderPath, txtFile, null, null, null);
            var jpg = files.Add(tempFolderPath, jpgFile, tempFolderPath, jpgFile, null, null, exif);
            files.CreateFiles();

            txt.GetExifDate(defaultDate).Should().Be(defaultDate);
            jpg.GetExifDate(defaultDate).Should().Be(exif);
        }

        [Test]
        public void GetExifDate()
        {
            const string txtFileCreated = "created.txt";
            const string txtFileModified = "modified.txt";
            const string jpgFile = "exif.jpg";
            var first = new DateTime(2001, 1, 1, 1, 1, 1);
            var second = new DateTime(2002, 2, 2, 2, 2, 2);
            var exif = new DateTime(2003, 3, 3, 3, 3, 3);

            // TXT, because a JPG is always created with EXIF date
            var txtCreated = files.Add(tempFolderPath, txtFileCreated, tempFolderPath, txtFileCreated, first, second, null);
            var txtModified = files.Add(tempFolderPath, txtFileModified, tempFolderPath, txtFileModified, second, first, null);
            var jpg = files.Add(tempFolderPath, jpgFile, tempFolderPath, jpgFile, null, null, exif);
            files.CreateFiles();

            txtCreated.GetExifDate().Should().Be(first);
            txtModified.GetExifDate().Should().Be(first);
            jpg.GetExifDate().Should().Be(exif);
        }

        [Test]
        public void FileDateComparerNonExif()
        {
            const string file1 = "c.txt";
            const string file2 = "b.txt";
            const string file3 = "a.txt";
            var first = new DateTime(2001, 1, 1, 1, 1, 1);
            var second = new DateTime(2002, 2, 2, 2, 2, 2);
            var third = new DateTime(2003, 3, 3, 3, 3, 3);
            var fileInfos = new List<FileInfo>();

            fileInfos.Add(files.Add(tempFolderPath, file3, tempFolderPath, file3, third, third, null));
            fileInfos.Add(files.Add(tempFolderPath, file2, tempFolderPath, file2, second, second, null));
            fileInfos.Add(files.Add(tempFolderPath, file1, tempFolderPath, file1, first, first, null));
            files.CreateFiles();

            fileInfos[0].Name.Should().Be(file3);
            fileInfos[1].Name.Should().Be(file2);
            fileInfos[2].Name.Should().Be(file1);

            fileInfos.Sort(Common.FileComparer);

            fileInfos[0].Name.Should().Be(file1);
            fileInfos[1].Name.Should().Be(file2);
            fileInfos[2].Name.Should().Be(file3);
        }

        [Test]
        public void FileDateComparerExif()
        {
            const string file1 = "c.jpg";
            const string file2 = "b.jpg";
            const string file3 = "a.jpg";
            var first = new DateTime(2001, 1, 1, 1, 1, 1);
            var second = new DateTime(2002, 2, 2, 2, 2, 2);
            var third = new DateTime(2003, 3, 3, 3, 3, 3);
            var fileInfos = new List<FileInfo>();

            fileInfos.Add(files.Add(tempFolderPath, file3, tempFolderPath, file3, null, null, third));
            fileInfos.Add(files.Add(tempFolderPath, file2, tempFolderPath, file2, null, null, second));
            fileInfos.Add(files.Add(tempFolderPath, file1, tempFolderPath, file1, null, null, first));
            files.CreateFiles();

            fileInfos[0].Name.Should().Be(file3);
            fileInfos[1].Name.Should().Be(file2);
            fileInfos[2].Name.Should().Be(file1);

            fileInfos.Sort(Common.FileComparer);

            fileInfos[0].Name.Should().Be(file1);
            fileInfos[1].Name.Should().Be(file2);
            fileInfos[2].Name.Should().Be(file3);
        }

        [TestCase("b.txt", "20010101", "20010101", "20010101", "a.txt", "20020202", "20020202", "20020202")]
        [TestCase("b.txt", "20010101", "20010101", "20000303", "a.txt", "20020202", "20020202", "20000101")]
        [TestCase("b.txt", "20010101", "20040404", "20000303", "a.txt", "20020202", "20030303", "20000101")]
        [TestCase("b.txt", "20040404", "20010101", "20000303", "a.txt", "20030303", "20020202", "20000101")]

        [TestCase("b.txt", "20010101", "20010101", null, "a.txt", "20020202", "20020202", null)]
        [TestCase("b.txt", "20010101", "20040404", null, "a.txt", "20020202", "20030303", null)]
        [TestCase("b.txt", "20040404", "20010101", null, "a.txt", "20030303", "20020202", null)]

        [TestCase("b.txt", "20010101", null, null, "a.txt", "20020202", null, null)]
        [TestCase("b.txt", null, "20010101", null, "a.txt", null, "20020202", null)]
        [TestCase("b.txt", "20010101", null, null, "a.txt", null, "20020202", null)]
        [TestCase("b.txt", null, "20010101", null, "a.txt", "20020202", null, null)]

        [TestCase("b.jpg", "20010101", "20010101", "20010101", "a.jpg", "20020202", "20020202", "20020202")]
        [TestCase("b.jpg", "20040404", "20040404", "20010101", "a.jpg", "20000101", "20000101", "20020202")]
        [TestCase("b.jpg", null, "20040404", "20010101", "a.jpg", null, "20000101", "20020202")]
        [TestCase("b.jpg", "20040404", null, "20010101", "a.jpg", "20000101", null, "20020202")]
        [TestCase("b.jpg", null, "20040404", "20010101", "a.jpg", "20000101", null, "20020202")]
        [TestCase("b.jpg", "20040404", null, "20010101", "a.jpg", null, "20000101", "20020202")]
        [TestCase("b.jpg", null, null, "20010101", "a.jpg", null, null, "20020202")]

        [TestCase("b.jpg", "20010101", "20010101", "20010101", "a.txt", "20020202", "20020202", "20020202")]
        [TestCase("b.jpg", "20040404", "20040404", "20010101", "a.txt", "20020202", "20020202", "20020202")]
        [TestCase("b.jpg", "20040404", "20040404", "20010101", "a.txt", "20020202", "20020202", null)]
        [TestCase("b.jpg", "20040404", "20040404", "20010101", "a.txt", "20020202", null, null)]
        [TestCase("b.jpg", "20040404", null, "20010101", "a.txt", "20020202", null, null)]
        [TestCase("b.jpg", null, "20040404", "20010101", "a.txt", "20020202", null, null)]
        [TestCase("b.jpg", null, null, "20010101", "a.txt", "20020202", null, null)]
        [TestCase("b.jpg", "20040404", "20040404", "20010101", "a.txt", null, "20020202", null)]
        [TestCase("b.jpg", "20040404", null, "20010101", "a.txt", null, "20020202", null)]
        [TestCase("b.jpg", null, "20040404", "20010101", "a.txt", null, "20020202", null)]
        [TestCase("b.jpg", null, null, "20010101", "a.txt", null, "20020202", null)]

        [TestCase("b.txt", "20010101", "20010101", "20010101", "a.jpg", "20020202", "20020202", "20020202")]
        [TestCase("b.txt", "20010101", "20010101", "20010101", "a.jpg", "20000101", "20000101", "20020202")]
        [TestCase("b.txt", "20010101", "20010101", null, "a.jpg", "20000101", "20000101", "20020202")]
        [TestCase("b.txt", "20010101", "20010101", null, "a.jpg", "20000101", null, "20020202")]
        [TestCase("b.txt", "20010101", "20010101", null, "a.jpg", null, "20000101", "20020202")]
        [TestCase("b.txt", "20010101", "20010101", null, "a.jpg", null, null, "20020202")]
        [TestCase("b.txt", "20010101", null, null, "a.jpg", "20000101", "20000101", "20020202")]
        [TestCase("b.txt", "20010101", null, null, "a.jpg", "20000101", null, "20020202")]
        [TestCase("b.txt", "20010101", null, null, "a.jpg", null, "20000101", "20020202")]
        [TestCase("b.txt", "20010101", null, null, "a.jpg", null, null, "20020202")]
        [TestCase("b.txt", null, "20010101", null, "a.jpg", "20000101", "20000101", "20020202")]
        [TestCase("b.txt", null, "20010101", null, "a.jpg", "20000101", null, "20020202")]
        [TestCase("b.txt", null, "20010101", null, "a.jpg", null, "20000101", "20020202")]
        [TestCase("b.txt", null, "20010101", null, "a.jpg", null, null, "20020202")]
        public void FileDateComparerGeneric(string file1, string created1, string modified1, string exif1, string file2, string created2, string modified2, string exif2)
        {
            const string format = "yyyyMMdd";
            DateTime? createdDate1 = created1 != null ? DateTime.ParseExact(created1, format, null) : null;
            DateTime? modifiedDate1 = modified1 != null ? DateTime.ParseExact(modified1, format, null) : null;
            DateTime? exifDate1 = exif1 != null ? DateTime.ParseExact(exif1, format, null) : null;
            DateTime? createdDate2 = created2 != null ? DateTime.ParseExact(created2, format, null) : null;
            DateTime? modifiedDate2 = modified2 != null ? DateTime.ParseExact(modified2, format, null) : null;
            DateTime? exifDate2 = exif2 != null ? DateTime.ParseExact(exif2, format, null) : null;

            var fileInfos = new List<FileInfo>();

            fileInfos.Add(files.Add(tempFolderPath, file2, tempFolderPath, file2, createdDate2, modifiedDate2, exifDate2));
            fileInfos.Add(files.Add(tempFolderPath, file1, tempFolderPath, file1, createdDate1, modifiedDate1, exifDate1));
            files.CreateFiles();

            fileInfos[0].Name.Should().Be(file2);
            fileInfos[1].Name.Should().Be(file1);

            fileInfos.Sort(Common.FileComparer);

            fileInfos[0].Name.Should().Be(file1);
            fileInfos[1].Name.Should().Be(file2);
        }
    }
}