using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    public class ImportConfigTest : TestFixtureBase
    {
        [Test]
        public void ImportRuleTests()
        {
            var rule = new ImportRule();
            Action validate = () => rule.Validate();

            rule.Param = null;
            validate.Should().Throw<InvalidDataException>();

            rule.Param = "dummy";
            validate.Should().NotThrow();
        }

        [Test]
        public void ImportConfigTests()
        {
            var config = new ImportConfig();
            Action validate = () => config.Validate();

            config.DestRoot = null;
            validate.Should().Throw<InvalidDataException>();

            config.DestRoot = "dummy";
            validate.Should().NotThrow();

            config.DateFormat = null;
            validate.Should().Throw<InvalidDataException>();

            config.DateFormat = "yyMMdd";
            validate.Should().NotThrow();

            config.FileFilter = null;
            config.FileFilter.Should().NotBeNullOrWhiteSpace();

            config.Rules.Add(null);
            validate.Should().Throw<NullReferenceException>();

            config.Rules.Clear();
            config.Rules.Add(new ImportRule() { Param = null });
            validate.Should().Throw<InvalidDataException>();

            config.Rules.Clear();
            config.Rules.Add(new ImportRule() { Param = "dummy" });
            validate.Should().NotThrow();

            config.BatchRenameConfigs.Add(null);
            validate.Should().Throw<NullReferenceException>();

            config.BatchRenameConfigs.Clear();
            var brc = new BatchRenameConfig();
            config.BatchRenameConfigs.Add(brc);
            validate.Should().NotThrow();

            brc.AdditionalFileTypes.Add("");
            validate.Should().Throw<InvalidDataException>();

            config.BatchRenameConfigs.Clear();
            validate.Should().NotThrow();

            config.MoveConfigs.Clear();
            var mc = new MoveConfig();
            config.MoveConfigs.Add(mc);
            validate.Should().NotThrow();

            mc.Filter = "";
            validate.Should().Throw<InvalidDataException>();

            mc.Filter = "*.CR3";
            validate.Should().NotThrow();

            mc.RelativeFolder = "";
            validate.Should().Throw<InvalidDataException>();

            mc.RelativeFolder = "RAW";
            validate.Should().NotThrow();

            config.MoveConfigs.Clear();
            validate.Should().NotThrow();
        }
    }
}