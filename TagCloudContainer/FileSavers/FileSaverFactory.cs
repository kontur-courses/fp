using System;
using TagCloudContainer.FileReaders;
using TagCloudContainer.TaskResult;
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
                "png" => Result.OnSuccess(new PngSaver() as IFileSaver),
                "jpeg" => Result.OnSuccess(new JpegSaver() as IFileSaver),
                "gif" => Result.OnSuccess(new GifSaver() as IFileSaver),
                "bmp" => Result.OnSuccess(new BmpSaver() as IFileSaver),
                _ => Result.OnFail<IFileSaver>("Wrong file format")
            };
        }
    }
}