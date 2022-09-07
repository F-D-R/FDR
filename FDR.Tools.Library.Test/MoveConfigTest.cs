using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    public class MoveConfigTest : TestFixtureBase
    {
        [Test]
        public void MoveConfigTests()
        {
            var config = new MoveConfig();
            Action validate = () => config.Validate();

            validate.Should().NotThrow();

            config.Filter = null;
            validate.Should().Throw<InvalidDataException>();

            config.Filter = "*.CR3";
            validate.Should().NotThrow();

            config.RelativeFolder = null;
            validate.Should().Throw<InvalidDataException>();

            config.RelativeFolder = "RAW";
            validate.Should().NotThrow();
        }
   }
}