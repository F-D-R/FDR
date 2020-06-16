using System.IO;
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
        private const string DEFAULT_FILTER = "*.CR2|*.MP4|*.AVI|*.MOV";

        private string filter;
        public string Filter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        public string FilenamePattern { get; set; }

        public CharacterCasing FilenameCase { get; set; } = CharacterCasing.unchanged;

        public CharacterCasing ExtensionCase { get; set; } = CharacterCasing.lower;

        public string[] AdditionalFileTypes { get; set; } = { ".JPG" };

        public bool StopOnError { get; set; } = true;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FilenamePattern)) throw new InvalidDataException("Filename rename pattern cannot be empty!");
            if (AdditionalFileTypes != null)
            {
                foreach (var type in AdditionalFileTypes)
                {
                    if (string.IsNullOrWhiteSpace(type)) throw new InvalidDataException("Additional file type cannot be empty!");
                    if (string.CompareOrdinal(type, type.Trim()) != 0) throw new InvalidDataException("Additional file type must not contain white spaces!");
                    if (!type.StartsWith(".")) throw new InvalidDataException("Additional file type must start with a dot!");
                }
            }
        }
    }
}
