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
            var batchrename = new RenameConfig();
            batchrename.Should().NotBeNull();
            batchrename.FilenamePattern = "{name}";
            batchrename.FilenameCase = CharacterCasing.unchanged;
            batchrename.ExtensionCase = CharacterCasing.lower;
            batchrename.FileFilter = "*.jpg";
            config.RenameConfigs.Add("rename", batchrename);
            config.RenameConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            batchrename.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.RenameConfigs.Clear();
            config.RenameConfigs.Should().HaveCount(0);
            validate.Should().NotThrow();

            config.ResizeConfigs.Should().HaveCount(0);
            var batchresize = new ResizeConfig();
            batchresize.Should().NotBeNull();
            batchresize.FilenamePattern = "{name}";
            batchresize.FilenameCase = CharacterCasing.unchanged;
            batchresize.ExtensionCase = CharacterCasing.lower;
            batchresize.ResizeMethod = ResizeMethod.fit_in;
            batchresize.MaxWidth = 100;
            batchresize.MaxHeight = 100;
            batchresize.FileFilter = "*.jpg";
            config.ResizeConfigs.Add("resize", batchresize);
            config.ResizeConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            batchresize.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();
            config.ResizeConfigs.Clear();
            config.ResizeConfigs.Should().HaveCount(0);
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