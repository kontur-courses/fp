using TagsCloudApp.Parsers;
using TagsCloudContainer.DependencyInjection;
using TagsCloudContainer.MathFunctions;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

namespace TagsCloudApp.Actions
{
    public class SetWordsScaleFunctionAction : IAction
    {
        private readonly IRenderArgs renderArgs;
        private readonly IEnumParser enumParser;
        private readonly IServiceResolver<MathFunctionType, IMathFunction> resolver;
        private readonly IWordsScaleSettings settings;

        public SetWordsScaleFunctionAction(
            IEnumParser enumParser,
            IServiceResolver<MathFunctionType, IMathFunction> resolver,
            IWordsScaleSettings settings,
            IRenderArgs renderArgs)
        {
            this.enumParser = enumParser;
            this.resolver = resolver;
            this.settings = settings;
            this.renderArgs = renderArgs;
        }

        public Result<None> Perform()
        {
            return enumParser.Parse<MathFunctionType>(renderArgs.WordsScale)
                .Then(type =>
                {
                    settings.Function = resolver.GetService(type);
                });
        }
    }
}