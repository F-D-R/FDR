using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using System.Linq;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class CommonTest : TempFolderTestBase
    {
        [Test]
        public void IsFolderValidTests()
        {
            string folder = Path.GetTempPath();
            folder.Should().NotBeNullOrWhiteSpace();
            Common.IsFolderValid(folder).Should().BeTrue();
            Common.IsFolderValid(Path.Combine(folder, Guid.NewGuid().ToString())).Should().BeFalse();
            Common.IsFolderValid(null).Should().BeFalse();
            Common.IsFolderValid("  ").Should().BeFalse();
        }

        [TestCase("*.*", @"^.*\..*$")]
        [TestCase("*.CR3", @"^.*\.CR3$")]
        [TestCase("*.CR2|*.CR3", @"^.*\.CR2|.*\.CR3$")]
        [TestCase("*.CR?", @"^.*\.CR.$")]
        [TestCase("*.??3", @"^.*\...3$")]
        public void WildcardToRegexTests(string filter, string result)
        {
            Common.WildcardToRegex(filter).Should().Be(result);
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
            var fi = new FileInfo("dummy" + file);
            Common.IsImageFile(fi).Should().Be(result);
        }

        [Test]
        public void GetExifDateOnly()
        {
            const string txtFile = "no_exif.txt";
            const string jpgFile = "exif.jpg";
            var exif = new DateTime(2001, 1, 1, 1, 1, 1);

            // TXT, because a JPG is always created with EXIF date
            var txt = files.Add(tempFolderPath, txtFile, tempFolderPath, txtFile);
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
            var txt = files.Add(tempFolderPath, txtFile, tempFolderPath, txtFile);
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

            var files = new List<FileInfo>();
            files.Add(base.files.Add(tempFolderPath, file3, tempFolderPath, file3, third, third, null));
            files.Add(base.files.Add(tempFolderPath, file2, tempFolderPath, file2, second, second, null));
            files.Add(base.files.Add(tempFolderPath, file1, tempFolderPath, file1, first, first, null));
            base.files.CreateFiles();
            files.ForEach(f => f.Refresh());

            var exifFiles = new List<ExifFile>();
            exifFiles.Add(new ExifFile(files[0]));
            exifFiles.Add(new ExifFile(files[1]));
            exifFiles.Add(new ExifFile(files[2]));

            exifFiles[0].Name.Should().Be(file3);
            exifFiles[1].Name.Should().Be(file2);
            exifFiles[2].Name.Should().Be(file1);

            exifFiles = exifFiles.OrderBy(f => f.ExifTime).ToList();

            exifFiles[0].Name.Should().Be(file1);
            exifFiles[1].Name.Should().Be(file2);
            exifFiles[2].Name.Should().Be(file3);
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

            var files = new List<ExifFile>();
            files.Add(new ExifFile(base.files.Add(tempFolderPath, file3, tempFolderPath, file3, null, null, third)));
            files.Add(new ExifFile(base.files.Add(tempFolderPath, file2, tempFolderPath, file2, null, null, second)));
            files.Add(new ExifFile(base.files.Add(tempFolderPath, file1, tempFolderPath, file1, null, null, first)));
            base.files.CreateFiles();

            files[0].Name.Should().Be(file3);
            files[1].Name.Should().Be(file2);
            files[2].Name.Should().Be(file1);

            files = files.OrderBy(f => f.ExifTime).ToList();

            files[0].Name.Should().Be(file1);
            files[1].Name.Should().Be(file2);
            files[2].Name.Should().Be(file3);
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

            var files = new List<FileInfo>();
            files.Add(base.files.Add(tempFolderPath, file2, tempFolderPath, file2, createdDate2, modifiedDate2, exifDate2));
            files.Add(base.files.Add(tempFolderPath, file1, tempFolderPath, file1, createdDate1, modifiedDate1, exifDate1));
            base.files.CreateFiles();
            files.ForEach(f => f.Refresh());

            var exifFiles = new List<ExifFile>();
            exifFiles.Add(new ExifFile(files[0]));
            exifFiles.Add(new ExifFile(files[1]));

            exifFiles[0].Name.Should().Be(file2);
            exifFiles[1].Name.Should().Be(file1);

            exifFiles = exifFiles.OrderBy(f => f.ExifTime).ToList();

            exifFiles[0].Name.Should().Be(file1);
            exifFiles[1].Name.Should().Be(file2);
        }

        [TestCase(".", "*.xxx")]
        [TestCase(".", null, typeof(ArgumentNullException))]
        [TestCase(".", "  ", typeof(ArgumentNullException))]
        [TestCase(null, "*.*", typeof(ArgumentNullException))]
        [TestCase("xxxxxxxxxxxx", "*.*", typeof(DirectoryNotFoundException))]
        public void GetFilesValidationTests(string folder, string filter, Type ex = null)
        {
            DirectoryInfo di = null;
            if (!string.IsNullOrWhiteSpace(folder))
                di = new DirectoryInfo(folder);

            if (ex != null)
                Assert.Throws(ex, delegate { Common.GetFiles(di, filter, false); });
            else
                Assert.That(Common.GetFiles(di, filter, false), Is.Not.Null);
        }

        [TestCase(".", true)]
        [TestCase(".", null, typeof(ArgumentNullException))]
        [TestCase(".", false, typeof(InvalidDataException))]
        [TestCase(null, true, typeof(ArgumentNullException))]
        [TestCase("xxxxxxxxxxxx", true, typeof(DirectoryNotFoundException))]
        public void GetFilesWithImportConfigValidationTests(string folder, bool? validConfig, Type ex = null)
        {
            DirectoryInfo di = null;
            if (!string.IsNullOrWhiteSpace(folder))
                di = new DirectoryInfo(folder);

            var config = new ImportConfig();
            if (validConfig == null)
                config = null;
            else if (validConfig == true)
                config.DestRoot = ".";
            else if (validConfig == false)
                config.DestRoot = null;

            if (ex != null)
                Assert.Throws(ex, delegate { Common.GetFiles(di, config); });
            else
                Assert.That(Common.GetFiles(di, config), Is.Not.Null);
        }

        [Test]
        public void GetFilesFindAll()
        {
            files.Add(tempFolderPath, "1.aaa");
            files.Add(tempFolderPath, "2.bbb");
            files.Add(tempFolderPath, "3.ccc");
            files.CreateFiles();

            var getFiles = Common.GetFiles(tempFolder, "*.*", false);

            Console.WriteLine("files:");
            files.ForEach(f => Console.WriteLine(f.GetSourcePath()));
            Console.WriteLine("GetFiles:");
            getFiles.ForEach(f => Console.WriteLine(f.FullName));

            getFiles.Where(gf => !files.Select(f => f.GetSourcePath()).Contains(gf.FullName)).Any().Should().BeFalse("'GetFiles has more results'");
            files.Where(f => !getFiles.Select(gf => gf.FullName).Contains(f.GetSourcePath())).Any().Should().BeFalse("'There are files missing from GetFiles result'");
        }

        [Test]
        public void GetFilesWithImportConfigFindAll()
        {
            files.Add(tempFolderPath, "1.aaa");
            files.Add(tempFolderPath, "2.bbb");
            files.Add(tempFolderPath, "3.ccc");
            files.CreateFiles();

            var config = new ImportConfig();
            config.DestRoot = tempFolderPath;
            config.FileFilter = "*.*";

            var getFiles = Common.GetFiles(tempFolder, config);

            Console.WriteLine("files:");
            files.ForEach(f => Console.WriteLine(f.GetSourcePath()));
            Console.WriteLine("GetFiles:");
            getFiles.ForEach(f => Console.WriteLine(f.FullName));

            getFiles.Where(gf => !files.Select(f => f.GetSourcePath()).Contains(gf.FullName)).Any().Should().BeFalse("'GetFiles has more results'");
            files.Where(f => !getFiles.Select(gf => gf.FullName).Contains(f.GetSourcePath())).Any().Should().BeFalse("'There are files missing from GetFiles result'");
        }

        [Test]
        public void GetFilesFindSome()
        {
            files.Add(tempFolderPath, "1.aaa");
            files.Add(tempFolderPath, "2.bbb");
            files.Add(tempFolderPath, "3.aaa");
            files.CreateFiles();

            var getFiles = Common.GetFiles(tempFolder, "*.AAA", false);

            Console.WriteLine("files:");
            files.ForEach(f => Console.WriteLine(f.GetSourcePath()));
            Console.WriteLine("GetFiles:");
            getFiles.ForEach(f => Console.WriteLine(f.FullName));

            getFiles.Where(gf => !files.Where(f => f.Name.EndsWith(".AAA", StringComparison.InvariantCultureIgnoreCase)).Select(f => f.GetSourcePath()).Contains(gf.FullName)).Any().Should().BeFalse("'GetFiles has more results'");
            files.Where(f => f.Name.EndsWith(".AAA", StringComparison.InvariantCultureIgnoreCase) && !getFiles.Select(gf => gf.FullName).Contains(f.GetSourcePath())).Any().Should().BeFalse("'There are files missing from GetFiles result'");
        }

        [Test]
        public void GetFilesWithImportConfigFindSome()
        {
            files.Add(tempFolderPath, "1.aaa");
            files.Add(tempFolderPath, "2.bbb");
            files.Add(tempFolderPath, "3.aaa");
            files.CreateFiles();

            var config = new ImportConfig();
            config.DestRoot = tempFolderPath;
            config.FileFilter = "*.AAA";

            var getFiles = Common.GetFiles(tempFolder, config);

            Console.WriteLine("files:");
            files.ForEach(f => Console.WriteLine(f.GetSourcePath()));
            Console.WriteLine("GetFiles:");
            getFiles.ForEach(f => Console.WriteLine(f.FullName));

            getFiles.Where(gf => !files.Where(f => f.Name.EndsWith(".AAA", StringComparison.InvariantCultureIgnoreCase)).Select(f => f.GetSourcePath()).Contains(gf.FullName)).Any().Should().BeFalse("'GetFiles has more results'");
            files.Where(f => f.Name.EndsWith(".AAA", StringComparison.InvariantCultureIgnoreCase) && !getFiles.Select(gf => gf.FullName).Contains(f.GetSourcePath())).Any().Should().BeFalse("'There are files missing from GetFiles result'");
        }

        [Test]
        public void GetFilesFindAllNonRecursive()
        {
            var sep = Path.DirectorySeparatorChar;
            files.Add(tempFolderPath, "1.aaa");
            files.Add(tempFolderPath, "2.bbb");
            files.Add(tempFolderPath, "3.ccc");
            files.Add(tempFolderPath, $"subfolder{sep}4.ddd");
            files.Add(tempFolderPath, $"subfolder{sep}5.eee");
            files.CreateFiles();
            files.RemoveAt(3);
            files.RemoveAt(3);

            var getFiles = Common.GetFiles(tempFolder, "*.*", false);

            Console.WriteLine("files:");
            files.ForEach(f => Console.WriteLine(f.GetSourcePath()));
            Console.WriteLine("GetFiles:");
            getFiles.ForEach(f => Console.WriteLine(f.FullName));

            getFiles.Where(gf => !files.Select(f => f.GetSourcePath()).Contains(gf.FullName)).Any().Should().BeFalse("'GetFiles has more results'");
            files.Where(f => !getFiles.Select(gf => gf.FullName).Contains(f.GetSourcePath())).Any().Should().BeFalse("'There are files missing from GetFiles result'");
        }

        [Test]
        public void GetFilesFindAllRecursive()
        {
            var sep = Path.DirectorySeparatorChar;
            files.Add(tempFolderPath, "1.aaa");
            files.Add(tempFolderPath, "2.bbb");
            files.Add(tempFolderPath, "3.ccc");
            files.Add(tempFolderPath, $"subfolder{sep}4.ddd");
            files.Add(tempFolderPath, $"subfolder{sep}5.eee");
            files.CreateFiles();

            var getFiles = Common.GetFiles(tempFolder, "*.*", true);

            Console.WriteLine("files:");
            files.ForEach(f => Console.WriteLine(f.GetSourcePath()));
            Console.WriteLine("GetFiles:");
            getFiles.ForEach(f => Console.WriteLine(f.FullName));

            getFiles.Where(gf => !files.Select(f => f.GetSourcePath()).Contains(gf.FullName)).Any().Should().BeFalse("'GetFiles has more results'");
            files.Where(f => !getFiles.Select(gf => gf.FullName).Contains(f.GetSourcePath())).Any().Should().BeFalse("'There are files missing from GetFiles result'");
        }

        [Test]
        public void GetFilesWithImportConfigFindAllRecursive()
        {
            var sep = Path.DirectorySeparatorChar;
            files.Add(tempFolderPath, "1.aaa");
            files.Add(tempFolderPath, "2.bbb");
            files.Add(tempFolderPath, "3.ccc");
            files.Add(tempFolderPath, $"subfolder{sep}4.ddd");
            files.Add(tempFolderPath, $"subfolder{sep}5.eee");
            files.CreateFiles();

            var config = new ImportConfig();
            config.DestRoot = tempFolderPath;
            config.FileFilter = "*.*";

            var getFiles = Common.GetFiles(tempFolder, config);

            Console.WriteLine("files:");
            files.ForEach(f => Console.WriteLine(f.GetSourcePath()));
            Console.WriteLine("GetFiles:");
            getFiles.ForEach(f => Console.WriteLine(f.FullName));

            getFiles.Where(gf => !files.Select(f => f.GetSourcePath()).Contains(gf.FullName)).Any().Should().BeFalse("'GetFiles has more results'");
            files.Where(f => !getFiles.Select(gf => gf.FullName).Contains(f.GetSourcePath())).Any().Should().BeFalse("'There are files missing from GetFiles result'");
        }

        [Test]
        public void GetFilesFindSomeRecursive()
        {
            var sep = Path.DirectorySeparatorChar;
            files.Add(tempFolderPath, "1.aaa");
            files.Add(tempFolderPath, "2.bbb");
            files.Add(tempFolderPath, "3.aaa");
            files.Add(tempFolderPath, $"subfolder{sep}4.ddd");
            files.Add(tempFolderPath, $"subfolder{sep}5.aaa");
            files.CreateFiles();

            var getFiles = Common.GetFiles(tempFolder, "*.AAA", true);

            Console.WriteLine("files:");
            files.ForEach(f => Console.WriteLine(f.GetSourcePath()));
            Console.WriteLine("GetFiles:");
            getFiles.ForEach(f => Console.WriteLine(f.FullName));

            getFiles.Where(gf => !files.Where(f => f.Name.EndsWith(".AAA", StringComparison.InvariantCultureIgnoreCase)).Select(f => f.GetSourcePath()).Contains(gf.FullName)).Any().Should().BeFalse("'GetFiles has more results'");
            files.Where(f => f.Name.EndsWith(".AAA", StringComparison.InvariantCultureIgnoreCase) && !getFiles.Select(gf => gf.FullName).Contains(f.GetSourcePath())).Any().Should().BeFalse("'There are files missing from GetFiles result'");
        }

        [Test]
        public void GetFilesWithImportConfigFindSomeRecursive()
        {
            var sep = Path.DirectorySeparatorChar;
            files.Add(tempFolderPath, "1.aaa");
            files.Add(tempFolderPath, "2.bbb");
            files.Add(tempFolderPath, "3.aaa");
            files.Add(tempFolderPath, $"subfolder{sep}4.ddd");
            files.Add(tempFolderPath, $"subfolder{sep}5.aaa");
            files.CreateFiles();

            var config = new ImportConfig();
            config.DestRoot = tempFolderPath;
            config.FileFilter = "*.AAA";

            var getFiles = Common.GetFiles(tempFolder, config);

            Console.WriteLine("files:");
            files.ForEach(f => Console.WriteLine(f.GetSourcePath()));
            Console.WriteLine("GetFiles:");
            getFiles.ForEach(f => Console.WriteLine(f.FullName));

            getFiles.Where(gf => !files.Where(f => f.Name.EndsWith(".AAA", StringComparison.InvariantCultureIgnoreCase)).Select(f => f.GetSourcePath()).Contains(gf.FullName)).Any().Should().BeFalse("'GetFiles has more results'");
            files.Where(f => f.Name.EndsWith(".AAA", StringComparison.InvariantCultureIgnoreCase) && !getFiles.Select(gf => gf.FullName).Contains(f.GetSourcePath())).Any().Should().BeFalse("'There are files missing from GetFiles result'");
        }

    }
}