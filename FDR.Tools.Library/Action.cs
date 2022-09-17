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
        rehash
    }

    public class ActionClass : ConfigPartBase
    {
        public ActionClass() { }

        public ActionClass(AppConfig? appConfig) : base(appConfig) { }

        public ActionType ActionType { get; set; }

        public string? ActionConfig { get; set; }

        public void Do(DirectoryInfo folder)
        {
            Validate();

            if (AppConfig == null || string.IsNullOrWhiteSpace(ActionConfig)) return;

            switch (ActionType)
            {
                case ActionType.rename:
                    BatchRenameConfig? batchRenameConfig;
                    if (!AppConfig.BatchRenameConfigs.TryGetValue(ActionConfig, out batchRenameConfig)) throw new ArgumentOutOfRangeException();
                    Rename.RenameFilesInFolder(folder, batchRenameConfig);
                    break;

                case ActionType.move:
                    MoveConfig? moveConfig;
                    if (!AppConfig.MoveConfigs.TryGetValue(ActionConfig, out moveConfig)) throw new ArgumentOutOfRangeException();
                    Import.MoveFilesInFolder(folder, moveConfig);
                    throw new NotImplementedException();

                case ActionType.resize:
                    BatchResizeConfig? batchResizeConfig;
                    if (!AppConfig.BatchResizeConfigs.TryGetValue(ActionConfig, out batchResizeConfig)) throw new ArgumentOutOfRangeException();
                    Resize.ResizeFilesInFolder(folder, batchResizeConfig);
                    break;

                case ActionType.hash:
                    Verify.HashFolder(folder);
                    break;

                case ActionType.rehash:
                    Verify.HashFolder(folder, true);
                    break;

                default:
                    break;
            }
        }

        public override void Validate()
        {
            base.Validate();

            switch (ActionType)
            {
                case ActionType.rename:
                    if (AppConfig == null) throw new InvalidDataException("Application config cannot be empty!");
                    if (string.IsNullOrWhiteSpace(ActionConfig)) throw new InvalidDataException("ActionConfig cannot be empty!");
                    BatchRenameConfig? batchRenameConfig;
                    if (!AppConfig.BatchRenameConfigs.TryGetValue(ActionConfig, out batchRenameConfig)) throw new ArgumentOutOfRangeException();
                    break;

                case ActionType.move:
                    if (AppConfig == null) throw new InvalidDataException("Application config cannot be empty!");
                    if (string.IsNullOrWhiteSpace(ActionConfig)) throw new InvalidDataException("ActionConfig cannot be empty!");
                    MoveConfig? moveConfig;
                    if (!AppConfig.MoveConfigs.TryGetValue(ActionConfig, out moveConfig)) throw new ArgumentOutOfRangeException();
                    break;

                case ActionType.resize:
                    if (AppConfig == null) throw new InvalidDataException("Application config cannot be empty!");
                    if (string.IsNullOrWhiteSpace(ActionConfig)) throw new InvalidDataException("ActionConfig cannot be empty!");
                    BatchResizeConfig? batchResizeConfig;
                    if (!AppConfig.BatchResizeConfigs.TryGetValue(ActionConfig, out batchResizeConfig)) throw new ArgumentOutOfRangeException();
                    break;

                case ActionType.hash:
                case ActionType.rehash:
                    //No 
                    break;

                default:
                    throw new NotImplementedException("Invalid ActionType: " + ActionType.ToString());
            }
        }
    }

    public class Actions : ConfigListBase<ActionClass>
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
