using TagsCloudVisualization.InterfacesForSettings;

namespace TagsCloudVisualization.TagsCloud
{
    public class TagsCloudSettings : ITagsCloudSettings
    {
        public IWordsSettings WordsSettings { get; set; }
        public Palette Palette { get; set; }
        public IImageSettings ImageSettings { get; set; }
        public TypeTagsCloud TypeTagsCloud { get; set; }

        public TagsCloudSettings(IWordsSettings wordsSettings, Palette palette, IImageSettings imageSettings)
        {
            TypeTagsCloud = TypeTagsCloud.TagsCloud;
            WordsSettings = wordsSettings;
            Palette = palette;
            ImageSettings = imageSettings;
        }
    }
}