using System.Drawing;
using TagsCloudApp.Parsers;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

namespace TagsCloudApp.Actions
{
    public class SetRenderingSettingsAction : IAction
    {
        private readonly IRenderArgs renderArgs;
        private readonly IArgbColorParser colorParser;
        private readonly IRenderingSettings settings;

        public SetRenderingSettingsAction(
            IRenderingSettings settings,
            IRenderArgs renderArgs,
            IArgbColorParser colorParser)
        {
            this.settings = settings;
            this.renderArgs = renderArgs;
            this.colorParser = colorParser;
        }

        public Result<None> Perform()
        {
            settings.Scale = renderArgs.ImageScale;
            settings.DesiredImageSize = renderArgs.ImageSize;
            settings.Background.Dispose();
            return SetBackgroundColor();
        }

        private Result<None> SetBackgroundColor()
        {
            return colorParser.Parse(renderArgs.BackgroundColor)
                .Then(color =>
                {
                    settings.Background.Dispose();
                    settings.Background = new SolidBrush(color);
                });
        }
    }
}