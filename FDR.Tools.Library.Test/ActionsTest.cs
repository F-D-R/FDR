﻿using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace FDR.Tools.Library.Test
{
    public class ActionsTest : TestFixtureBase
    {
        private string tempFolderPath;
        private DirectoryInfo folder;
        private string rawFolderPath;
        private TestFiles files = new TestFiles();

        private class TestFile
        {
            public TestFile(DateTime created, string sourceFolder, string sourceName, string destFolder, string destName)
            {
                Keep = true;
                Create = true;
                Created = created;
                SourceFolder = sourceFolder;
                SourceName = sourceName;
                DestFolder = destFolder;
                DestName = destName;
            }
            public TestFile(string destFolder, string destName, bool keep)
            {
                Keep = keep;
                Create = false;
                DestFolder = destFolder;
                DestName = destName;
            }
            public bool Keep;
            public bool Create;
            public DateTime Created;
            public string SourceFolder;
            public string SourceName;
            public string DestFolder;
            public string DestName;
            public string SourcePath => System.IO.Path.Combine(SourceFolder, SourceName);
            public string DestPath => System.IO.Path.Combine(DestFolder, DestName);
        }

        private class TestFiles : List<TestFile>
        {
            public void Add(DateTime created, string sourceFolder, string sourceName, string destFolder, string destName)
            {
                base.Add(new TestFile(created, sourceFolder, sourceName, destFolder, destName));
            }
            public void Add(string destFolder, string destName, bool keep = true)
            {
                base.Add(new TestFile(destFolder, destName, keep));
            }
        }

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolderPath);
            folder = new DirectoryInfo(tempFolderPath);

            rawFolderPath = Path.Combine(tempFolderPath, "RAW");
            Directory.CreateDirectory(rawFolderPath);
        }

        public override void OneTimeTearDown()
        {
            if (Directory.Exists(tempFolderPath)) Directory.Delete(tempFolderPath, true);

            base.OneTimeTearDown();
        }

        public override void SetUp()
        {
            base.SetUp();

            Directory.GetFiles(tempFolderPath, "*", SearchOption.AllDirectories).ToList().ForEach(f => File.Delete(f));
            files.Clear();
        }

        public override void TearDown()
        {
            Directory.GetFiles(tempFolderPath, "*", SearchOption.AllDirectories).ToList().ForEach(f => File.Delete(f));
            files.Clear();

            base.TearDown();
        }

        [Test]
        public void ActionsTests()
        {
            var appConfig = new AppConfig();
            appConfig.Should().NotBeNull();
            appConfig.MoveConfigs.Add("move", new MoveConfig());
            appConfig.MoveConfigs.Should().HaveCount(1);
            appConfig.BatchRenameConfigs.Add("rename", new BatchRenameConfig() { FilenamePattern = "dest_{counter:3}", AdditionalFileTypes = { ".JPG" } });
            appConfig.BatchRenameConfigs.Should().HaveCount(1);
            var actions = new Actions(appConfig);
            actions.Should().NotBeNull();
            actions.Add(new ActionClass() { Type = ActionType.hash });
            actions.Add(new ActionClass() { Type = ActionType.rename, Config = "rename" });
            actions.Add(new ActionClass() { Type = ActionType.move, Config = "move" });
            actions.Add(new ActionClass() { Type = ActionType.hash });
            actions.Add(new ActionClass() { Type = ActionType.cleanup });
            //actions.Add(new ActionClass() { Type = ActionType.rehash });
            actions.Should().HaveCount(5);

            files.Add(new DateTime(2022, 1, 3), tempFolderPath, "01.jpg", tempFolderPath, "dest_003.jpg");
            files.Add(new DateTime(2022, 1, 2), tempFolderPath, "02.jpg", tempFolderPath, "dest_002.jpg");
            files.Add(new DateTime(2022, 1, 1), tempFolderPath, "03.jpg", tempFolderPath, "dest_001.jpg");
            files.Add(tempFolderPath, ".01.jpg.md5", false);
            files.Add(tempFolderPath, ".02.jpg.md5", false);
            files.Add(tempFolderPath, ".03.jpg.md5", false);
            files.Add(tempFolderPath, ".dest_003.jpg.md5");
            files.Add(tempFolderPath, ".dest_002.jpg.md5");
            files.Add(tempFolderPath, ".dest_001.jpg.md5");

            files.Add(new DateTime(2022, 1, 3), tempFolderPath, "01.crw", rawFolderPath, "dest_003.crw");
            files.Add(new DateTime(2022, 1, 2), tempFolderPath, "02.cr2", rawFolderPath, "dest_002.cr2");
            files.Add(new DateTime(2022, 1, 1), tempFolderPath, "03.cr3", rawFolderPath, "dest_001.cr3");
            files.Add(tempFolderPath, ".01.crw.md5", false);
            files.Add(tempFolderPath, ".02.cr2.md5", false);
            files.Add(tempFolderPath, ".03.cr3.md5", false);
            files.Add(rawFolderPath, ".dest_003.crw.md5");
            files.Add(rawFolderPath, ".dest_002.cr2.md5");
            files.Add(rawFolderPath, ".dest_001.cr3.md5");

            files.Should().HaveCount(18);

            files.Where(f => f.Create).ToList().ForEach(f => { File.WriteAllText(f.SourcePath, ""); File.SetCreationTimeUtc(f.SourcePath, f.Created); });

            actions.ForEach(a => a.Do(folder));

            files.ForEach(f => File.Exists(f.DestPath).Should().Be(f.Keep, f.DestName));
        }
    }
}
