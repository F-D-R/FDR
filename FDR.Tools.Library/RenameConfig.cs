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

    public class RenameConfig
    {
        public string? FilenamePattern { get; set; } = "{name}";

        public CharacterCasing FilenameCase { get; set; } = CharacterCasing.unchanged;

        public CharacterCasing ExtensionCase { get; set; } = CharacterCasing.lower;

        public virtual void Validate()
        {
            if (string.IsNullOrWhiteSpace(FilenamePattern)) throw new InvalidDataException("Renaming filename pattern cannot be empty!");
        }
    }

    public sealed class BatchRenameConfig : RenameConfig
    {
        private const string DEFAULT_FILTER = "*.CR3|*.CR2|*.MP4|*.AVI|*.MOV";

        private string? filter;
        public string FileFilter
        {
            get { return !string.IsNullOrWhiteSpace(filter) ? filter : DEFAULT_FILTER; }
            set { filter = value; }
        }

        public List<string> AdditionalFileTypes { get; } = new List<string>() { ".JPG" };

        public bool StopOnError { get; set; } = true;

        public override void Validate()
        {
            base.Validate();
            foreach (var type in AdditionalFileTypes)
            {
                if (string.IsNullOrWhiteSpace(type)) throw new InvalidDataException("Additional file type cannot be empty!");
                if (string.CompareOrdinal(type, type.Trim()) != 0) throw new InvalidDataException("Additional file type must not contain white spaces!");
                if (!type.StartsWith(".")) throw new InvalidDataException("Additional file type must start with a dot!");
            }
        }
    }
}
