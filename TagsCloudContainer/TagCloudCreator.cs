using System;
using TagsCloudContainer.Drawing;
using TagsCloudContainer.FileReaders;
using TagsCloudContainer.ImageSavers;
using TagsCloudContainer.Settings;
using TagsCloudContainer.WordFilters;
using TagsCloudContainer.WordsConverter;
using TagsCloudContainer.WordsPreprocessors;

namespace TagsCloudContainer
{
    public class TagCloudCreator
    {
        private readonly IImageSaver saver;
        private readonly IWordsFilter wordsFilter;
        private readonly IFileReaderFactory fileReaderFactory;
        private readonly IDrawer cloudDrawer;
        private readonly IAppSettings appSettings;
        private readonly IWordConverter wordConverter;
        private readonly IWordsPreprocessor wordsPreprocessor;

        public TagCloudCreator(
            IImageSaver saver, 
            IWordsFilter wordsFilter, 
            IFileReaderFactory fileReaderFactory,
            IDrawer cloudDrawer, 
            IAppSettings appSettings, 
            IWordConverter wordConverter,
            IWordsPreprocessor wordsPreprocessor)
        {
            this.saver = saver;
            this.wordsFilter = wordsFilter;
            this.fileReaderFactory = fileReaderFactory;
            this.cloudDrawer = cloudDrawer;
            this.appSettings = appSettings;
            this.wordConverter = wordConverter;
            this.wordsPreprocessor = wordsPreprocessor;
        }

        public void CreateTagCloudImage()
        {
            fileReaderFactory
                .GetProperFileReader(appSettings.InputPath)
                .Then(reader => reader.ReadWordsFromFile(appSettings.InputPath))
                .Then(wordsPreprocessor.ProcessWords)
                .Then(wordsFilter.Filter)
                .Then(wordConverter.ConvertWords)
                .Then(cloudDrawer.DrawImage)
                .Then(image => saver.Save(image, appSettings.ImagePath))
                .OnFail(Console.WriteLine);
        }
    }
}