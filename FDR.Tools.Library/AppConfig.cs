using System.Linq;
using System.Collections.Generic;
using FDR.Tools.Library;

namespace FDR
{
    public sealed class AppConfig
    {
        public AppConfig()
        {
            RenameConfigs = new RenameConfigs(this);
            BatchRenameConfigs = new BatchRenameConfigs(this);
            ResizeConfigs = new ResizeConfigs(this);
            BatchResizeConfigs = new BatchResizeConfigs(this);
            MoveConfigs = new MoveConfigs(this);
            ImportConfigs = new ImportConfigs(this);
        }

        public RenameConfigs RenameConfigs { get; }

        public ResizeConfigs ResizeConfigs { get; }

        public BatchRenameConfigs BatchRenameConfigs { get; }

        public BatchResizeConfigs BatchResizeConfigs { get; }

        public MoveConfigs MoveConfigs { get; }

        public ImportConfigs ImportConfigs { get; }

        public void Validate()
        {
            RenameConfigs?.ToList().ForEach(rnc => rnc.Value.Validate());
            ResizeConfigs?.ToList().ForEach(rsc => rsc.Value.Validate());
            BatchRenameConfigs?.ToList().ForEach(brnc => brnc.Value.Validate());
            BatchResizeConfigs?.ToList().ForEach(brsc => brsc.Value.Validate());
            MoveConfigs?.ToList().ForEach(mc => mc.Value.Validate());
            ImportConfigs?.ToList().ForEach(ic => ic.Value.Validate());
        }
    }

    public abstract class ConfigPartBase
    {
        public virtual AppConfig? AppConfig { get; internal set; }

        public virtual void Validate()
        {
        }
    }

    public abstract class ConfigDictionaryBase<T> : Dictionary<string, T>
        where T : ConfigPartBase
    {
        private AppConfig appConfig;

        public ConfigDictionaryBase(AppConfig appConfig) => this.appConfig = appConfig;

        public new void Add(string key, T value)
        {
            value.AppConfig = appConfig;
            base.Add(key, value);
        }
    }

    public abstract class ConfigListBase<T> : List<T>
        where T : ConfigPartBase
    {
        private AppConfig? appConfig;

        public ConfigListBase(AppConfig? appConfig) => this.appConfig = appConfig;

        internal virtual AppConfig? AppConfig { get; set; }

        public new void Add(T value)
        {
            value.AppConfig = appConfig;
            base.Add(value);
        }
    }
}
