using System.IO;

namespace FDR.Tools.Library
{
    public class MoveConfig : RenameConfig
    {
        //private const string DEFAULT_FILTER = "*.CR3|*.CR2|*.CRW";  // *.NEF|*.ARW|*.PEF ???
        private const string DEFAULT_FILTER = "*.*";

        public override string FileFilter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        public string? RelativeFolder { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (string.IsNullOrWhiteSpace(FileFilter)) throw new InvalidDataException("Filename filter cannot be empty!");
            //if (string.IsNullOrWhiteSpace(RelativeFolder)) throw new InvalidDataException("Relative folder path cannot be empty!");
        }
    }
}
