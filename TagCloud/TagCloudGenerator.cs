using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Counter;
using TagCloud.Data;
using TagCloud.Drawer;
using TagCloud.Processor;
using TagCloud.Reader;
using TagCloud.Saver;
using TagCloud.WordsLayouter;

namespace TagCloud
{
    public class TagCloudGenerator
    {
        private readonly IWordsFileReader wordsFileReader;
        private readonly IEnumerable<IWordsProcessor> processors;
        private readonly IWordsCounter counter;
        private readonly IWordsLayouter wordsLayouter;
        private readonly IWordsDrawer wordsDrawer;
        private readonly IEnumerable<IImageSaver> savers;

        public TagCloudGenerator(
            IWordsLayouter wordsLayouter,
            IWordsDrawer wordsDrawer,
            IWordsFileReader wordsFileReader,
            IEnumerable<IWordsProcessor> processors, 
            IWordsCounter counter,
            IEnumerable<IImageSaver> savers)
        {
            this.wordsLayouter = wordsLayouter;
            this.wordsDrawer = wordsDrawer;
            this.wordsFileReader = wordsFileReader;
            this.processors = processors;
            this.counter = counter;
            this.savers = savers;
        }

        public Result<None> Generate(Arguments arguments)
        {
            var wordsColor = Color.FromName(arguments.WordsColorName);
            var backgroundColor = Color.FromName(arguments.BackgroundColorName);
            var font = new FontFamily(arguments.FontFamilyName);

            return wordsFileReader
                .ReadWords(arguments.WordsFileName)
                .Then(Process)
                .Then(words => counter.GetWordsInfo(words))
                .Then(wordInfos => wordsLayouter.GenerateLayout(wordInfos, font, arguments.Multiplier))
                .Then(layout => wordsDrawer.CreateImage(layout, wordsColor, backgroundColor))
                .Then(image => Save(image, arguments));
        }

        private Result<IEnumerable<string>> Process(IEnumerable<string> initialWords)
        {
            return processors.Aggregate(Result.Ok(initialWords),
                (current, wordsProcessor) => current.Then(wordsProcessor.Process));
        }

        private Result<None> Save(Bitmap image, Arguments arguments)
        {
            return savers
                .Where(saver => saver.GetType() != typeof(ClipboardImageSaver) || arguments.ToEnableClipboardSaver)
                .Aggregate(Result.Ok<None>(null),
                    (current, saver) => current.Then(_ => saver.Save(image, arguments.ImageFileName)));
        }
    }
}