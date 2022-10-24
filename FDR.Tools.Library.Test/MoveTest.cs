using System.IO;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class MoveTest : TempFolderTestBase
    {
        private string parentFolderPath;
        private string sourceFolderPath;
        private string childFolderPath;
        private string grandChildFolderPath;
        private string parallelFolderPath;
        private DirectoryInfo sourceFolder;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            parentFolderPath = Path.Combine(tempFolderPath, "parent");
            sourceFolderPath = Path.Combine(parentFolderPath, "source");
            childFolderPath = Path.Combine(sourceFolderPath, "child");
            grandChildFolderPath = Path.Combine(childFolderPath, "grandchild");
            parallelFolderPath = Path.Combine(parentFolderPath, "parallel");
            sourceFolder = new DirectoryInfo(sourceFolderPath);
        }

        [Test]
        public void MoveFilesToChildFolder()
        {
            var config = new MoveConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.RelativeFolder = "child";

            files.Add(sourceFolderPath, "aaa.cr3", childFolderPath, "aaa.cr3");
            files.Add(sourceFolderPath, "bbb.cr2", childFolderPath, "bbb.cr2");
            files.Add(sourceFolderPath, "ccc.jpg", sourceFolderPath, "ccc.jpg");
            files.CreateFiles();

            Import.MoveFilesInFolder(sourceFolder, config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void MoveFilesToGrandChildFolder()
        {
            var config = new MoveConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.RelativeFolder = "child/grandchild";

            files.Add(sourceFolderPath, "aaa.cr3", grandChildFolderPath, "aaa.cr3");
            files.Add(sourceFolderPath, "bbb.cr2", grandChildFolderPath, "bbb.cr2");
            files.Add(sourceFolderPath, "ccc.jpg", sourceFolderPath, "ccc.jpg");
            files.CreateFiles();

            Import.MoveFilesInFolder(sourceFolder, config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void MoveFilesToParentFolder()
        {
            var config = new MoveConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.RelativeFolder = "..";

            files.Add(sourceFolderPath, "aaa.cr3", parentFolderPath, "aaa.cr3");
            files.Add(sourceFolderPath, "bbb.cr2", parentFolderPath, "bbb.cr2");
            files.Add(sourceFolderPath, "ccc.jpg", sourceFolderPath, "ccc.jpg");
            files.CreateFiles();

            Import.MoveFilesInFolder(sourceFolder, config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void MoveFilesToParallelFolder()
        {
            var config = new MoveConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.RelativeFolder = "../parallel";

            files.Add(sourceFolderPath, "aaa.cr3", parallelFolderPath, "aaa.cr3");
            files.Add(sourceFolderPath, "bbb.cr2", parallelFolderPath, "bbb.cr2");
            files.Add(sourceFolderPath, "ccc.jpg", sourceFolderPath, "ccc.jpg");
            files.CreateFiles();

            Import.MoveFilesInFolder(sourceFolder, config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesToGrandChildFolder()
        {
            var config = new MoveConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.FilenamePattern = "{name}_{name}";
            config.RelativeFolder = "child/grandchild";

            files.Add(sourceFolderPath, "aaa.cr3", grandChildFolderPath, "aaa_aaa.cr3");
            files.Add(sourceFolderPath, "bbb.cr2", grandChildFolderPath, "bbb_bbb.cr2");
            files.Add(sourceFolderPath, "ccc.jpg", sourceFolderPath, "ccc.jpg");
            files.CreateFiles();

            Import.MoveFilesInFolder(sourceFolder, config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }

        [Test]
        public void RenameFilesToGrandChildFolder2()
        {
            var config = new MoveConfig();
            config.Should().NotBeNull();
            config.FileFilter = "*.CR3|*.CR2";
            config.FilenamePattern = "grandchild/{name}_{name}";
            config.RelativeFolder = "child";

            files.Add(sourceFolderPath, "aaa.cr3", grandChildFolderPath, "aaa_aaa.cr3");
            files.Add(sourceFolderPath, "bbb.cr2", grandChildFolderPath, "bbb_bbb.cr2");
            files.Add(sourceFolderPath, "ccc.jpg", sourceFolderPath, "ccc.jpg");
            files.CreateFiles();

            Import.MoveFilesInFolder(sourceFolder, config);

            files.ForEach(f => File.Exists(f.GetDestPath()).Should().Be(f.Keep, f.Name));
        }
    }
}