using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

    public sealed class ImportRule
    {
        public ImportRuleType Type { get; set; } = ImportRuleType.contains_folder;

        public string? Param { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Param)) throw new InvalidDataException("Rule parameter cannot be empty!");
        }
    }

    public sealed class ImportConfig
    {
        private const string DEFAULT_FILTER = "*.CR3|*.CR2|*.CRW|*.JPG|*.MP4|*.AVI|*.MOV";

        public string? Name { get; set; }

        public List<ImportRule> Rules { get; } = new List<ImportRule>();

        public string? DestRoot { get; set; }

        public FolderStructure DestStructure { get; set; } = FolderStructure.year_date;

        public string DateFormat { get; set; } = "yyMMdd";

        private string? filter;
        public string FileFilter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        public List<BatchRenameConfig> BatchRenameConfigs { get; } = new List<BatchRenameConfig>();

        public List<MoveConfig> MoveConfigs { get; } = new List<MoveConfig>();

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(DestRoot)) throw new InvalidDataException("Destination root cannot be empty!");
            if (string.IsNullOrWhiteSpace(DateFormat)) throw new InvalidDataException("Date format cannot be empty!");
            Rules.ForEach(r => r.Validate());
            BatchRenameConfigs.ForEach(rc => rc.Validate());
            MoveConfigs.ForEach(mc => mc.Validate());
        }

    }
}
