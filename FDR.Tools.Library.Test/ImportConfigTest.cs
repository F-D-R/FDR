using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class ImportConfigTest
    {
        [Test]
        public void ImportRuleTests()
        {
            var rule = new ImportRule();
            rule.Should().NotBeNull();
            System.Action validate = () => rule.Validate();

            rule.Param = null;
            validate.Should().Throw<InvalidDataException>("Invalid Param");
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
            System.Action validate = () => config.Validate();

            config.DestRoot = null;
            validate.Should().Throw<InvalidDataException>("Invalid DestRoot");
            config.DestRoot = "dummy";
            validate.Should().NotThrow();

            config.DateFormat = null;
            validate.Should().Throw<InvalidDataException>("Invalid DateFormat");
            config.DateFormat = "yyMMdd";
            validate.Should().NotThrow();

            config.FileFilter = null;
            config.FileFilter.Should().NotBeNullOrWhiteSpace();

            config.Rules.Should().HaveCount(0);
            config.Rules.Add(null);
            config.Rules.Should().HaveCount(1);
            validate.Should().Throw<NullReferenceException>("Rules with null item");
            config.Rules.Clear();
            config.Rules.Should().HaveCount(0);
            config.Rules.Add(new ImportRule() { Param = null });
            config.Rules.Should().HaveCount(1);
            validate.Should().Throw<InvalidDataException>("Invalid Rule");
            config.Rules.Clear();
            config.Rules.Should().HaveCount(0);
            config.Rules.Add(new ImportRule() { Param = "dummy" });
            config.Rules.Should().HaveCount(1);
            validate.Should().NotThrow();

            appConfig.ImportConfigs.Add("test", config);
            config.Actions.Should().HaveCount(0);
            var a = new Action(appConfig) { Type = ActionType.hash };
            a.Should().NotBeNull();
            config.Actions.Add(a);
            config.Actions.Should().HaveCount(1);
            validate.Should().NotThrow();
            a.Type = ActionType.rehash;
            validate.Should().NotThrow();
            a.Type = ActionType.rename;
            a.Config = null;
            validate.Should().Throw<InvalidDataException>("Rename action without ActionConfig");
            a.Type = ActionType.rename;
            a.Config = "rename";
            validate.Should().Throw<ArgumentOutOfRangeException>("Rename action with invalid ActionConfig");
            appConfig.BatchRenameConfigs.Add("rename", new BatchRenameConfig());
            appConfig.BatchRenameConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            a.Type = ActionType.resize;
            a.Config = null;
            validate.Should().Throw<InvalidDataException>("Resize action without ActionConfig");
            a.Type = ActionType.resize;
            a.Config = "resize";
            validate.Should().Throw<ArgumentOutOfRangeException>("Resize action with invalid ActionConfig");
            appConfig.BatchResizeConfigs.Add("resize", new BatchResizeConfig());
            appConfig.BatchResizeConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            a.Type = ActionType.move;
            a.Config = null;
            validate.Should().Throw<InvalidDataException>("Move action without ActionConfig");
            a.Type = ActionType.move;
            a.Config = "move";
            validate.Should().Throw<ArgumentOutOfRangeException>("Move action with invalid ActionConfig");
            appConfig.MoveConfigs.Add("move", new MoveConfig());
            appConfig.MoveConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();
            config.Actions.Clear();
            config.Actions.Should().HaveCount(0);
            validate.Should().NotThrow();
        }
    }
}