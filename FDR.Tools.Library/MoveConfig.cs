using System.IO;

namespace FDR.Tools.Library
{
    public class MoveConfig : RenameConfig
    {
        private const string DEFAULT_FILTER = "*.*";

        public override string FileFilter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        public string? RelativeFolder { get; set; }

        public RenameConfig GetNewRenameConfig()
        {
            var config = new RenameConfig();
            config.AppConfig = AppConfig;
            config.FileFilter = FileFilter;
            config.AdditionalFileTypes.AddRange(AdditionalFileTypes);
            if (string.IsNullOrWhiteSpace(RelativeFolder))
                config.FilenamePattern = FilenamePattern;
            else
                config.FilenamePattern = RelativeFolder + "/" + FilenamePattern;
            config.FilenameCase = FilenameCase;
            config.ExtensionCase = ExtensionCase;
            config.Recursive = Recursive;
            config.StopOnError = StopOnError;
            return config;
        }

        public override void Validate()
        {
            base.Validate();

            if (string.IsNullOrWhiteSpace(FileFilter)) throw new InvalidDataException("Filename filter cannot be empty!");
        }
    }
}
