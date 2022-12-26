using TagsCloudContainer.Core.Options.Interfaces;

namespace TagsCloudContainer.Core.Options
{
    public class TagCloudOptions : ITagCloudOptions
    {
        public IImageOptions ImageOptions { get; init; }
        public FontOptions FontOptions { get; init; }
        public IFilterOptions FilterOptions { get; init; }
        public string FilePath { get; init; }

        public TagCloudOptions(IImageOptions imageOptions, FontOptions fontOptions, IFilterOptions filterOptions, string filePath)
        {
            ImageOptions = imageOptions;
            FontOptions = fontOptions;
            FilterOptions = filterOptions;
            FilePath = filePath;
        }
    }
}
