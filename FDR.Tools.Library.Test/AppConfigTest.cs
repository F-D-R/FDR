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
            rename.FileFilter = "*.jpg";
            rename.FilenamePattern = "{name}";
            rename.FilenameCase = CharacterCasing.unchanged;
            rename.ExtensionCase = CharacterCasing.lower;
            config.RenameConfigs.Add("rename", rename);
            config.RenameConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            rename.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>("Rename config's FilenamePattern is empty");
            config.RenameConfigs.Clear();
            config.RenameConfigs.Should().HaveCount(0);
            validate.Should().NotThrow();

            config.ResizeConfigs.Should().HaveCount(0);
            var resize = new ResizeConfig();
            resize.Should().NotBeNull();
            resize.FileFilter = "*.jpg";
            resize.FilenamePattern = "{name}";
            resize.FilenameCase = CharacterCasing.unchanged;
            resize.ExtensionCase = CharacterCasing.lower;
            resize.ResizeMethod = ResizeMethod.fit_in;
            resize.MaxWidth = 100;
            resize.MaxHeight = 100;
            config.ResizeConfigs.Add("resize", resize);
            config.ResizeConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            resize.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>("Resize config's FilenamePattern is empty");
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