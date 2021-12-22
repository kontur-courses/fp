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
            //generatorHelper.RegisterTypes(options);
            InnerCoreLogic core2 = null;
            //var core = builder.Build().Resolve<InnerCoreLogic>();
            //core.Run(options);
            return generatorHelper.RegisterTypes(options)
                .Then(x => core2 = builder.Build().Resolve<InnerCoreLogic>())
                .Then(x => core2.Run(options));
            //return Result.Ok();
            //return generatorHelper.RegisterTypes(options).Then(x => core.Run(options));
        }
    }
}
