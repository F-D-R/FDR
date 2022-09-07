﻿using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    public class RenameConfigTest : TestFixtureBase
    {
        [Test]
        public void RenameConfigTests()
        {
            var config = new RenameConfig();
            Action validate = () => config.Validate();

            config.FilenamePattern.Should().NotBeNullOrWhiteSpace();
            validate.Should().NotThrow();

            config.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();

            config.FilenamePattern = "{name}";
            validate.Should().NotThrow();
        }

        [Test]
        public void BatchRenameConfigTests()
        {
            var config = new BatchRenameConfig();
            Action validate = () => config.Validate();

            config.FilenamePattern.Should().NotBeNullOrWhiteSpace();
            validate.Should().NotThrow();

            config.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();

            config.FilenamePattern = "{name}";
            validate.Should().NotThrow();

            config.Filter = null;
            config.Filter.Should().NotBeNullOrWhiteSpace();

            config.Filter = "*.CR3";
            config.Filter.Should().Be("*.CR3");

            config.FilenamePattern = null;
            validate.Should().Throw<InvalidDataException>();

            config.FilenamePattern = "dummy";
            validate.Should().NotThrow();

            // Empty list
            config.AdditionalFileTypes.Clear();
            validate.Should().NotThrow();

            // Empty item
            config.AdditionalFileTypes.Clear();
            config.AdditionalFileTypes.Add("");
            validate.Should().Throw<InvalidDataException>();

            // Trailing whitespace
            config.AdditionalFileTypes.Clear();
            config.AdditionalFileTypes.Add(".JPG ");
            validate.Should().Throw<InvalidDataException>();

            // Missing dot
            config.AdditionalFileTypes.Clear();
            config.AdditionalFileTypes.Add("JPG");
            validate.Should().Throw<InvalidDataException>();

            // Valid
            config.AdditionalFileTypes.Clear();
            config.AdditionalFileTypes.Add(".JPG");
            config.AdditionalFileTypes.Add(".PNG");
            validate.Should().NotThrow();
        }
    }
}