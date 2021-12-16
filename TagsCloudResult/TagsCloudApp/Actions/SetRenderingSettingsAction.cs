using System.Drawing;
using TagsCloudApp.Parsers;
using TagsCloudApp.RenderCommand;
using TagsCloudContainer;
using TagsCloudContainer.Settings;
using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudApp.Actions
{
    public class SetRenderingSettingsAction : IAction
    {
        private IRenderArgs renderArgs;
        private IArgbColorParser colorParser;
        private IRenderingSettings settings;

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
            return SetBackgroundColor();
        }

        private Result<None> SetBackgroundColor()
        {
            return colorParser.TryParse(renderArgs.BackgroundColor)
                .Then(color =>
                {
                    settings.Background = new SolidBrush(color);
                });
        }
    }
}