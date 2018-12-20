using TagsCloud.Graphics;
using TagsCloud.Layout;
using TagsCloud.Words;

namespace TagsCloud
{
    public class TagCloudRender
    {
        private readonly IFrequencyCollection collection;
        private readonly CoordinatesAtImage coordinatesAtImage;
        private readonly IGraphics graphics;
        private readonly ITagCloudLayouter layout;
        private readonly IBoringWordsCollection words;

        public TagCloudRender(ITagCloudLayouter layout, CoordinatesAtImage coordinatesAtImage,
            IBoringWordsCollection words,
            IFrequencyCollection collection, IGraphics graphics)
        {
            this.layout = layout;
            this.coordinatesAtImage = coordinatesAtImage;
            this.words = words;
            this.collection = collection;
            this.graphics = graphics;
        }

        public Result<None> Render()
        {
            return collection.GetFrequencyCollection(words.DeleteBoringWords().Value)
                .Then(frequencyDictionary => layout.GetLayout(frequencyDictionary)
                    .Then(wordsToDraw => coordinatesAtImage.GetCoordinates(wordsToDraw))
                    .Then(coordinates => graphics.Save(coordinates)));
        }
    }
}