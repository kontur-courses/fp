using TagsCloudContainer.ColorMappers;
using TagsCloudContainer.DependencyInjection;

namespace TagsCloudContainer.Settings.Default
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