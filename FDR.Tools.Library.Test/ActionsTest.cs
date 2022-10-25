using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class ActionsTest : TempFolderTestBase
    {
        private string rawFolderPath;

        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            rawFolderPath = Path.Combine(tempFolderPath, "RAW");
            Directory.CreateDirectory(rawFolderPath);
        }

        [Test]
        public void ActionsTests()
        {
            var appConfig = new AppConfig();
            appConfig.Should().NotBeNull();
            appConfig.RenameConfigs.Add("rename", new RenameConfig() { FileFilter = "*.CR3|*.CR2|*.CRW", FilenamePattern = "dest_{counter:3}", AdditionalFileTypes = { ".JPG" } });
            appConfig.RenameConfigs.Should().HaveCount(1);
            appConfig.MoveConfigs.Add("move", new MoveConfig() { FileFilter = "*.CR3|*.CR2|*.CRW", RelativeFolder = "RAW" });
            appConfig.MoveConfigs.Should().HaveCount(1);

            var actions = new Actions(appConfig);
            actions.Should().NotBeNull();
            actions.Add(new Action() { Type = ActionType.hash });
            actions.Add(new Action() { Type = ActionType.rename, Config = "rename" });
            actions.Add(new Action() { Type = ActionType.move, Config = "move" });
            //actions.Add(new Action() { Type = ActionType.hash });
            actions.Add(new Action() { Type = ActionType.rehash });
            actions.Add(new Action() { Type = ActionType.cleanup });
            actions.Should().HaveCount(5);
            //actions.Add(new Action() { Type = ActionType.rehash });
            //actions.Should().HaveCount(6);

            files.Add(new DateTime(2022, 1, 3), tempFolderPath, "01.jpg", tempFolderPath, "dest_003.jpg");
            files.Add(new DateTime(2022, 1, 2), tempFolderPath, "02.jpg", tempFolderPath, "dest_002.jpg");
            files.Add(new DateTime(2022, 1, 1), tempFolderPath, "03.jpg", tempFolderPath, "dest_001.jpg");
            files.Add(new DateTime(2022, 1, 3), tempFolderPath, "01.crw", rawFolderPath, "dest_003.crw");
            files.Add(new DateTime(2022, 1, 2), tempFolderPath, "02.cr2", rawFolderPath, "dest_002.cr2");
            files.Add(new DateTime(2022, 1, 1), tempFolderPath, "03.cr3", rawFolderPath, "dest_001.cr3");
            files.CreateFiles();

            files.Add(tempFolderPath, ".01.jpg.md5", false);
            files.Add(tempFolderPath, ".02.jpg.md5", false);
            files.Add(tempFolderPath, ".03.jpg.md5", false);
            files.Add(tempFolderPath, ".dest_003.jpg.md5");
            files.Add(tempFolderPath, ".dest_002.jpg.md5");
            files.Add(tempFolderPath, ".dest_001.jpg.md5");

            files.Add(tempFolderPath, ".01.crw.md5", false);
            files.Add(tempFolderPath, ".02.cr2.md5", false);
            files.Add(tempFolderPath, ".03.cr3.md5", false);
            files.Add(rawFolderPath, ".dest_003.crw.md5");
            files.Add(rawFolderPath, ".dest_002.cr2.md5");
            files.Add(rawFolderPath, ".dest_001.cr3.md5");

            actions.ForEach(a => a.Do(tempFolder));

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.DestName));
        }
    }
}
