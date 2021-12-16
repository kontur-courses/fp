using TagsCloudApp.Parsers;
using TagsCloudApp.Settings;
using TagsCloudContainer.Results;

namespace TagsCloudApp.Actions
{
    public class SetSaveSettingsAction : IAction
    {
        private readonly IRenderArgs renderArgs;
        private readonly IImageFormatParser parser;
        private readonly ISaveSettings settings;

        public SetSaveSettingsAction(IRenderArgs renderArgs, IImageFormatParser parser, ISaveSettings settings)
        {
            this.renderArgs = renderArgs;
            this.parser = parser;
            this.settings = settings;
        }

        public Result<None> Perform()
        {
            settings.OutputFile = renderArgs.OutputPath;
            return parser.Parse(renderArgs.ImageFormat)
                .Then(format => { settings.ImageFormat = format; });
        }
    }
}