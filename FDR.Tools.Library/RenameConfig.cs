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
        public static Dictionary<string, string> GetRenameConfigAttributeList()
        {
            var attributes = new Dictionary<string, string>();
            attributes.Add(nameof(FileFilter), "Filter condition for the files to process. Wildcards (*?) are supported. Multiple filters can be defined separated with pipes. Example: \"*.CR3|*.CR2\"");
            attributes.Add(nameof(AdditionalFileTypes), "List of file types (extensions) which should be renamed together with the originally filtered files. Example: { \"JPG\", \"XMP\" }");
            attributes.Add(nameof(FilenamePattern), "The name pattern to rename the files to. It can contain static text parts and placeholders described below. Example: \"{mdate:yyMMdd}_{counter:3}s\"");
            attributes.Add(nameof(FilenameCase), $"The character case of the new filename. Possible values: \"{nameof(CharacterCasing.unchanged)}\", \"{nameof(CharacterCasing.lower)}\", \"{nameof(CharacterCasing.upper)}\". Default is {nameof(CharacterCasing.unchanged)}.");
            attributes.Add(nameof(ExtensionCase), $"The character case of the new file's extension. Possible values: \"{nameof(CharacterCasing.unchanged)}\", \"{nameof(CharacterCasing.lower)}\", \"{nameof(CharacterCasing.upper)}\". Default is {nameof(CharacterCasing.lower)}.");
            attributes.Add(nameof(Recursive), "Defines if only the files of the current folder should be renamed (false) or the ones in the subfolders as well (true). Default is false.");
            attributes.Add(nameof(StopOnError), "Defines whether to stop on the first error during batch renaming of several files (true) or to continue the batch (false). Default is true.");
            return attributes;
        }

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
