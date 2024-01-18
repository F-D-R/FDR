using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FDR.Tools.Library
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CharacterCasing
    {
        [Display(Name = "Upper")]
        upper = 0,
        [Display(Name = "Lower")]
        lower = 1,
        [Display(Name = "Unchanged")]
        unchanged = 2
    }

    public class RenameConfig : ConfigPartBase
    {
        public static Dictionary<string, string> GetRenameConfigAttributeList()
        {
            var attributes = new Dictionary<string, string>()
            {
                { nameof(FileFilter), "Filter condition for the files to process. Wildcards (*?) are supported. Multiple filters can be defined separated with pipes. Example: \"*.CR3|*.CR2\"" },
                { nameof(AdditionalFiles), " Defines if additional files with the same name should be renamed too (true) or not (false). Default is true." },
                { nameof(FilenamePattern), "The name pattern to rename the files to. It can contain static text parts and placeholders described below. Example: \"{mdate:yyMMdd}_{counter:3}s\"" },
                { nameof(FilenameCase), $"The character case of the new filename. Possible values: \"{nameof(CharacterCasing.unchanged)}\", \"{nameof(CharacterCasing.lower)}\", \"{nameof(CharacterCasing.upper)}\". Default is {nameof(CharacterCasing.unchanged)}." },
                { nameof(ExtensionCase), $"The character case of the new file's extension. Possible values: \"{nameof(CharacterCasing.unchanged)}\", \"{nameof(CharacterCasing.lower)}\", \"{nameof(CharacterCasing.upper)}\". Default is {nameof(CharacterCasing.lower)}." },
                { nameof(Recursive), "Defines if only the files of the current folder should be renamed (false) or the ones in the subfolders as well (true). Default is false." },
                { nameof(StopOnError), "Defines whether to stop on the first error during batch renaming of several files (true) or to continue the batch (false). Default is true." }
            };
            return attributes;
        }

        private const string DEFAULT_FILTER = "*.*";
        private const string FILENAME_PATTERN_ERROR = "Renaming filename pattern cannot be empty!";
        private const string INVALID_FILENAME_CASE = "Invalid filename case!";
        private const string INVALID_EXTENSION_CASE = "Invalid extension case!";

        [DisplayName("Filename pattern")]
        [Required(ErrorMessage = FILENAME_PATTERN_ERROR)]
        public string? FilenamePattern { get; set; } = "{name}";

        [DisplayName("Filename case")]
        [Required]
        [Range(0, 2, ErrorMessage = INVALID_FILENAME_CASE)]
        public CharacterCasing FilenameCase { get; set; } = CharacterCasing.unchanged;

        [DisplayName("Extension case")]
        [Required]
        [Range(0, 2, ErrorMessage = INVALID_EXTENSION_CASE)]
        public CharacterCasing ExtensionCase { get; set; } = CharacterCasing.lower;

        protected string? filter;
        [DisplayName("File filter")]
        public virtual string FileFilter
        {
            get { return !string.IsNullOrWhiteSpace(filter) ? filter : DEFAULT_FILTER; }
            set { filter = value; }
        }

        [DisplayName("Additional files")]
        public virtual bool AdditionalFiles { get; set; } = true;

        public bool Recursive { get; set; } = false;

        [DisplayName("Stop on errors")]
        public bool StopOnError { get; set; } = true;

        public RenameConfig Clone()
        {
            return (RenameConfig)MemberwiseClone();
        }

        public override void Validate()
        {
            base.Validate();

            if (string.IsNullOrWhiteSpace(FilenamePattern)) throw new InvalidDataException(FILENAME_PATTERN_ERROR);

            switch (FilenameCase)
            {
                case CharacterCasing.unchanged:
                case CharacterCasing.lower:
                case CharacterCasing.upper:
                    break;
                default:
                    throw new InvalidDataException(INVALID_FILENAME_CASE);
            }

            switch (ExtensionCase)
            {
                case CharacterCasing.unchanged:
                case CharacterCasing.lower:
                case CharacterCasing.upper:
                    break;
                default:
                    throw new InvalidDataException(INVALID_EXTENSION_CASE);
            }
        }
    }

    public sealed class RenameConfigs : ConfigDictionaryBase<RenameConfig>
    {
        public RenameConfigs(AppConfig appConfig) : base(appConfig) { }
    }
}
