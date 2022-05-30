using System.IO;

namespace FDR.Tools.Library
{
    public class MoveConfig
    {
        public string Filter { get; set; } = "*.CR3|*.CR2";

        public string RelativeFolder { get; set; } = "RAW";

        public bool StopOnError { get; set; } = true;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Filter)) throw new InvalidDataException("Filename filter cannot be empty!");
            if (string.IsNullOrWhiteSpace(RelativeFolder)) throw new InvalidDataException("Relative folder path cannot be empty!");
        }
    }
}
