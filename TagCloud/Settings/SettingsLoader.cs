using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TagCloud.Settings
{
    public class SettingsLoader
    {
        public ImageSettings ImageSettings;
        public FontSettings FontSettings;

        public SettingsLoader()
        {
            CreateFileWatcher(Environment.CurrentDirectory);
        }

        private void CreateFileWatcher(string path)
        {
            ReadJson($"{path}\\settings.json");
            var watcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                        | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.json"
            };

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;

            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Result.OfAction(() => ReadJson(e.FullPath)).RefineError($"Error, trying to load json file {e.FullPath}");
        }

        private void ReadJson(string path)
        {
            var settingsFile = File.ReadAllText(path);
            var j = JArray.Parse(settingsFile);
            FontSettings = JsonConvert.DeserializeObject<FontSettings>(j[0].ToString());
            ImageSettings = JsonConvert.DeserializeObject<ImageSettings>(j[1].ToString());
        }
    }
}