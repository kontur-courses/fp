using System;
using TagCloudContainer.FileReaders;
using TagCloudContainer.Result;
using TagCloudContainer.UI;

namespace TagCloudContainer.FileSavers
{
    public class FileSaverFactory
    {
        private readonly Func<IUi> settingsProvider;

        public FileSaverFactory(Func<IUi> settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        public Result<IFileSaver> Create()
        {
            var actualSettings = settingsProvider.Invoke();
            return actualSettings.FormatToSave switch
            {
                "png" => new Result<IFileSaver>(null, new PngSaver()),
                "jpeg" => new Result<IFileSaver>(null, new JpegSaver()),
                "gif" => new Result<IFileSaver>(null, new GifSaver()),
                "bmp" => new Result<IFileSaver>(null, new BmpSaver()),
                _ => new Result<IFileSaver>("Wrong file format")
            };
        }
    }
}