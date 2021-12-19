using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Preprocessors;

namespace TagsCloudContainer.Appliers
{
    public class PreprocessorsApplier : IPreprocessorsApplier
    {
        private readonly IPreprocessor[] preprocessors;

        public PreprocessorsApplier(Settings settings)
        {
            preprocessors = settings.Preprocessors;
        }

        public IEnumerable<string> ApplyPreprocessors(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                var processed = word;
                foreach (var preprocessor in preprocessors)
                    processed = preprocessor.Preprocess(processed);

                yield return processed;
            }
        }
    }
}
