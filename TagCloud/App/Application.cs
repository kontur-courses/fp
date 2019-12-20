using System;
using System.Linq;
using ResultOf;
using TagCloud.Infrastructure;
using TagCloud.TextReading;
using TagCloud.Visualization;
using TagCloud.WordsProcessing;

namespace TagCloud.App
{
    public class Application
    {
        private readonly ISettingsProvider settingsProvider;
        private readonly ITextReaderSelector textReaderSelector;
        private readonly IWordProcessor wordProcessor;
        private readonly IWordSizeSetter wordSizeSetter;
        private readonly ITagCloudGenerator tagCloudGenerator;
        private readonly IImageFormat imageFormat;

        public Application(
            ISettingsProvider settingsProvider,
            ITextReaderSelector textReaderSelector,
            IWordProcessor wordProcessor, 
            IWordSizeSetter wordSizeSetter,
            ITagCloudGenerator tagCloudGenerator,
            IImageFormat imageFormat)
        {
            this.settingsProvider = settingsProvider;
            this.textReaderSelector = textReaderSelector;
            this.wordProcessor = wordProcessor;
            this.wordSizeSetter = wordSizeSetter;
            this.tagCloudGenerator = tagCloudGenerator;
            this.imageFormat = imageFormat;
        }

        public Result<None> Run()
        {
            var settingsResult = settingsProvider.GetSettings();
            if (!settingsResult.IsSuccess)
                return Result.Fail<None>(settingsResult.Error);
            var settings = settingsResult.GetValueOrThrow();
            return textReaderSelector
                .GetTextReader(settings.InputFileInfo)
                .Then(r => r.ReadWords(settings.InputFileInfo))
                .Then(w => wordProcessor.PrepareWords(w))
                .Then(w => wordSizeSetter.GetSizedWords(w, settings.PictureConfig))
                .Then(w => tagCloudGenerator.GetTagCloudBitmap(w))
                .Then(b => imageFormat.SaveImage(b, settings.OutputFilePath));
        }
    }
}
