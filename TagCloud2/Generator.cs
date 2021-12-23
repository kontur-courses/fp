using System.Drawing;
using TagCloud2.Image;
using TagCloud2.Text;
using TagCloud2.TextGeometry;
using TagCloudVisualisation;
using Autofac;
using ResultOf;

namespace TagCloud2
{
    public class Generator
    {
        public Result<None> Generate(IOptions options)
        {
            var builder = new ContainerBuilder();
            var generatorHelper = new GeneratorHelper(builder);
            builder.RegisterType<InnerCoreLogic>().AsSelf();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            if (options.AngleSpeed == 0 || options.LinearSpeed == 0)
            {
                if (options.Path != null)
                {
                    return Result.Fail<None>("angle or linear speed is zero");
                }
                else
                {
                    return Result.Fail<None>("arguments are incorrect");
                }
            }

            var spiral = new ArchimedeanSpiral(new Point(options.X/2, options.Y/2), options.AngleSpeed, options.LinearSpeed);
            builder.RegisterInstance(spiral).As<ISpiral>();
            builder.RegisterType<LinesWordReader>().As<IWordReader>();
            builder.RegisterType<StringPreprocessor>().As<IStringPreprocessor>();
            builder.RegisterType<StringToRectangleConverter>().As<IStringToSizeConverter>();
            builder.RegisterType<ColoredCloud>().As<IColoredCloud>();
            builder.RegisterType<FileGenerator>().As<IFileGenerator>();
            builder.RegisterType<ColoredCloudToBitmap>().As<IColoredCloudToImageConverter>();
            builder.RegisterType<SillyWordsFilter>().As<ISillyWordsFilter>();
            builder.RegisterInstance(new ExcludedWordsPath() { Path = options.ExcludePath }).As<ExcludedWordsPath>();
            return generatorHelper.RegisterTypes(options)
                .Then(x => builder.Build().Resolve<InnerCoreLogic>())
                .Then(x => x.Run(options));
        }
    }
}
