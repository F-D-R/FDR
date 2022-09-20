using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class MoveConfigTest
    {
        [Test]
        public void MoveConfigTests()
        {
            var config = new MoveConfig();
            config.Should().NotBeNull();
            System.Action validate = () => config.Validate();

            validate.Should().NotThrow();

            config.FileFilter = null;
            validate.Should().Throw<InvalidDataException>();

            config.FileFilter = "*.CR3";
            validate.Should().NotThrow();

            config.RelativeFolder = null;
            validate.Should().Throw<InvalidDataException>();

            config.RelativeFolder = "RAW";
            validate.Should().NotThrow();
        }
   }
}