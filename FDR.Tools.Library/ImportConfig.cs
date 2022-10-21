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
        private const string DEFAULT_FILTER = "*.CR3|*.CR2|*.CRW|*.JPG|*.MP4|*.AVI|*.MOV";

        public string? Name { get; set; }

        public ImportRules Rules { get; } = new ImportRules();

        public string? DestRoot { get; set; }

        public FolderStructure DestStructure { get; set; } = FolderStructure.year_date;

        public string DateFormat { get; set; } = "yyMMdd";

        private string? filter;
        public string FileFilter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        //public List<BatchRenameConfig> BatchRenameConfigs { get; } = new List<BatchRenameConfig>();

        //public List<MoveConfig> MoveConfigs { get; } = new List<MoveConfig>();

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
            //BatchRenameConfigs.ForEach(rc => rc.Validate());
            //MoveConfigs.ForEach(mc => mc.Validate());
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
