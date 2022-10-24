using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class RenameConfigTest
    {
        [Test]
        public void RenameConfigTests()
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();
            System.Action validate = () => config.Validate();

            config.FileFilter.Should().NotBeNullOrWhiteSpace("Default FileFilter");
            validate.Should().NotThrow();

            config.FileFilter = null;
            config.FileFilter.Should().NotBeNullOrWhiteSpace("FileFilter must have default");
            validate.Should().NotThrow();

            config.FileFilter = "*.CR3";
            config.FileFilter.Should().Be("*.CR3");
            validate.Should().NotThrow();

            config.FilenamePattern.Should().NotBeNullOrWhiteSpace("Default FilenamePattern");
            validate.Should().NotThrow();

            config.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>("FilenamePattern is empty");

            config.FilenamePattern = "{name}";
            validate.Should().NotThrow();

            config.AdditionalFileTypes.Clear();
            validate.Should().NotThrow();

            config.AdditionalFileTypes.Clear();
            config.AdditionalFileTypes.Add("");
            validate.Should().Throw<InvalidDataException>("Empty item");

            config.AdditionalFileTypes.Clear();
            config.AdditionalFileTypes.Add(" *. ");
            validate.Should().Throw<InvalidDataException>("Doesn't contain extension");

            config.AdditionalFileTypes.Clear();
            config.AdditionalFileTypes.Add("JPG");
            config.AdditionalFileTypes.Add(" *.PNG ");
            validate.Should().NotThrow();
        }
    }
}