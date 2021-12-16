using TagsCloudApp.RenderCommand;
using TagsCloudContainer;
using TagsCloudContainer.Settings;
using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudApp.Actions
{
    public class SetFontSizeAction : IAction
    {
        private readonly IRenderArgs renderArgs;
        private readonly IFontSizeSettings settings;

        public SetFontSizeAction(IRenderArgs renderArgs, IFontSizeSettings settings)
        {
            this.renderArgs = renderArgs;
            this.settings = settings;
        }

        public Result<None> Perform()
        {
            return SetMaxFontSize()
                .Then(SetMinFontSize);
        }

        private Result<None> SetMaxFontSize()
        {
            return Validate.Positive(nameof(renderArgs.MaxFontSize), renderArgs.MaxFontSize)
                .Then(max =>
                {
                    settings.MaxFontSize = max;
                });
        }

        private Result<None> SetMinFontSize()
        {
            return Validate.Positive(nameof(renderArgs.MinFontSize), renderArgs.MinFontSize)
                .Then(min =>
                {
                    settings.MinFontSize = min;
                });
        }
    }
}