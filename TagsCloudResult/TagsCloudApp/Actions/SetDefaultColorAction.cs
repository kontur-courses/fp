using TagsCloudApp.Parsers;
using TagsCloudApp.RenderCommand;
using TagsCloudContainer;
using TagsCloudContainer.Settings;
using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudApp.Actions
{
    public class SetDefaultColorAction : IAction
    {
        private readonly IRenderArgs renderArgs;
        private readonly IArgbColorParser colorParser;
        private readonly IDefaultColorSettings settings;

        public SetDefaultColorAction(
            IRenderArgs renderArgs,
            IArgbColorParser colorParser,
            IDefaultColorSettings settings)
        {
            this.renderArgs = renderArgs;
            this.colorParser = colorParser;
            this.settings = settings;
        }

        public Result<None> Perform()
        {
            return colorParser.TryParse(renderArgs.DefaultColor)
                .Then(color =>
                {
                    settings.Color = color;
                });
        }
    }
}