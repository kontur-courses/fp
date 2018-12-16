using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TagCloud.Settings
{
    public class SettingsChecker
    {
        public ImageSettings ImageSettings;
        public FontSettings FontSettings;

        public SettingsChecker()
        {
            CreateFileWatcher(Environment.CurrentDirectory);
        }

        public void CreateFileWatcher(string path)
        {
            ReadJson($"{path}\\settings.json");
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                            | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.json";

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;

            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            MessageBox.Show("File: " + e.FullPath + " " + e.ChangeType);
            ReadJson(e.FullPath);
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