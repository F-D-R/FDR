using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    public class RenamePatternTest : TestFixtureBase
    {
        private string tempFolderPath;
        private string folderPath;
        private string filePath;
        private DirectoryInfo folder;
        private FileInfo file;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolderPath);

            folderPath = Path.Combine(tempFolderPath, "temp", "testfolder");
            Directory.CreateDirectory(folderPath);

            filePath = Path.Combine(folderPath, "abcdef.txt");
            File.WriteAllText(filePath, "");
            File.SetCreationTime(filePath, new DateTime(2007, 8, 9));
            File.SetLastWriteTime(filePath, new DateTime(2010, 11, 12));
            file = new FileInfo(filePath);

            Directory.SetCreationTime(folderPath, new DateTime(2001, 2, 3));
            Directory.SetLastWriteTime(folderPath, new DateTime(2004, 5, 6));
            folder = new DirectoryInfo(folderPath);
        }

        public override void OneTimeTearDown()
        {
            if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath, true);

            base.OneTimeTearDown();
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
            Rename.EvaluateNamePattern("pre_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post", folder)
                .Should().Be($"pre_2001_02_03_post");

            Rename.EvaluateNamePattern("pre_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post", file)
                .Should().Be($"pre_2010_11_12_post");
            Rename.EvaluateNamePattern("pre_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post", folder)
                .Should().Be($"pre_2004_05_06_post");

            var date = DateTime.Now;
            Rename.EvaluateNamePattern("pre_{now:yyyy}_{now:MM}_{now:dd}_post", null)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");
        }

        [Test]
        public void EvaluateFolderNamePattern()
        {
            Rename.EvaluateFolderNamePattern("pre_{name:0,2}_{name:2,2}_post", folder)
                .Should().Be($"pre_te_st_post");

            Rename.EvaluateFolderNamePattern("pre_{cdate:yyyy}_{cdate:MM}_{cdate:dd}_post", folder)
                .Should().Be($"pre_2001_02_03_post");

            Rename.EvaluateFolderNamePattern("pre_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post", folder)
                .Should().Be($"pre_2004_05_06_post");

            var date = DateTime.Now;
            Rename.EvaluateFolderNamePattern("pre_{now:yyyy}_{now:MM}_{now:dd}_post", null)
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

            Rename.EvaluateFileNamePattern("pre_{mdate:yyyy}_{mdate:MM}_{mdate:dd}_post", file, 1)
                .Should().Be($"pre_2010_11_12_post");

            var date = DateTime.Now;
            Rename.EvaluateFileNamePattern("pre_{now:yyyy}_{now:MM}_{now:dd}_post", null, 1)
                .Should().Be($"pre_{date:yyyy}_{date:MM}_{date:dd}_post");

            Rename.EvaluateFileNamePattern("pre_{pfolder:0,2}_{pfolder:2,2}_post", file, 1)
                .Should().Be($"pre_te_st_post");

            Rename.EvaluateFileNamePattern("pre_{counter}_{counter:3}_post", null, 1)
                .Should().Be($"pre_1_001_post");
        }
    }
}