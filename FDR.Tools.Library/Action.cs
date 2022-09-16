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
                    if (AppConfig.BatchRenameConfigs.TryGetValue(ActionConfig, out batchRenameConfig))
                        Rename.RenameFilesInFolder(folder, batchRenameConfig);
                    break;

                case ActionType.move:
                    throw new NotImplementedException();

                case ActionType.resize:
                    BatchResizeConfig? batchResizeConfig;
                    if (AppConfig.BatchResizeConfigs.TryGetValue(ActionConfig, out batchResizeConfig))
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

            if (AppConfig == null) throw new InvalidDataException("Application config cannot be empty!");
            if (string.IsNullOrWhiteSpace(ActionConfig)) throw new InvalidDataException("ActionConfig cannot be empty!");
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
