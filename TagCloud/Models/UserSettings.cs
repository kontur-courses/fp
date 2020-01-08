using System;
using System.IO;

namespace TagCloud.Models
{
    public class UserSettings : ICloneable
    {
        public UserSettings(ImageSettings imageSettings, string pathToRead)
        {
            ImageSettings = imageSettings;
            PathToRead = pathToRead;
        }

        public ImageSettings ImageSettings { get; set; }
        public string PathToRead { get; set; }
        public static UserSettings DefaultSettings { get; } = UserSettings.GetDefaultUserSettings();
        public static UserSettings GetDefaultUserSettings()
        {
            var defaultPath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.FullName}\\test.txt";
            return new UserSettings(ImageSettings.GetDefaultSettings(), defaultPath);
        }

        public void MakeDefault()
        {
            var defaultSettings = GetDefaultUserSettings();
            PathToRead = defaultSettings.PathToRead;
            ImageSettings = defaultSettings.ImageSettings;
        }

        public override int GetHashCode()
        {
            return ImageSettings.GetHashCode() + PathToRead.GetHashCode() * 887;
        }

        public override bool Equals(object obj)
        {
            var settings = obj as UserSettings;
            return !(obj is null) &&
                   PathToRead == settings.PathToRead &&
                   ImageSettings.Equals(settings.ImageSettings);
        }

        public object Clone()
        {
            return new UserSettings(ImageSettings.Clone() as ImageSettings,PathToRead);
        }
    }
}