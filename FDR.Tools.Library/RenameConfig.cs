using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FDR.Tools.Library
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CharacterCasing
    {
        upper,
        lower,
        unchanged
    }

    public class RenameConfig : ConfigPartBase
    {
        private const string DEFAULT_FILTER = "*.*";

        public string? FilenamePattern { get; set; } = "{name}";

        public CharacterCasing FilenameCase { get; set; } = CharacterCasing.unchanged;

        public CharacterCasing ExtensionCase { get; set; } = CharacterCasing.lower;

        protected string? filter;
        public virtual string FileFilter
        {
            get { return !string.IsNullOrWhiteSpace(filter) ? filter : DEFAULT_FILTER; }
            set { filter = value; }
        }

        public List<string> AdditionalFileTypes { get; } = new List<string>();

        public bool Recursive { get; set; } = false;

        public bool StopOnError { get; set; } = true;

        public override void Validate()
        {
            base.Validate();
            if (string.IsNullOrWhiteSpace(FilenamePattern)) throw new InvalidDataException("Renaming filename pattern cannot be empty!");
            switch (FilenameCase)
            {
                case CharacterCasing.unchanged:
                case CharacterCasing.lower:
                case CharacterCasing.upper:
                    break;
                default:
                    throw new InvalidDataException("Invalid FilenameCase!");
            }
            switch (ExtensionCase)
            {
                case CharacterCasing.unchanged:
                case CharacterCasing.lower:
                case CharacterCasing.upper:
                    break;
                default:
                    throw new InvalidDataException("Invalid ExtensionCase!");
            }
            foreach (var type in AdditionalFileTypes)
            {
                if (type == null) throw new InvalidDataException("Additional file type cannot be null!");
                if (string.IsNullOrWhiteSpace(type.Trim().TrimStart('*').TrimStart('.'))) throw new InvalidDataException("Additional file type cannot be empty!");
            }
        }
    }

    public sealed class RenameConfigs : ConfigDictionaryBase<RenameConfig>
    {
        public RenameConfigs(AppConfig appConfig) : base(appConfig) { }
    }
}
