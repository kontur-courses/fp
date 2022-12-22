using System;
using TagCloudContainer.Result;
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
                "txt" => new Result<IFileReader>(null, new TxtReader()),
                _ => new Result<IFileReader>("Wrong path to open")
            };
        }
    }
}