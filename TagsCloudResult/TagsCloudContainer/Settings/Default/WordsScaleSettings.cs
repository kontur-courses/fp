using TagsCloudContainer.DependencyInjection;
using TagsCloudContainer.MathFunctions;

namespace TagsCloudContainer.Settings.Default
{
    public class WordsScaleSettings : IWordsScaleSettings
    {
        public IMathFunction Function { get; set; }

        public WordsScaleSettings(IServiceResolver<MathFunctionType, IMathFunction> resolver)
        {
            Function = resolver.GetService(MathFunctionType.Linear);
        }
    }
}