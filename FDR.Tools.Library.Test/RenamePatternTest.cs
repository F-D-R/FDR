using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class RenamePatternTest : TempFolderTestBase
    {
        private string folderPath;
        private DirectoryInfo folder;
        private string filePath;
        private FileInfo file;
        private ExifFile exifFile;

        public override void SetUp()
        {
            base.SetUp();

            folderPath = Path.Combine(tempFolderPath, "temp", "testfolder");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            filePath = Path.Combine(folderPath, "abcdef.txt");
            File.WriteAllText(filePath, "");
            File.SetCreationTime(filePath, new DateTime(2007, 8, 9));
            File.SetLastWriteTime(filePath, new DateTime(2010, 11, 12));
            file = new FileInfo(filePath);
            exifFile = new ExifFile(file);

            Directory.SetCreationTime(folderPath, new DateTime(2001, 2, 3));
            Directory.SetLastWriteTime(folderPath, new DateTime(2004, 5, 6));
            folder = new DirectoryInfo(folderPath);
        }

        [TestCase("{NAME}", "name")]
        [TestCase("abc{NAME}def", "name")]
        [TestCase("{NAME:0,2}", "name", "0,2")]
        [TestCase("abc{NAME:0,2}def", "name", "0,2")]
        [TestCase("{NAME:0,2:order}", "name", "0,2", "order")]
        [TestCase("abc{NAME:0,2:order}def", "name", "0,2", "order")]
        [TestCase("{EDATE:yyMMdd}", "edate", "yyMMdd")]
        [TestCase("{EDATE:yyMMdd_HHmmss}", "edate", "yyMMdd_HHmmss")]
        [TestCase("abc{EDATE:yyMMdd}def", "edate", "yyMMdd")]
        [TestCase("{COUNTER}", "counter")]
        [TestCase("{COUNTER:2}", "counter", "2")]
        [TestCase("abc{COUNTER:2}def", "counter", "2")]
        [TestCase("{COUNTER:2:edate}", "counter", "2", "EDATE")]
        [TestCase("abc{COUNTER:2:edate}def", "counter", "2", "EDATE")]
        public void RegexPatternTest(string pattern, string first, string second = null, string third = null)
        {
            Console.WriteLine("Regex: " + Rename.REGEX + " Pattern: " + pattern);

            var regex = new Regex(Rename.REGEX, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match match in regex.Matches(pattern))
            {
                Console.WriteLine("First: " + first??"" + " First group: " + match.Groups[1].Value??"");
                string.Compare(match.Groups[1].Value, first, true).Should().Be(0, "'First group'");
                if (match.Groups.Count > 2 && !string.IsNullOrEmpty(match.Groups[2].Value))
                {
                    Console.WriteLine("Second: " + second??"" + " Second group: " + match.Groups[2].Value??"");
                    string.Compare(match.Groups[2].Value, second, true).Should().Be(0, "'Second group'");
                }
                if (match.Groups.Count > 3 && !string.IsNullOrEmpty(match.Groups[3].Value))
                {
                    Console.WriteLine("Third: " + third??"" + " Third group: " + match.Groups[3].Value??"");
                    string.Compare(match.Groups[3].Value, third, true).Should().Be(0, "'Third group'");
                }
            }
        }

        [Test]
        public void EvaluateNamePattern()
        {
            Rename.EvaluateNamePattern("pre_{name:0,2}_{name:2,2}_post", file)
                .Should().Be($"pre_ab_cd_post");
            Rename.EvaluateNamePattern("pre_{name:0,2}_{name:2,2}_post", folder)
                .Should().Be($"pre_te_st_post");

            Rename.EvaluateNamePattern("pre_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post", file)
                .Should().Be($"pre_2007_08_09_post");
            Rename.EvaluateNamePattern("pre_{cdate:yyyy_MM_dd}_post", file)
                .Should().Be($"pre_2007_08_09_post");
            Rename.EvaluateNamePattern("pre_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post", folder)
                .Should().Be($"pre_2001_02_03_post");
            Rename.EvaluateNamePattern("pre_{cdate:yyyy_MM_dd}_post", folder)
                .Should().Be($"pre_2001_02_03_post");

            Rename.EvaluateNamePattern("pre_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post", file)
                .Should().Be($"pre_2010_11_12_post");
            Rename.EvaluateNamePattern("pre_{mdate:yyyy_MM_dd}_post", file)
                .Should().Be($"pre_2010_11_12_post");
            Rename.EvaluateNamePattern("pre_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post", folder)
                .Should().Be($"pre_2004_05_06_post");
            Rename.EvaluateNamePattern("pre_{mdate:yyyy_MM_dd}_post", folder)
                .Should().Be($"pre_2004_05_06_post");

            var date = DateTime.Now;
            Rename.EvaluateNamePattern("pre_{now:yyyy}_{now:MM}_{now:dd}_post", null)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");
            Rename.EvaluateNamePattern("pre_{now:yyyy_MM_dd}_post", null)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");
        }

        [Test]
        public void EvaluateFolderNamePattern()
        {
            Rename.EvaluateFolderNamePattern("pre_{name:0,2}_{name:2,2}_post", folder)
                .Should().Be($"pre_te_st_post");

            Rename.EvaluateFolderNamePattern("pre_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post", folder)
                .Should().Be($"pre_2001_02_03_post");
            Rename.EvaluateFolderNamePattern("pre_{cdate:yyyy_MM_dd}_post", folder)
                .Should().Be($"pre_2001_02_03_post");

            Rename.EvaluateFolderNamePattern("pre_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post", folder)
                .Should().Be($"pre_2004_05_06_post");
            Rename.EvaluateFolderNamePattern("pre_{mdate:yyyy_MM_dd}_post", folder)
                .Should().Be($"pre_2004_05_06_post");

            var date = DateTime.Now;
            Rename.EvaluateFolderNamePattern("pre_{now:yyyy}_{now:MM}_{now:dd}_post", null)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");
            Rename.EvaluateFolderNamePattern("pre_{now:yyyy_MM_dd}_post", null)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");

            Rename.EvaluateFolderNamePattern("pre_{pfolder:0,2}_{pfolder:2,2}_post", folder)
                .Should().Be($"pre_te_mp_post");
        }

        [Test]
        public void EvaluateFileNamePattern()
        {
            Rename.EvaluateFileNamePattern("pre_{name:0,2}_{name:2,2}_post", file, 1)
                .Should().Be($"pre_ab_cd_post");

            Rename.EvaluateFileNamePattern("pre_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post", file, 1)
                .Should().Be($"pre_2007_08_09_post");
            Rename.EvaluateFileNamePattern("pre_{cdate:yyyy_MM_dd}_post", file, 1)
                .Should().Be($"pre_2007_08_09_post");

            Rename.EvaluateFileNamePattern("pre_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post", file, 1)
                .Should().Be($"pre_2010_11_12_post");
            Rename.EvaluateFileNamePattern("pre_{mdate:yyyy_MM_dd}_post", file, 1)
                .Should().Be($"pre_2010_11_12_post");

            var date = DateTime.Now;
            Rename.EvaluateFileNamePattern("pre_{now:yyyy}_{now:MM}_{now:dd}_post", (FileInfo)null, 1)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");
            Rename.EvaluateFileNamePattern("pre_{now:yyyy_MM_dd}_post", (FileInfo)null, 1)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");

            Rename.EvaluateFileNamePattern("pre_{pfolder:0,2}_{pfolder:2,2}_post", file, 1)
                .Should().Be($"pre_te_st_post");

            Rename.EvaluateFileNamePattern("pre_{counter}_{counter:3}_post", (FileInfo)null, 1)
                .Should().Be($"pre_1_001_post");
        }

        [Test]
        public void EvaluateExifFileNamePattern()
        {
            Rename.EvaluateFileNamePattern("pre_{name:0,2}_{name:2,2}_post", exifFile, 1)
                .Should().Be($"pre_ab_cd_post");

            Rename.EvaluateFileNamePattern("pre_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post", exifFile, 1)
                .Should().Be($"pre_2007_08_09_post");
            Rename.EvaluateFileNamePattern("pre_{cdate:yyyy_MM_dd}_post", exifFile, 1)
                .Should().Be($"pre_2007_08_09_post");

            Rename.EvaluateFileNamePattern("pre_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post", exifFile, 1)
                .Should().Be($"pre_2010_11_12_post");
            Rename.EvaluateFileNamePattern("pre_{mdate:yyyy_MM_dd}_post", exifFile, 1)
                .Should().Be($"pre_2010_11_12_post");

            var date = DateTime.Now;
            Rename.EvaluateFileNamePattern("pre_{now:yyyy}_{now:MM}_{now:dd}_post", (ExifFile)null, 1)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");
            Rename.EvaluateFileNamePattern("pre_{now:yyyy_MM_dd}_post", (ExifFile)null, 1)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");

            Rename.EvaluateFileNamePattern("pre_{pfolder:0,2}_{pfolder:2,2}_post", exifFile, 1)
                .Should().Be($"pre_te_st_post");

            Rename.EvaluateFileNamePattern("pre_{counter}_{counter:3}_post", (ExifFile)null, 1)
                .Should().Be($"pre_1_001_post");
        }

        [Test]
        public void CalculateFileName()
        {
            var config = new RenameConfig();
            var date = DateTime.Now;

            config.FilenamePattern = "PRE_{name:0,2}_{name:2,2}_post";
            config.FilenameCase = CharacterCasing.unchanged;
            config.ExtensionCase = CharacterCasing.unchanged;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, "PRE_ab_cd_post.txt"));

            config.FilenamePattern = "PRE_{name:0,2}_{name:2,2}_post";
            config.FilenameCase = CharacterCasing.upper;
            config.ExtensionCase = CharacterCasing.lower;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, "PRE_AB_CD_POST.txt"));

            config.FilenamePattern = "PRE_{name:0,2}_{name:2,2}_post";
            config.FilenameCase = CharacterCasing.lower;
            config.ExtensionCase = CharacterCasing.upper;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, "pre_ab_cd_post.TXT"));


            config.FilenamePattern = "PRE_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post";
            config.FilenameCase = CharacterCasing.unchanged;
            config.ExtensionCase = CharacterCasing.unchanged;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, "PRE_2007_08_09_post.txt"));

            config.FilenamePattern = "PRE_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post";
            config.FilenameCase = CharacterCasing.upper;
            config.ExtensionCase = CharacterCasing.lower;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, "PRE_2007_08_09_POST.txt"));

            config.FilenamePattern = "PRE_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post";
            config.FilenameCase = CharacterCasing.lower;
            config.ExtensionCase = CharacterCasing.upper;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, "pre_2007_08_09_post.TXT"));


            config.FilenamePattern = "PRE_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post";
            config.FilenameCase = CharacterCasing.unchanged;
            config.ExtensionCase = CharacterCasing.unchanged;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, "PRE_2010_11_12_post.txt"));

            config.FilenamePattern = "PRE_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post";
            config.FilenameCase = CharacterCasing.upper;
            config.ExtensionCase = CharacterCasing.lower;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, "PRE_2010_11_12_POST.txt"));

            config.FilenamePattern = "PRE_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post";
            config.FilenameCase = CharacterCasing.lower;
            config.ExtensionCase = CharacterCasing.upper;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, "pre_2010_11_12_post.TXT"));


            config.FilenamePattern = "PRE_{now:yyyy}_{now:MM}_{now:dd}_post";
            config.FilenameCase = CharacterCasing.unchanged;
            config.ExtensionCase = CharacterCasing.unchanged;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, $"PRE_{date:yyyy}_{date:MM}_{date:dd}_post.txt"));

            config.FilenamePattern = "PRE_{now:yyyy}_{now:MM}_{now:dd}_post";
            config.FilenameCase = CharacterCasing.upper;
            config.ExtensionCase = CharacterCasing.lower;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, $"PRE_{date:yyyy}_{date:MM}_{date:dd}_POST.txt"));

            config.FilenamePattern = "PRE_{now:yyyy}_{now:MM}_{now:dd}_post";
            config.FilenameCase = CharacterCasing.lower;
            config.ExtensionCase = CharacterCasing.upper;
            Rename.CalculateFileName(exifFile, config)
                .Should().Be(Path.Combine(folderPath, $"pre_{date:yyyy}_{date:MM}_{date:dd}_post.TXT"));
        }

        [TestCase("abcdef.txt", true)]
        [TestCase("abcdef.txt", null, typeof(ArgumentNullException))]
        [TestCase(null, true, typeof(ArgumentNullException))]
        [TestCase("xxxxxxxxxxxx", true, typeof(FileNotFoundException))]
        public void CalculateFileNameValidationTests(string fileName, bool? validConfig, Type ex = null)
        {
            ExifFile ef = null;
            if (!string.IsNullOrWhiteSpace(fileName))
                ef = new ExifFile(new FileInfo(Path.Combine(folderPath, fileName)));

            var config = new RenameConfig();
            if (validConfig == null)
                config = null;

            if (ex != null)
                Assert.Throws(ex, delegate { Rename.CalculateFileName(ef, config); });
            else
                Assert.That(Rename.CalculateFileName(ef, config), Is.Not.Null);
        }

    }
}