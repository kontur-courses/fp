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
        private readonly IFileInfoProvider fileInfoProvider;
        private readonly ITextReaderSelector textReaderSelector;
        private readonly IWordProcessor wordProcessor;
        private readonly IWordSizeSetter wordSizeSetter;
        private readonly ITagCloudGenerator tagCloudGenerator;
        private readonly IImageFormat imageFormat;

        public Application(
            ISettingsProvider settingsProvider,
            IFileInfoProvider fileInfoProvider,
            ITextReaderSelector textReaderSelector,
            IWordProcessor wordProcessor, 
            IWordSizeSetter wordSizeSetter,
            ITagCloudGenerator tagCloudGenerator,
            IImageFormat imageFormat)
        {
            this.settingsProvider = settingsProvider;
            this.fileInfoProvider = fileInfoProvider;
            this.textReaderSelector = textReaderSelector;
            this.wordProcessor = wordProcessor;
            this.wordSizeSetter = wordSizeSetter;
            this.tagCloudGenerator = tagCloudGenerator;
            this.imageFormat = imageFormat;
        }

        public Result<None> Run()
        {
            var settings = settingsProvider.GetSettings().GetValueOrThrow();
           // var file = fileInfoProvider.GetFileInfo(settings.InputFilePath);
            var textReader = textReaderSelector.GetTextReader(settings.InputFileInfo);
            var wordReadingResult = textReader.ReadWords(settings.InputFileInfo);
            if (!wordReadingResult.IsSuccess)
                return Result.Fail<None>(wordReadingResult.Error);
            var rawWords = wordReadingResult.GetValueOrThrow();
            var preparedWords = wordProcessor.PrepareWords(rawWords).ToList();
            var sizedWords = wordSizeSetter.GetSizedWords(preparedWords, settings.PictureConfig).ToList();
            var bitmap = tagCloudGenerator.GetTagCloudBitmap(sizedWords);
            imageFormat.SaveImage(bitmap, settings.OutputFilePath);
            return Result.Ok();
        }
    }
}
