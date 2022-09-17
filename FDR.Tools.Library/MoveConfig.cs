using System.IO;

namespace FDR.Tools.Library
{
    public class MoveConfig
    {
        public string FileFilter { get; set; } = "*.CR3|*.CR2|*.CRW";  // *.NEF|*.ARW|*.PEF ???

        public string RelativeFolder { get; set; } = "RAW";

        public bool StopOnError { get; set; } = true;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FileFilter)) throw new InvalidDataException("Filename filter cannot be empty!");
            if (string.IsNullOrWhiteSpace(RelativeFolder)) throw new InvalidDataException("Relative folder path cannot be empty!");
        }
    }
}
