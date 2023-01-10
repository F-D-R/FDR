using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;

namespace FDR.Tools.Library
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FolderStructure
    {
        date,
        year_date,
        year_month_date,
        year_month_day,
        year_month
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ImportRuleType
    {
        contains_folder,
        contains_file,
        volume_label
    }

    public sealed class ImportRule : IValidatable
    {
        public ImportRuleType Type { get; set; }

        public string? Param { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Param)) throw new InvalidDataException("Rule parameter cannot be empty!");
            switch (Type)
            {
                case ImportRuleType.contains_folder:
                case ImportRuleType.contains_file:
                case ImportRuleType.volume_label:
                    break;
                default:
                    throw new InvalidDataException("Invalid Type!");
            }
        }
    }

    public sealed class ImportRules : List<ImportRule>
    {
        public bool Evaluate(DirectoryInfo source)
        {
            if (Count == 0) return false;

            foreach (var rule in this)
            {
                switch (rule.Type)
                {
                    case ImportRuleType.volume_label:
                        if (string.Compare(Import.GetVolumeLabel(source.FullName), rule.Param, true) != 0)
                            return false;
                        break;

                    case ImportRuleType.contains_file:
                        if (!Common.GetFiles(source, rule.Param??"*", true).Any())
                            return false;
                        break;

                    case ImportRuleType.contains_folder:
                        //TODO: Common.GetDirectories
                        if (!Directory.GetDirectories(source.FullName, rule.Param??"*", SearchOption.AllDirectories).Any())
                            return false;
                        break;

                    default:
                        throw new NotImplementedException($"Unknown rule type: {rule.Type}");
                }
            }
            return true;
        }
    }

    public sealed class ImportConfig : ConfigPartBase
    {
        public static Dictionary<string, string> GetImportConfigAttributeList()
        {
            var attributes = new Dictionary<string, string>();
            attributes.Add(nameof(Name), "Name of the import configuration. It helps choosing one during the import wizard.");
            attributes.Add(nameof(Rules), $"List of rules, which can determine the suitable import configuration automatically based on the memory card content. Each rule has a \"Type\" and a \"Param\" value. The type determines what kind of checks the rule will test within the DCIM folder of the memory card. Possible type values are: \"{nameof(ImportRuleType.contains_folder)}\", \"{nameof(ImportRuleType.contains_file)}\", \"{nameof(ImportRuleType.volume_label)}\". The param value is the name of the file, folder or volume label. Wildcards (*?) are supported for the file and folder names. A typical rule list looks like this: [{{\"{nameof(ImportRule.Type)}\":\"{nameof(ImportRuleType.contains_folder)}\", \"{nameof(ImportRule.Param)}\":\"CANONMSC\"}}].");
            attributes.Add(nameof(DestRoot), "The destination root folder. The import will build the configured folder structure within that.");
            attributes.Add(nameof(DestStructure), $"The type of the folder structure. There are one, two and three level deep structures. Possible values are: \"{nameof(FolderStructure.date)}\", \"{nameof(FolderStructure.year_date)}\", \"{nameof(FolderStructure.year_month)}\", \"{nameof(FolderStructure.year_month_date)}\". Default is \"{nameof(FolderStructure.year_date)}\".");
            attributes.Add(nameof(DateFormat), $"Format of the date part of the folder structure. Default is \"{DEFAULT_DATEFORMAT}\".");
            attributes.Add(nameof(FileFilter), "Filter condition for the files to import. Wildcards (*?) are supported. Multiple filters can be defined separated with pipes. Example: \"*.CR3|*.CR2\".");
            attributes.Add(nameof(Actions), $"Defines a list of actions, which will be executed in the given order after the import of the files. Each action has a \"Type\" and a \"Config\" value. The type determines what kind of action that is. Possible type values are: \"{nameof(ActionType.rename)}\", \"{nameof(ActionType.move)}\", \"{nameof(ActionType.resize)}\", \"{nameof(ActionType.hash)}\", \"{nameof(ActionType.rehash)}\", \"{nameof(ActionType.cleanup)}\". The config value is a named configuration from the appropriate section of the configuration file (appsettings.json). Some actions like the {nameof(ActionType.hash)}, {nameof(ActionType.rehash)} and {nameof(ActionType.cleanup)} don't need a config value. A typical action list looks like this: [{{\"{nameof(Action.Type)}\":\"{nameof(ActionType.rename)}\", \"{nameof(Action.Config)}\":\"some_renameconfig_name\"}}, {{\"{nameof(Action.Type)}\":\"{nameof(ActionType.move)}\", \"{nameof(Action.Config)}\":\"some_moveconfig_name\"}}].");
            return attributes;
        }

        private const string DEFAULT_FILTER = "*.CR3|*.CR2|*.CRW|*.DNG|*.JPG|*.MP4|*.AVI|*.MOV|*.SRT";
        private const string DEFAULT_DATEFORMAT = "yyMMdd";

        public string? Name { get; set; }

        public ImportRules Rules { get; } = new ImportRules();

        public string? DestRoot { get; set; }

        public FolderStructure DestStructure { get; set; } = FolderStructure.year_date;

        public string DateFormat { get; set; } = DEFAULT_DATEFORMAT;

        private string? filter;
        public string FileFilter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        public Actions Actions { get; } = new Actions(null);

        public override AppConfig? AppConfig
        {
            get => base.AppConfig;
            internal set
            {
                base.AppConfig = value;
                Actions.AppConfig = value;
            }
        }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(DestRoot)) throw new InvalidDataException("Destination root cannot be empty!");
            if (string.IsNullOrWhiteSpace(DateFormat)) throw new InvalidDataException("Date format cannot be empty!");
            Rules.ForEach(r => r.Validate());
            Actions.AppConfig = AppConfig;
            Actions.ForEach(a => a.Validate());
            switch (DestStructure)
            {
                case FolderStructure.date:
                case FolderStructure.year_date:
                case FolderStructure.year_month:
                case FolderStructure.year_month_date:
                case FolderStructure.year_month_day:
                    break;
                default:
                    throw new InvalidDataException("Invalid DestStructure!");
            }
        }
    }

    public sealed class ImportConfigs : ConfigDictionaryBase<ImportConfig>
    {
        public ImportConfigs(AppConfig appConfig) : base(appConfig) { }
    }
}
