using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    public class RenameTests : TestBase
    {
        private const string folderPath = @"C:\temp\testfolder\";
        private const string filePath = @"C:\temp\testfolder\abcdef.txt";
        private DirectoryInfo folder;
        private FileInfo file;

        [SetUp]
        public void Setup()
        {
        }

        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            Directory.SetCreationTime(folderPath, new DateTime(2001, 2, 3));
            Directory.SetLastWriteTime(folderPath, new DateTime(2004, 5, 6));
            folder = new DirectoryInfo(folderPath);

            if (!File.Exists(filePath)) File.CreateText(filePath);
            File.SetCreationTime(filePath, new DateTime(2007, 8, 9));
            File.SetLastWriteTime(filePath, new DateTime(2010, 11, 12));
            file = new FileInfo(filePath);
        }

        public override void FixtureDestroy()
        {
            //if (File.Exists(filePath)) File.Delete(filePath);
            //if (file.Exists) file.Delete();

            //if (Directory.Exists(folderPath)) Directory.Delete(folderPath);
            //if (folder.Exists) folder.Delete();

            base.FixtureDestroy();
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




        //[TestCase("password", "salt")]
        //[TestCase("L23df%tf", "1d6f71c4-3251-46f6-9f80-d5b5208fa83c")]
        //public void GivenAPasswordWithSalt_WhenHashPassword_ThenShouldReturnHash(string password, string salt)
        //{
        //    var hash = _subject.HashPassword(password, salt);

        //    hash.Should().NotBeNullOrEmpty();

        //    hash.Should().NotContain(password);
        //    hash.Should().NotContain(salt);
        //}

    }
}