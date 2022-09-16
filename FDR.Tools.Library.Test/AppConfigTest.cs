using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    public class AppConfigTest : TestFixtureBase
    {
        [Test]
        public void AppConfigTests()
        {
            var config = new AppConfig();
            Action validate = () => config.Validate();
            validate.Should().NotThrow();

            var rename = new RenameConfig();
            config.RenameConfigs.Add("rename", rename);
            validate.Should().NotThrow();
            rename.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.RenameConfigs.Clear();
            validate.Should().NotThrow();

            var batchrename = new BatchRenameConfig();
            config.BatchRenameConfigs.Add("batchrename", batchrename);
            validate.Should().NotThrow();
            batchrename.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.BatchRenameConfigs.Clear();
            validate.Should().NotThrow();

            var resize = new ResizeConfig();
            config.ResizeConfigs.Add("resize", resize);
            validate.Should().NotThrow();
            resize.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.ResizeConfigs.Clear();
            validate.Should().NotThrow();

            var batchresize = new BatchResizeConfig();
            config.BatchResizeConfigs.Add("batchresize", batchresize);
            validate.Should().NotThrow();
            batchresize.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.BatchResizeConfigs.Clear();
            validate.Should().NotThrow();

            var import = new ImportConfig();
            config.ImportConfigs.Add("import", import);
            validate.Should().Throw<InvalidDataException>();
            import.DestRoot = "dummy";
            validate.Should().NotThrow();
            config.ImportConfigs.Clear();
            validate.Should().NotThrow();
        }
    }
}