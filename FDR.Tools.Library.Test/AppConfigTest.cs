using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class AppConfigTest
    {
        [Test]
        public void AppConfigTests()
        {
            var config = new AppConfig();
            config.Should().NotBeNull();
            System.Action validate = () => config.Validate();
            validate.Should().NotThrow();

            config.RenameConfigs.Should().HaveCount(0);
            var rename = new RenameConfig();
            rename.Should().NotBeNull();
            config.RenameConfigs.Add("rename", rename);
            config.RenameConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            rename.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.RenameConfigs.Clear();
            config.RenameConfigs.Should().HaveCount(0);
            validate.Should().NotThrow();

            config.BatchRenameConfigs.Should().HaveCount(0);
            var batchrename = new BatchRenameConfig();
            batchrename.Should().NotBeNull();
            config.BatchRenameConfigs.Add("batchrename", batchrename);
            config.BatchRenameConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            batchrename.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.BatchRenameConfigs.Clear();
            config.BatchRenameConfigs.Should().HaveCount(0);
            validate.Should().NotThrow();

            config.ResizeConfigs.Should().HaveCount(0);
            var resize = new ResizeConfig();
            resize.Should().NotBeNull();
            config.ResizeConfigs.Add("resize", resize);
            config.ResizeConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            resize.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.ResizeConfigs.Clear();
            config.ResizeConfigs.Should().HaveCount(0);
            validate.Should().NotThrow();

            config.BatchResizeConfigs.Should().HaveCount(0);
            var batchresize = new BatchResizeConfig();
            batchresize.Should().NotBeNull();
            config.BatchResizeConfigs.Add("batchresize", batchresize);
            config.BatchResizeConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            batchresize.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.BatchResizeConfigs.Clear();
            config.BatchResizeConfigs.Should().HaveCount(0);
            validate.Should().NotThrow();

            config.ImportConfigs.Should().HaveCount(0);
            var import = new ImportConfig();
            import.Should().NotBeNull();
            config.ImportConfigs.Add("import", import);
            config.ImportConfigs.Should().HaveCount(1);
            validate.Should().Throw<InvalidDataException>();
            import.DestRoot = "dummy";
            validate.Should().NotThrow();
            config.ImportConfigs.Clear();
            config.ImportConfigs.Should().HaveCount(0);
            validate.Should().NotThrow();
        }
    }
}