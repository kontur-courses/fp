using TagsCloudContainer.ColorMappers;

namespace TagsCloudContainer.Settings.Interfaces
{
    public interface IWordColorMapperSettings
    {
        IWordColorMapper ColorMapper { get; set; }
    }
}