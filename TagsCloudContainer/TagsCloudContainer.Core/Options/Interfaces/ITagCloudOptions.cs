namespace TagsCloudContainer.Core.Options.Interfaces
{
    public interface ITagCloudOptions
    {
        public IImageOptions ImageOptions { get; }

        public FontOptions FontOptions { get; }

        public IFilterOptions FilterOptions { get; }

        public string FilePath { get; }
    }
}
