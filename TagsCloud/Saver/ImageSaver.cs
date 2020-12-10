using System;
using System.Drawing;
using TagsCloud.Settings.SettingsForStorage;

namespace TagsCloud.Saver
{
    public class ImageSaver : IImageSaver
    {
        private readonly IStorageSettings _storageSettings;

        public ImageSaver(IStorageSettings storageSettings)
        {
            _storageSettings = storageSettings;
        }

        public void Save(Image image)
        {
            try
            {
                image.Save($"{_storageSettings.PathToSave}.{_storageSettings.ImageFormat.ToString().ToLower()}",
                    _storageSettings.ImageFormat);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to save image", e);
            }
        }
    }
}