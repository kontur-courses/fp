using TagsCloudApp.Parsers;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

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
            return colorParser.Parse(renderArgs.DefaultColor)
                .Then(color =>
                {
                    settings.Color = color;
                });
        }
    }
}