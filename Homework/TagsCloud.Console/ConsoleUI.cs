using TagsCloudContainer;
using TagsCloudContainer.BitmapSaver;
using TagsCloudContainer.FileReader;

namespace TagsCloud.Console
{
    public class ConsoleUI : IConsoleUI
    {
        private readonly IResolver<string, IFileReader> fileReadersResolver;
        private readonly IBitmapSaver saver;
        private readonly ITagCloud tagCloud;
        private readonly IAppSettings appSettings;

        public ConsoleUI(IAppSettings appSettings, IResolver<string, IFileReader> fileReadersResolver, IBitmapSaver saver, ITagCloud tagCloud)
        {
            this.appSettings = appSettings;
            this.fileReadersResolver = fileReadersResolver;
            this.saver = saver;
            this.tagCloud = tagCloud;
        }

        public Result<None> Run()
        {
            return fileReadersResolver.Get(appSettings.InputPath)
                .Then(fileReader => fileReader.ReadWords(appSettings.InputPath))
                .Then(words => tagCloud.LayDown(words))
                .Then(bmp => saver.Save(bmp, appSettings.OutputPath));
        }
    }
}