using System;
using System.Linq;
using ResultOf;

namespace TagCloud.Models
{
    public class UserSettings
    {
        public UserSettings(ImageSettings imageSettings, string pathToRead)
        {
            ImageSettings = imageSettings;
            PathToRead = pathToRead;
        }

        public UserSettings()
        {
            ImageSettings = new ImageSettings();
            PathToRead = null;
        }
        public ImageSettings ImageSettings { get; set; }
        public string PathToRead { get; set;}
    }
}