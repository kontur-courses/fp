using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.FontFactory;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.WordsSizeServices;

namespace TagsCloud.Visualization.LayoutContainer.ContainerBuilder
{
    public class TagsContainerBuilder : AbstractTagsContainerBuilder
    {
        private readonly IFontFactory fontFactory;
        private readonly ICloudLayouter layouter;
        private readonly List<Tag> tags = new();
        private readonly IWordsSizeService wordsSizeService;

        public TagsContainerBuilder(
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
            Items = tags,
            Size = CalculateSize(),
            Center = tags.First().Border.GetCenter()
        };

        protected override TagsContainerBuilder AddWord(Word word, int minCount, int maxCount)
        {
            var font = fontFactory.GetFont(word, minCount, maxCount);
            var size = wordsSizeService.CalculateSize(word, font);

            layouter.PutNextRectangle(size)
                .Then(rectangle => tags.Add(new Tag(word, font, rectangle)));

            return this;
        }

        private Size CalculateSize() => tags.Select(x => x.Border).GetSize();
    }
}