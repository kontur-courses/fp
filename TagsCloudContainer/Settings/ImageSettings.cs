using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TagsCloudContainer.Settings
{
    public class ImageSettings
    {
        private const int DefaultWordCount = 10;

        public Size Size { get; private set; }

        public FontFamily FontFamily { get; private set; }
        public float MaxFontSize { get; }
        public float MinFontSize { get; }

        public Color BackgroundColor { get; }
        public Color TextColor { get; }
        public Color RectangleColor { get; }

        public ImageFormat ImageFormat { get; }


        [DefaultValue(DefaultWordCount)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int WordCount { get; }

        [JsonExtensionData]
        // ReSharper disable once CollectionNeverUpdated.Local
#pragma warning disable 649
        private IDictionary<string, JToken> additionalData;
#pragma warning restore 649

        public ImageSettings(Size size, FontFamily fontFamily, float maxFontSize, float minFontSize,
            Color backgroundColor,
            Color textColor, Color rectangleColor, ImageFormat imageFormat, int wordCount = DefaultWordCount)
        {
            Size = size;
            FontFamily = fontFamily;
            MaxFontSize = maxFontSize;
            MinFontSize = minFontSize;
            BackgroundColor = backgroundColor;
            TextColor = textColor;
            RectangleColor = rectangleColor;
            ImageFormat = imageFormat;
            WordCount = wordCount;
        }

        [OnDeserialized]
        // ReSharper disable once IdentifierTypo
        private void OnDeserialized(StreamingContext context)
        {
            Size = new Size((int) additionalData["Width"], (int) additionalData["Height"]);
            FontFamily = new FontFamily((string) additionalData["Font"]);
        }

        public static Result<ImageSettings> FromJson(string jsonSource)
        {
            try
            {
                var settings = JsonConvert.DeserializeObject<ImageSettings>(jsonSource);
                return Result.Ok(settings);
            }
            catch (JsonReaderException)
            {
                return Result.Fail<ImageSettings>("Bad json file");
            }
            catch (Exception e)
            {
                return Result.Fail<ImageSettings>(e.Message);
            }
        }
    }
}