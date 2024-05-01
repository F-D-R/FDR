using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FDR.Tools.Library
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        [Display(Name = "Rename")]
        rename,
        [Display(Name = "Move")]
        move,
        [Display(Name = "Resize")]
        resize,
        [Display(Name = "Hash")]
        hash,
        [Display(Name = "Rehash")]
        rehash,
        [Display(Name = "Cleanup")]
        cleanup
    }

    public sealed class Action : ConfigPartBase
    {
        public Action() { }

        public Action(AppConfig? appConfig) : base(appConfig) { }

        public ActionType Type { get; set; }

        public string? Config { get; set; }

        public void Do(DirectoryInfo folder, CancellationToken token, List<ExifFile>? allFiles = null)
        {
            Validate();

            switch (Type)
            {
                case ActionType.rename:
                    RenameConfig? renameConfig;
                    if (AppConfig == null || !AppConfig.RenameConfigs.TryGetValue(Config??"", out renameConfig)) throw new ArgumentOutOfRangeException(nameof(Config));
                    Rename.RenameFilesInFolder(folder, renameConfig, allFiles);
                    break;

                case ActionType.move:
                    MoveConfig? moveConfig;
                    if (AppConfig == null || !AppConfig.MoveConfigs.TryGetValue(Config??"", out moveConfig)) throw new ArgumentOutOfRangeException(nameof(Config));
                    Import.MoveFilesInFolder(folder, moveConfig, allFiles);
                    break;

                case ActionType.resize:
                    ResizeConfig? resizeConfig;
                    if (AppConfig == null || !AppConfig.ResizeConfigs.TryGetValue(Config??"", out resizeConfig)) throw new ArgumentOutOfRangeException(nameof(Config));
                    Resize.ResizeFilesInFolder(folder, resizeConfig, allFiles);
                    break;

                case ActionType.hash:
                    Verify.HashFolder(folder, false, allFiles);
                    break;

                case ActionType.rehash:
                    Verify.HashFolder(folder, true, allFiles);
                    break;

                case ActionType.cleanup:
                    Raw.CleanupFolder(folder, token);
                    break;

                default:
                    break;
            }
        }

        public override void Validate()
        {
            base.Validate();

            switch (Type)
            {
                case ActionType.rename:
                    if (AppConfig == null) throw new InvalidDataException("Application config cannot be empty!");
                    if (string.IsNullOrWhiteSpace(Config)) throw new InvalidDataException("ActionConfig cannot be empty!");
                    if (!AppConfig.RenameConfigs.TryGetValue(Config, out _)) throw new ArgumentOutOfRangeException();
                    break;

                case ActionType.move:
                    if (AppConfig == null) throw new InvalidDataException("Application config cannot be empty!");
                    if (string.IsNullOrWhiteSpace(Config)) throw new InvalidDataException("ActionConfig cannot be empty!");
                    if (!AppConfig.MoveConfigs.TryGetValue(Config, out _)) throw new ArgumentOutOfRangeException();
                    break;

                case ActionType.resize:
                    if (AppConfig == null) throw new InvalidDataException("Application config cannot be empty!");
                    if (string.IsNullOrWhiteSpace(Config)) throw new InvalidDataException("ActionConfig cannot be empty!");
                    if (!AppConfig.ResizeConfigs.TryGetValue(Config, out _)) throw new ArgumentOutOfRangeException();
                    break;

                case ActionType.hash:
                case ActionType.rehash:
                case ActionType.cleanup:
                    //Nothing to validate
                    break;

                default:
                    throw new NotImplementedException("Invalid ActionType: " + Type.ToString());
            }
        }
    }

    public sealed class Actions : ConfigListBase<Action>
    {
        public Actions(AppConfig? appConfig) : base(appConfig) { }

        internal override AppConfig? AppConfig
        {
            get => base.AppConfig;
            set
            {
                if (base.AppConfig == null && value != null)
                    this.ForEach(a => a.AppConfig = value);
                base.AppConfig = value;
            }
        }
    }
}
