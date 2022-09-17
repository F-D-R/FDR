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
            rule.Should().NotBeNull();
            Action validate = () => rule.Validate();

            rule.Param = null;
            validate.Should().Throw<InvalidDataException>();
            rule.Param = "dummy";
            validate.Should().NotThrow();
        }

        [Test]
        public void ImportConfigTests()
        {
            var appConfig = new AppConfig();
            appConfig.Should().NotBeNull();
            var config = new ImportConfig();
            config.Should().NotBeNull();
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

            config.Rules.Should().HaveCount(0);
            config.Rules.Add(null);
            config.Rules.Should().HaveCount(1);
            validate.Should().Throw<NullReferenceException>();
            config.Rules.Clear();
            config.Rules.Should().HaveCount(0);
            config.Rules.Add(new ImportRule() { Param = null });
            config.Rules.Should().HaveCount(1);
            validate.Should().Throw<InvalidDataException>();
            config.Rules.Clear();
            config.Rules.Should().HaveCount(0);
            config.Rules.Add(new ImportRule() { Param = "dummy" });
            config.Rules.Should().HaveCount(1);
            validate.Should().NotThrow();

            config.BatchRenameConfigs.Should().HaveCount(0);
            config.BatchRenameConfigs.Add(null);
            config.BatchRenameConfigs.Should().HaveCount(1);
            validate.Should().Throw<NullReferenceException>();
            config.BatchRenameConfigs.Clear();
            config.BatchRenameConfigs.Should().HaveCount(0);
            var brc = new BatchRenameConfig();
            brc.Should().NotBeNull();
            config.BatchRenameConfigs.Add(brc);
            config.BatchRenameConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            brc.AdditionalFileTypes.Add("");
            validate.Should().Throw<InvalidDataException>();
            config.BatchRenameConfigs.Clear();
            config.BatchRenameConfigs.Should().HaveCount(0);
            validate.Should().NotThrow();

            config.MoveConfigs.Should().HaveCount(0);
            var mc = new MoveConfig();
            mc.Should().NotBeNull();
            config.MoveConfigs.Add(mc);
            config.MoveConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            mc.FileFilter = "";
            validate.Should().Throw<InvalidDataException>();
            mc.FileFilter = "*.CR3";
            validate.Should().NotThrow();
            mc.RelativeFolder = "";
            validate.Should().Throw<InvalidDataException>();
            mc.RelativeFolder = "RAW";
            validate.Should().NotThrow();
            config.MoveConfigs.Clear();
            config.MoveConfigs.Should().HaveCount(0);
            validate.Should().NotThrow();

            appConfig.ImportConfigs.Add("test", config);
            config.Actions.Should().HaveCount(0);
            var a = new ActionClass() { ActionType = ActionType.hash };
            //var a = new ActionClass(appConfig) { ActionType = ActionType.hash };
            a.Should().NotBeNull();
            config.Actions.Add(a);
            config.Actions.Should().HaveCount(1);
            validate.Should().NotThrow();
            //a.ActionType = ActionType.rename;
            //a.ActionConfig = null;
            //validate.Should().Throw<InvalidDataException>();
            a.ActionConfig = "dummy";
            validate.Should().NotThrow();
            config.Actions.Clear();
            config.Actions.Should().HaveCount(0);
            validate.Should().NotThrow();
        }
    }
}