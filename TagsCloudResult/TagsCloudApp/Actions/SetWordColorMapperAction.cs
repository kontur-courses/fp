using TagsCloudApp.Parsers;
using TagsCloudContainer.ColorMappers;
using TagsCloudContainer.DependencyInjection;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

namespace TagsCloudApp.Actions
{
    public class SetWordColorMapperAction : IAction
    {
        private readonly IRenderArgs renderArgs;
        private readonly IEnumParser enumParser;
        private readonly IServiceResolver<WordColorMapperType, IWordColorMapper> resolver;
        private readonly IWordColorMapperSettings settings;

        public SetWordColorMapperAction(
            IEnumParser enumParser,
            IWordColorMapperSettings settings,
            IServiceResolver<WordColorMapperType, IWordColorMapper> resolver,
            IRenderArgs renderArgs)
        {
            this.renderArgs = renderArgs;
            this.enumParser = enumParser;
            this.resolver = resolver;
            this.settings = settings;
        }

        public Result<None> Perform()
        {
            return enumParser.Parse<WordColorMapperType>(renderArgs.ColorMapperType)
                .Then(type =>
                {
                    settings.ColorMapper = resolver.GetService(type);
                });
        }
    }
}