using System.Drawing;
using Autofac;
using TagsCloudGenerator;

namespace TagsCloudConsoleUI.DIPresetModules
{
    internal class CloudSettingsSetupModule : DiPreset
    {
        private readonly int fontSizeMultiplier;
        private readonly int maximalFontSize;
        private readonly FontFamily textFontFamily;

        public CloudSettingsSetupModule(BuildOptions options) : base(options)
        {
            fontSizeMultiplier = options.FontSizeMultiplier;
            maximalFontSize = options.MaximalFontSize;
            textFontFamily = options.FontFamily;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CloudSettings>().WithParameters(new[]
            {
                new NamedParameter("tagTextFontFamily", textFontFamily),
                new NamedParameter("fontSizeMultiplier", fontSizeMultiplier),
                new NamedParameter("maximalFontSize", maximalFontSize)
            });
        }
    }
}