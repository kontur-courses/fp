using TagsCloudContainer.DependencyInjection;
using TagsCloudContainer.MathFunctions;
using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudContainer.Settings
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