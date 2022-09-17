using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace FDR.Tools.Library.Test
{
    public class ActionConfigTest : TestFixtureBase
    {
        [Test]
        public void ActionClassTests()
        {
            var appConfig = new AppConfig();
            appConfig.Should().NotBeNull();
            var config = new ActionClass(appConfig);
            config.Should().NotBeNull();
            Action validate = () => config.Validate();

            config.ActionType = ActionType.hash;
            config.ActionConfig = null;
            validate.Should().NotThrow();

            config.ActionType = ActionType.rehash;
            config.ActionConfig = null;
            validate.Should().NotThrow();

            config.ActionType = (ActionType)9999;
            config.ActionConfig = null;
            validate.Should().Throw<NotImplementedException>();

            config.ActionType = ActionType.rename;
            config.ActionConfig = null;
            validate.Should().Throw<InvalidDataException>();
            config.ActionType = ActionType.rename;
            config.ActionConfig = "dummy";
            validate.Should().Throw<ArgumentOutOfRangeException>();
            appConfig.BatchRenameConfigs.Add("dummy", new BatchRenameConfig());
            appConfig.BatchRenameConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();

            config.ActionType = ActionType.resize;
            config.ActionConfig = null;
            validate.Should().Throw<InvalidDataException>();
            config.ActionType = ActionType.resize;
            config.ActionConfig = "dummy";
            validate.Should().Throw<ArgumentOutOfRangeException>();
            appConfig.BatchResizeConfigs.Add("dummy", new BatchResizeConfig());
            appConfig.BatchResizeConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();

            config.ActionType = ActionType.move;
            config.ActionConfig = null;
            validate.Should().Throw<InvalidDataException>();
            config.ActionType = ActionType.move;
            config.ActionConfig = "dummy";
            validate.Should().Throw<ArgumentOutOfRangeException>();
            appConfig.MoveConfigs.Add("dummy", new MoveConfig());
            appConfig.MoveConfigs.Should().HaveCount(1);
            validate.Should().NotThrow();

            config = new ActionClass();     // without AppConfig
            config.Should().NotBeNull();
            validate = () => config.Validate();

            config.ActionType = ActionType.hash;
            validate.Should().NotThrow();

            config.ActionType = ActionType.rehash;
            validate.Should().NotThrow();

            config.ActionType = ActionType.rename;
            config.ActionConfig = "dummy";
            validate.Should().Throw<InvalidDataException>();

            config.ActionType = ActionType.resize;
            config.ActionConfig = "dummy";
            validate.Should().Throw<InvalidDataException>();

            config.ActionType = ActionType.move;
            config.ActionConfig = "dummy";
            validate.Should().Throw<InvalidDataException>();
        }

        [Test]
        public void ActionsTests()
        {
            var appConfig = new AppConfig();
            appConfig.Should().NotBeNull();
            Action validate = () => appConfig.Validate();
            validate.Should().NotThrow();

            var actions = new Actions(appConfig);
            actions.Should().NotBeNull();
            actions.Should().HaveCount(0);

            var a = new ActionClass() { ActionType = ActionType.hash };
            a.Should().NotBeNull();
            actions.Add(a);
            actions.Should().HaveCount(1);
            validate.Should().NotThrow();
        }
    }
}