using System.Linq;
using System.Collections.Generic;

namespace FDR.Tools.Library
{
    public interface IValidatable
    {
        public void Validate();
    }

    public sealed class AppConfig : IValidatable
    {
        public AppConfig()
        {
            RenameConfigs = new RenameConfigs(this);
            ResizeConfigs = new ResizeConfigs(this);
            MoveConfigs = new Dictionary<string, MoveConfig>();
            ImportConfigs = new ImportConfigs(this);
        }

        public RenameConfigs RenameConfigs { get; }

        public ResizeConfigs ResizeConfigs { get; }

        public Dictionary<string, MoveConfig> MoveConfigs { get; }

        public ImportConfigs ImportConfigs { get; }

        public void Validate()
        {
            RenameConfigs?.ToList().ForEach(rnc => rnc.Value.Validate());
            ResizeConfigs?.ToList().ForEach(rsc => rsc.Value.Validate());
            MoveConfigs?.ToList().ForEach(mc => mc.Value.Validate());
            ImportConfigs.Where(ic => ic.Value?.AppConfig == null).ToList().ForEach(ic => ic.Value.AppConfig = this);
            ImportConfigs.ToList().ForEach(ic => ic.Value.Validate());
        }
    }

    public abstract class ConfigPartBase : IValidatable
    {
        public ConfigPartBase() { }

        public ConfigPartBase(AppConfig? appConfig) => AppConfig = appConfig;

        public virtual AppConfig? AppConfig { get; internal set; }

        public virtual void Validate()
        {
        }
    }

    public abstract class ConfigDictionaryBase<T> : Dictionary<string, T>
        where T : ConfigPartBase
    {
        private AppConfig? appConfig;

        public ConfigDictionaryBase(AppConfig appConfig) => this.appConfig = appConfig;

        public AppConfig? AppConfig
        {
            get { return appConfig; }
            internal set { appConfig = value; }
        }

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
            if (appConfig != null) value.AppConfig = appConfig;
            base.Add(value);
        }
    }
}
