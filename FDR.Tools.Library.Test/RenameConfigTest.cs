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
        }

        [TestCase("{name}", false)]
        [TestCase("anything", false)]
        [TestCase("counter", false)]
        [TestCase("...{counter}...", true)]
        [TestCase("...{CoUnTeR}...", true)]
        [TestCase("...{counter:3}...", true)]
        [TestCase("...{COUNTER:auto}...", true)]
        public void NeedsOrderingTests(string filenamePatter, bool result)
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();

            config.FilenamePattern = filenamePatter;
            config.NeedsOrdering().Should().Be(result, config.FilenamePattern);
        }

        [TestCase("{name}", false)]
        [TestCase("anything", false)]
        [TestCase("edate", false)]
        [TestCase("sdate", false)]
        [TestCase("...{edate}...", true)]
        [TestCase("...{sdate}...", true)]
        [TestCase("...{EdAtE}...", true)]
        [TestCase("...{SdAtE}...", true)]
        [TestCase("...{edate:yyMMdd}...", true)]
        [TestCase("...{EDATE:yyMMdd}...", true)]
        [TestCase("...{sdate:yyMMdd}...", true)]
        [TestCase("...{SDATE:yyMMdd}...", true)]
        public void HasExifDateTests(string filenamePatter, bool result)
        {
            var config = new RenameConfig();
            config.Should().NotBeNull();

            config.FilenamePattern = filenamePatter;
            config.HasExifDate().Should().Be(result, config.FilenamePattern);
        }

    }
}