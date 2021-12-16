using TagsCloudContainer.ColorMappers;
using TagsCloudContainer.DependencyInjection;
using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudContainer.Settings
{
    public class WordColorMapperSettings : IWordColorMapperSettings
    {
        public IWordColorMapper ColorMapper { get; set; }

        public WordColorMapperSettings(
            IServiceResolver<WordColorMapperType, IWordColorMapper> resolver)
        {
            ColorMapper = resolver.GetService(WordColorMapperType.SpeechPart);
        }
    }
}