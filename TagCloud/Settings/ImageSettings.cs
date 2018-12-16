using System.Drawing;
using Newtonsoft.Json;

namespace TagCloud.Settings
{
    public class ImageSettings
    {
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 800;
        public Color TextColor { get; set; } = Color.White;
        public Color BackgroundColor { get; set; } = Color.Black;
        
        public void UpdateSettings()
        {
            var settings = new SettingsLoader().Settings["ImageSettings"];
            var imageSettings = JsonConvert.DeserializeObject<ImageSettings>(settings);
            Width = imageSettings.Width;
            Height = imageSettings.Height;
            TextColor = imageSettings.TextColor;
            BackgroundColor = imageSettings.BackgroundColor;
        }
    }
}