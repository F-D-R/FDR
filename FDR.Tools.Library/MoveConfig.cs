using System.Collections.Generic;
using System.IO;

namespace FDR.Tools.Library
{
    public class MoveConfig : RenameConfig
    {
        public static Dictionary<string, string> GetMoveConfigAttributeList()
        {
            var attributes = RenameConfig.GetRenameConfigAttributeList();
            attributes.Add(nameof(RelativeFolder), "Folder(s) name relative to the source folder. It can contain several folder names separated by slashes. Upper navigation is also supported with double dots. Example: \"*../some/other/folder\"");
            return attributes;
        }

        private const string DEFAULT_FILTER = "*.*";

        public override string FileFilter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        public string? RelativeFolder { get; set; }

        public override bool AdditionalFiles { get; set; } = false;

        public RenameConfig GetNewRenameConfig()
        {
            var config = new RenameConfig
            {
                AppConfig = AppConfig,
                FileFilter = FileFilter,
                AdditionalFiles = AdditionalFiles,
                FilenamePattern = string.IsNullOrWhiteSpace(RelativeFolder) ? FilenamePattern : RelativeFolder + "/" + FilenamePattern,
                FilenameCase = FilenameCase,
                ExtensionCase = ExtensionCase,
                Recursive = Recursive,
                StopOnError = StopOnError
            };
            return config;
        }

        public new MoveConfig Clone()
        {
            return (MoveConfig)MemberwiseClone();
        }

        public override void Validate()
        {
            base.Validate();

            if (string.IsNullOrWhiteSpace(FileFilter)) throw new InvalidDataException("Filename filter cannot be empty!");
        }
    }
}
