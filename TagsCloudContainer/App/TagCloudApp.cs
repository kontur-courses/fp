using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.CloudLayouter;
using TagsCloudContainer.CounterNamespace;
using TagsCloudContainer.Extensions;
using TagsCloudContainer.MorphologicalAnalysis;
using TagsCloudContainer.Visualizer;

namespace TagsCloudContainer.App
{
    public class TagCloudApp : IApp
    {
        private readonly MorphologicalAnalyzer analyzer;
        private readonly ICloudLayouter cloud;
        private readonly Options options;
        private readonly IVisualizer visualizer;

        public TagCloudApp(ICloudLayouter cloud, IVisualizer visualizer,
            MorphologicalAnalyzer analyzer, Options options)
        {
            this.options = options;
            this.cloud = cloud;
            this.visualizer = visualizer;
            this.analyzer = analyzer;
        }

        public Result<None> Run()
        {
            return Result.Of(() => FillCloud())
                .Then(none => visualizer.Visualize())
                .Then(none => visualizer.Save())
                .RefineError("Could not run App");
        }

        private Result<Counter<Word>> GetCounterSelectedWords()
        {
            var partSpeech = MorphologicalAnalyzer.GetPartSpeech(options.PartSpeeches);
            if (!partSpeech.IsSuccess)
                return Result.Fail<Counter<Word>>(partSpeech.Error);
            var words = analyzer.GetWords();
            var selectedWords = new List<Word>();
            foreach (var word in words)
            {
                if (!word.IsSuccess)
                    return Result.Fail<Counter<Word>>(word.Error);
                if (partSpeech.Value.HasFlag(word.Value.PartSpeech))
                    selectedWords.Add(word.Value);
            }
            return new Counter<Word>(selectedWords);
        }

        private Result<None> FillCloud()
        {
            var counter = GetCounterSelectedWords();
            if (!counter.IsSuccess)
                return Result.Fail<None>(counter.Error);
            var words = counter.Value.GetMostPopular(options.Count).ToList();
            var minAmount = counter.Value.GetAmount(words.Last());
            var maxAmount = counter.Value.GetAmount(words.First());
            foreach (var word in words)
            {
                if (visualizer is TagCloudVisualizer tagCloudVisualizer)
                {
                    var font = tagCloudVisualizer
                        .GetFontByWeightWord(counter.Value.GetAmount(word), minAmount, maxAmount);
                    if (!font.IsSuccess)
                        return Result.Fail<None>(font.Error);
                    var size = tagCloudVisualizer.MeasureString(word.Text, font.Value).Ceiling();
                    var item = cloud.PutNextCloudItem(word.Text, size, font.Value);
                    if (!item.IsSuccess)
                        return Result.Fail<None>(item.Error);
                }
            }
            return Result.Ok();
        }
    }
}