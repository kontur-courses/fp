using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.FontFactory;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.WordsSizeServices;

namespace TagsCloud.Visualization.LayoutContainer.ContainerBuilder
{
    public class WordsContainerBuilder : AbstractWordsContainerBuilder
    {
        private readonly IFontFactory fontFactory;
        private readonly ICloudLayouter layouter;
        private readonly List<Tag> words = new();
        private readonly IWordsSizeService wordsSizeService;

        public WordsContainerBuilder(
            ICloudLayouter layouter,
            IWordsSizeService wordsSizeService,
            IFontFactory fontFactory)
        {
            this.layouter = layouter;
            this.wordsSizeService = wordsSizeService;
            this.fontFactory = fontFactory;
        }

        public override TagsContainer Build() => new()
        {
            Items = words,
            Size = CalculateSize(),
            Center = words.First().Border.GetCenter()
        };

        protected override WordsContainerBuilder AddWord(Word word, int minCount, int maxCount)
        {
            var font = fontFactory.GetFont(word, minCount, maxCount);
            var size = wordsSizeService.CalculateSize(word, font);

            layouter.PutNextRectangle(size)
                .Then(rectangle => words.Add(new Tag(word, font, rectangle)));

            return this;
        }

        private Size CalculateSize() => words.Select(x => x.Border).GetSize();
    }
}