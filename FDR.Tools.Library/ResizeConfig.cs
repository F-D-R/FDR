using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FDR.Tools.Library
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResizeMethod
    {
        fit_in,
        max_width,
        max_height,
        stretch
    }

    public class ResizeConfig : MoveConfig
    {
        private const string DEFAULT_FILTER = "*.JPG";

        public ResizeMethod ResizeMethod { get; set; } = ResizeMethod.fit_in;

        public int MaxWidth { get; set; }

        public int MaxHeight { get; set; }

        public bool ClearMetadata { get; set; }

        private int jpgQuality = 90;

        public int JpgQuality
        {
            get { return Math.Max(0, Math.Min(100, jpgQuality)); }
            set { jpgQuality = Math.Max(0, Math.Min(100, value)); }
        }

        public override string FileFilter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        public override void Validate()
        {
            base.Validate();

            if (MaxWidth <= 0) throw new InvalidDataException("Maximum width should be more than zero!");
            if (MaxHeight <= 0) throw new InvalidDataException("Maximum height should be more than zero!");
            if (JpgQuality < 0 || JpgQuality > 100) throw new InvalidDataException("JPG Quality must be between 0 and 100!");
            switch (ResizeMethod)
            {
                case ResizeMethod.fit_in:
                case ResizeMethod.max_width:
                case ResizeMethod.max_height:
                case ResizeMethod.stretch:
                    break;
                default:
                    throw new InvalidDataException("Invalid ResizeMethod!");
            }
        }
    }

    public sealed class ResizeConfigs : ConfigDictionaryBase<ResizeConfig>
    {
        public ResizeConfigs(AppConfig appConfig) : base(appConfig) { }
    }
}
