using System;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Layouter;

namespace TagsCloudContainer
{
    public class TagsCloudContainer
    {
        private ITextReader textReader;
        private IWordsFilter wordsFilter;
        private IWordsCounter wordsCounter;
        private IWordsToSizesConverter wordsToSizesConverter;
        private ICloudLayouter CCL;
        private IVisualiser visualiser;
        private IFileSaver imageSaver;
        private string outputFile;
        private string inputFile;

        public TagsCloudContainer(ITextReader textReader, IWordsFilter wordsFilter, IWordsCounter wordsCounter,
            IWordsToSizesConverter wordsToSizesConverter,
            ICloudLayouter ccl, IVisualiser visualiser, IFileSaver fileSaver,
            string output,
            string input
        )
        {
            this.textReader = textReader;
            this.wordsFilter = wordsFilter;
            this.wordsCounter = wordsCounter;
            this.wordsToSizesConverter = wordsToSizesConverter;
            CCL = ccl;
            this.visualiser = visualiser;
            outputFile = output;
            inputFile = input;
            imageSaver = fileSaver;
        }

        public Result<None> Perform()
        {
            var sizesResult = textReader.Read(inputFile)
                .Then(wordsFilter.FilterWords)
                .Then(wordsCounter.CountWords)
                .Then(wordsToSizesConverter.GetSizesOf)
                .OnFail(Console.WriteLine);

            if (sizesResult.IsSuccess)
            {
                var sizes = sizesResult.GetValueOrThrow().OrderByDescending(x => x.Item2.Width)
                    .ThenBy(x => x.Item2.Height).ToArray();

                CCL.Center = new Point(CCL.Center.X, CCL.Center.Y - sizes[0].Item2.Height);
                Result<Rectangle> rectangleRes = Result.Fail<Rectangle>("");
                for (var i = 0; i < sizes.Length; i++)
                {
                    rectangleRes = CCL.PutNextRectangle(sizes[i].Item2)
                        .RefineError("Probably you are giving too small size")
                        .OnFail(Console.WriteLine);
                    if (!rectangleRes.IsSuccess)
                        break;
                }

                if (rectangleRes.IsSuccess)
                {
                    var result = visualiser.DrawRectangles(CCL, sizes).Then(inp => imageSaver.Save(inp, outputFile))
                        .OnFail(Console.WriteLine);
                    if (result.IsSuccess)
                        return Result.Ok();
                }
            }

            return Result.Fail<None>("File wasn't created. Try again.");
        }
    }
}