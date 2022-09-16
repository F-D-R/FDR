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

    public class ResizeConfig : RenameConfig
    {
        public ResizeMethod ResizeMethod { get; set; } = ResizeMethod.fit_in;

        public int MaxWidth { get; set; } = 600;

        public int MaxHeight { get; set; } = 600;

        public bool ClearMetadata { get; set; } = true;

        private int jpgQuality = 90;

        public int JpgQuality
        {
            get { return Math.Max(0, Math.Min(100, jpgQuality)); }
            set { jpgQuality = Math.Max(0, Math.Min(100, value)); }
        }

        public override void Validate()
        {
            base.Validate();

            if (MaxWidth <= 0) throw new InvalidDataException("Maximum width should be more than zero!");
            if (MaxHeight <= 0) throw new InvalidDataException("Maximum height should be more than zero!");
            if (JpgQuality < 0 || JpgQuality > 100) throw new InvalidDataException("JPG Quality must be between 0 and 100!");
        }
    }

    public class ResizeConfigs : ConfigDictionaryBase<ResizeConfig>
    {
        public ResizeConfigs(AppConfig appConfig) : base(appConfig) { }
    }

    public sealed class BatchResizeConfig : ResizeConfig
    {
        private const string DEFAULT_FILTER = "*.JPG";

        private string? filter;
        public string FileFilter
        {
            get { return string.IsNullOrWhiteSpace(filter) ? DEFAULT_FILTER : filter; }
            set { filter = value; }
        }

        public bool StopOnError { get; set; } = true;

        public override void Validate()
        {
            base.Validate();
        }
    }

    public class BatchResizeConfigs : ConfigDictionaryBase<BatchResizeConfig>
    {
        public BatchResizeConfigs(AppConfig appConfig) : base(appConfig) { }
    }
}
