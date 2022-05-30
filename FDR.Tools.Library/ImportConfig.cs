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
    public enum RuleType
    {
        contains_file,
        contains_folder,
        volume_label
    }

    public class Rule
    {
        public RuleType Type { get; set; }

        public string Param { get; set; }

        public void Validate()
        {

        }
    }

    public class ImportConfig
    {
        private const string DEFAULT_FILTER = "*.CR3|*.CR2|*.CRW|*.JPG|*.MP4|*.AVI|*.MOV";

        public string Name { get; set; }

        public Rule[] Rules { get; set; }

        public string DestRoot { get; set; }

        public FolderStructure DestStructure { get; set; } = FolderStructure.year_date;

        public string DateFormat { get; set; } = "yyMMdd";

        private string filter;
        public string FileFilter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        public RenameConfig[] RenameConfigs { get; set; }

        public MoveConfig[] MoveConfigs { get; set; }

        public void Validate()
        {

            //if (Rules == null) throw new InvalidDataException("Rules cannot be empty!");
            if (Rules != null) foreach (var r in Rules) { r.Validate(); };

            if (RenameConfigs != null) foreach (var rc in RenameConfigs) { rc.Validate(); };
            if (MoveConfigs != null) foreach (var mc in MoveConfigs) { mc.Validate(); };
        }

    }
}
