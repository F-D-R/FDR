using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Text;
using System;

namespace FDR.Tools.Library
{
    public interface IValidatable
    {
        public void Validate();
    }

    public sealed class AppConfig : IValidatable
    {
        public static AppConfig Load(string? configPath = null)
        {
            if (!string.IsNullOrWhiteSpace(configPath))
            {
                if (!File.Exists(configPath)) throw new FileNotFoundException($"Config file not found! ({configPath})");
            }
            else
            {
                var appPath = Assembly.GetExecutingAssembly().Location;
                configPath = Path.Combine(Path.GetDirectoryName(appPath)!, "appsettings.json");
            }
            var appConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(configPath, Encoding.UTF8));
            appConfig ??= new AppConfig();
            appConfig.Validate();
            return appConfig;
        }

        public static void SaveToFile(AppConfig appConfig, string configPath)
        {
            if (appConfig == null) throw new ArgumentNullException(nameof(appConfig));
            if (string.IsNullOrWhiteSpace(configPath)) throw new ArgumentNullException(nameof(configPath));

            File.WriteAllText(configPath, JsonConvert.SerializeObject(appConfig), Encoding.UTF8);
        }

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

        [JsonIgnore]
        public virtual AppConfig? AppConfig { get; internal set; }

        public virtual void Validate() { }
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
