using System;
using System.Linq;
using System.Windows.Forms;

namespace TagCloud.Models
{
    public class UserSettings
    {
        public UserSettings(ImageSettings imageSettings, string pathToRead)
        {
            ImageSettings = imageSettings;
            PathToRead = pathToRead;
            PathIsRead = false;
        }

        public UserSettings()
        {
            ImageSettings = new ImageSettings();
            PathIsRead = false;
            PathToRead = null;
        }

        public bool IsCompleted => PathIsRead && ImageSettings.IsCompleted;

        public ImageSettings ImageSettings { get; set; }
        public string PathToRead { get; private set; }
        private bool PathIsRead { get; set; }

        public void ReadPath(string path)
        {
            PathToRead = path;
            PathIsRead = true;
        }
        public bool CheckToComplete()
        {
            if (IsCompleted) return true;
            Console.WriteLine("Сначала задайте параметры: ");
            if (ImageSettings.IsCompleted)
            {
                foreach (var field in ImageSettings.FieldsToRead.Where(f => !f.Value))
                    Console.WriteLine(field.Key);
            }
            if(!PathIsRead)
                Console.WriteLine(PathIsRead);
            return false;

        }
    }
}