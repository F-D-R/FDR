using System;
using System.Collections.Generic;
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
        public static Dictionary<string, string> GetResizeConfigAttributeList()
        {
            var attributes = MoveConfig.GetMoveConfigAttributeList();
            attributes.Add(nameof(ResizeMethod), $"The method of resizing. Possible values: \"{nameof(ResizeMethod.fit_in)}\", \"{nameof(ResizeMethod.max_width)}\", \"{nameof(ResizeMethod.max_height)}\", \"{nameof(ResizeMethod.stretch)}\". Default is {nameof(ResizeMethod.fit_in)}.");
            attributes.Add(nameof(MaxWidth), "The maximum width to which the image will be resized.");
            attributes.Add(nameof(MaxHeight), "The maximum height to which the image will be resized.");
            attributes.Add(nameof(JpgQuality), $"The quality of the resized JPG image. Valid range is from 0 to 100. Default is {DEFAULT_QUALITY}.");
            attributes.Add(nameof(ClearMetadata), "Defines if all the metadata should be erased from the resized image. Default is false.");
            return attributes;
        }

        private const string DEFAULT_FILTER = "*.JPG";
        private const int DEFAULT_QUALITY = 90;

        public ResizeMethod ResizeMethod { get; set; } = ResizeMethod.fit_in;

        public int MaxWidth { get; set; }

        public int MaxHeight { get; set; }

        public bool ClearMetadata { get; set; }

        private int jpgQuality = DEFAULT_QUALITY;

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
