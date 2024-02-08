using TagsCloudContainer.Infrastucture.Settings;

namespace TagsCloudContainer.Algorithm
{
    public sealed class CloudSizer : ICloudSizer
    {
        private readonly ImageSettings imageSettings;

        public CloudSizer(ImageSettings imageSettings)
        {
            this.imageSettings = imageSettings;
        }

        public (Font, SizeF) GetCloudSize(
            Dictionary<string, int> wordFrequencies, 
            string word)
        {
            using (var bitmap = new Bitmap(imageSettings.Width, imageSettings.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    var textFont = GetFontCloudText(wordFrequencies, word, imageSettings);
                    var textSize = graphics.MeasureString(word, textFont);

                    return (textFont, textSize);
                }
            }
        }

        private Font GetFontCloudText(Dictionary<string, int> wordFrequencies, string word, ImageSettings imageSettings)
        {
            var fontSize = CalculateFontSize(wordFrequencies, word, imageSettings.Font.Size);
            var font = new Font(imageSettings.Font.FontFamily, fontSize, imageSettings.Font.Style, imageSettings.Font.Unit);

            return font;
        }

        private float CalculateFontSize(Dictionary<string, int> wordFrequencies, string word, float fontSize)
        {
            return fontSize + (wordFrequencies[word] - wordFrequencies.Values.Min()) * 20 / (wordFrequencies.Values.Max());
        }
    }
}
