using TagsCloudApp.Parsers;
using TagsCloudApp.RenderCommand;
using TagsCloudContainer;
using TagsCloudContainer.ColorMappers;
using TagsCloudContainer.DependencyInjection;
using TagsCloudContainer.Settings;
using TagsCloudContainer.Settings.Interfaces;

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
            return enumParser.TryParse<WordColorMapperType>(renderArgs.ColorMapperType)
                .Then(type =>
                {
                    settings.ColorMapper = resolver.GetService(type);
                });
        }
    }
}