using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    [TestFixture]
    public class ActionConfigTest
    {
        [Test]
        public void ActionClassTests()
        {
            var appConfig = new AppConfig();
            appConfig.Should().NotBeNull();
            var config = new Action(appConfig);
            config.Should().NotBeNull();
            System.Action validate = () => config.Validate();

            config.Type = ActionType.hash;
            config.Config = null;
            validate.Should().NotThrow();

            config.Type = ActionType.rehash;
            config.Config = null;
            validate.Should().NotThrow();

            config.Type = (ActionType)9999;
            config.Config = null;
            validate.Should().Throw<NotImplementedException>("Invalid action type");

            config.Type = ActionType.rename;
            config.Config = null;
            validate.Should().Throw<InvalidDataException>("Missing rename config name");
            config.Type = ActionType.rename;
            config.Config = "dummy";
            validate.Should().Throw<ArgumentOutOfRangeException>("Missing rename config");
            appConfig.RenameConfigs.Add("dummy", new RenameConfig());
            appConfig.RenameConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();

            config.Type = ActionType.resize;
            config.Config = null;
            validate.Should().Throw<InvalidDataException>("Missing resize config name");
            config.Type = ActionType.resize;
            config.Config = "dummy";
            validate.Should().Throw<ArgumentOutOfRangeException>("Missing resize config");
            appConfig.ResizeConfigs.Add("dummy", new ResizeConfig());
            appConfig.ResizeConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();

            config.Type = ActionType.move;
            config.Config = null;
            validate.Should().Throw<InvalidDataException>("Missing move config name");
            config.Type = ActionType.move;
            config.Config = "dummy";
            validate.Should().Throw<ArgumentOutOfRangeException>("Missing move config");
            appConfig.MoveConfigs.Add("dummy", new MoveConfig());
            appConfig.MoveConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();

            config = new Action();     // without AppConfig
            config.Should().NotBeNull();
            validate = () => config.Validate();

            config.Type = ActionType.hash;
            validate.Should().NotThrow();

            config.Type = ActionType.rehash;
            validate.Should().NotThrow();

            config.Type = ActionType.rename;
            config.Config = "dummy";
            validate.Should().Throw<InvalidDataException>("Missing rename config");

            config.Type = ActionType.resize;
            config.Config = "dummy";
            validate.Should().Throw<InvalidDataException>("Missing resize config");

            config.Type = ActionType.move;
            config.Config = "dummy";
            validate.Should().Throw<InvalidDataException>("Missing move config");
        }

        [Test]
        public void ActionsTests()
        {
            var appConfig = new AppConfig();
            appConfig.Should().NotBeNull();
            System.Action validate = () => appConfig.Validate();
            validate.Should().NotThrow();

            var actions = new Actions(appConfig);
            actions.Should().NotBeNull();
            actions.Should().HaveCount(0);

            var a = new Action() { Type = ActionType.hash };
            a.Should().NotBeNull();
            actions.Add(a);
            a = new Action() { Type = ActionType.rehash };
            a.Should().NotBeNull();
            actions.Add(a);
            actions.Should().HaveCount(2);
            validate.Should().NotThrow();
        }
    }
}