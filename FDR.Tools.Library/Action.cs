using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FDR.Tools.Library
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        rename,
        move,
        resize,
        hash,
        rehash,
        cleanup
    }

    public sealed class Action : ConfigPartBase
    {
        public Action() { }

        public Action(AppConfig? appConfig) : base(appConfig) { }

        public ActionType Type { get; set; }

        public string? Config { get; set; }

        public void Do(DirectoryInfo folder)
        {
            Validate();

            switch (Type)
            {
                case ActionType.rename:
                    BatchRenameConfig? batchRenameConfig;
                    if (AppConfig == null || !AppConfig.BatchRenameConfigs.TryGetValue(Config??"", out batchRenameConfig)) throw new ArgumentOutOfRangeException();
                    Rename.RenameFilesInFolder(folder, batchRenameConfig);
                    break;

                case ActionType.move:
                    MoveConfig? moveConfig;
                    if (AppConfig == null || !AppConfig.MoveConfigs.TryGetValue(Config??"", out moveConfig)) throw new ArgumentOutOfRangeException();
                    Import.MoveFilesInFolder(folder, moveConfig);
                    break;

                case ActionType.resize:
                    BatchResizeConfig? batchResizeConfig;
                    if (AppConfig == null || !AppConfig.BatchResizeConfigs.TryGetValue(Config??"", out batchResizeConfig)) throw new ArgumentOutOfRangeException();
                    Resize.ResizeFilesInFolder(folder, batchResizeConfig);
                    break;

                case ActionType.hash:
                    Verify.HashFolder(folder);
                    break;

                case ActionType.rehash:
                    Verify.HashFolder(folder, true);
                    break;

                case ActionType.cleanup:
                    Raw.CleanupFolder(folder);
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
                    BatchRenameConfig? batchRenameConfig;
                    if (!AppConfig.BatchRenameConfigs.TryGetValue(Config, out batchRenameConfig)) throw new ArgumentOutOfRangeException();
                    break;

                case ActionType.move:
                    if (AppConfig == null) throw new InvalidDataException("Application config cannot be empty!");
                    if (string.IsNullOrWhiteSpace(Config)) throw new InvalidDataException("ActionConfig cannot be empty!");
                    MoveConfig? moveConfig;
                    if (!AppConfig.MoveConfigs.TryGetValue(Config, out moveConfig)) throw new ArgumentOutOfRangeException();
                    break;

                case ActionType.resize:
                    if (AppConfig == null) throw new InvalidDataException("Application config cannot be empty!");
                    if (string.IsNullOrWhiteSpace(Config)) throw new InvalidDataException("ActionConfig cannot be empty!");
                    BatchResizeConfig? batchResizeConfig;
                    if (!AppConfig.BatchResizeConfigs.TryGetValue(Config, out batchResizeConfig)) throw new ArgumentOutOfRangeException();
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
