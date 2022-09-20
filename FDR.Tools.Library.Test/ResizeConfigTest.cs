using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class ResizeConfigTest
    {
        [Test]
        public void ResizeConfigTests()
        {
            var config = new ResizeConfig();
            config.Should().NotBeNull();
            System.Action validate = () => config.Validate();

            config.FilenamePattern.Should().NotBeNullOrWhiteSpace();
            validate.Should().NotThrow();

            config.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();

            config.FilenamePattern = "{name}";
            validate.Should().NotThrow();

            config.MaxWidth = 0;
            validate.Should().Throw<InvalidDataException>();

            config.MaxWidth = 600;
            validate.Should().NotThrow();

            config.MaxHeight = 0;
            validate.Should().Throw<InvalidDataException>();

            config.MaxHeight = 600;
            validate.Should().NotThrow();
        }

        [Test]
        public void BatchResizeConfigTests()
        {
            var config = new BatchResizeConfig();
            config.Should().NotBeNull();
            System.Action validate = () => config.Validate();

            config.FilenamePattern.Should().NotBeNullOrWhiteSpace();
            validate.Should().NotThrow();

            config.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();

            config.FilenamePattern = "{name}";
            validate.Should().NotThrow();

            config.MaxWidth = 0;
            validate.Should().Throw<InvalidDataException>();

            config.MaxWidth = 600;
            validate.Should().NotThrow();

            config.MaxHeight = 0;
            validate.Should().Throw<InvalidDataException>();

            config.MaxHeight = 600;
            validate.Should().NotThrow();

            config.FileFilter = null;
            config.FileFilter.Should().NotBeNullOrWhiteSpace();

            config.FileFilter = "*.JPG";
            config.FileFilter.Should().Be("*.JPG");
        }
    }
}