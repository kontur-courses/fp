using System.Drawing;
using Autofac;
using TagsCloudGenerator;
using TagsCloudGenerator.CloudPrepossessing;
using TagsCloudGenerator.ShapeGenerator;

namespace TagsCloudConsoleUI.DIPresetModules
{
    internal class CircularRandomCloudModule : DiPreset
    {
        private readonly Color backgroundColor;
        private readonly Color[] colorsPalette;
        private readonly Point center;
        private readonly float spiralStep;

        public CircularRandomCloudModule(BuildOptions options) : base(options)
        {
            backgroundColor = options.BackgroundColor;
            colorsPalette = options.ColorsPalette;
            center = new Point(options.Width / 2, options.Height / 2);
            spiralStep = options.SpiralStep;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RandomTagOrder>().As<ITagOrder>();
            builder.RegisterType<FirstBigLetterPreform>().As<ITagTextPreform>();
            builder.RegisterType<RandomlyCloudPainter>().As<ICloudColorPainter>()
                .WithParameters(new[]{
                    new NamedParameter("colorsPalette", colorsPalette),
                    new NamedParameter("backgroundColor", backgroundColor)
                });

            builder.RegisterType<CircularCloudPrepossessing>().As<ITagsPrepossessing>()
                .WithParameter(new NamedParameter("center", center));

            builder.RegisterType<ArchimedeanShape>().As<IShapeGenerator>()
                .WithParameters(new[]
                {
                    new NamedParameter("center", center),
                    new NamedParameter("spiralStep", spiralStep),
                });
        }
    }
}