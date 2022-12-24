using System;
using TagCloudContainer.FileSavers;
using TagCloudContainer.TaskResult;
using TagCloudContainer.UI;

namespace TagCloudContainer.FileReaders
{
    public class FileReaderFactory
    {
        private readonly Func<IUi> settingsProvider;

        public FileReaderFactory(Func<IUi> settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        public Result<IFileReader> Create()
        {
            var actualSettings = settingsProvider.Invoke();
            var index = actualSettings.PathToOpen.LastIndexOf('.');
            var format = actualSettings.PathToOpen.Substring(index + 1);
            return format switch
            {
                "txt" => Result.OnSuccess(new TxtReader() as IFileReader),
                _ => Result.OnFail<IFileReader>("Wrong path to open")
            };
        }
    }
}