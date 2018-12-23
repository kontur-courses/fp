using System.Collections.Generic;
using System.Linq;
using Autofac;
using CloodLayouter.App.Handlers;
using CloodLayouter.Infrastructer;
using CloudLayouter.App;
using CommandLine;
using ResultOf;

namespace CloodLayouter.App
{
    public class DIBilder
    {
        public Result<IContainer> Bild(ParserResult<Options> parserResult)
        {
            if (parserResult.AsResult().IsSuccess)
            {
                var logicBuilder = new ContainerBuilder();

                parserResult.WithParsed(opt =>
                    logicBuilder.Register(x => new FileWordProvider(opt.InputFiles.ToArray()))
                        .As<IProvider<IEnumerable<Result<string>>>>().SingleInstance());

                logicBuilder.RegisterType<WordSelector>()
                    .As<IConverter<IEnumerable<Result<string>>, IEnumerable<Result<string>>>>().SingleInstance();
                logicBuilder.RegisterType<FromWordToTagConverter>()
                    .As<IConverter<IEnumerable<Result<string>>, IEnumerable<Result<Tag>>>>().SingleInstance();
                logicBuilder.RegisterType<ConvertPerfomer>().As<IProvider<IEnumerable<Result<Tag>>>>().SingleInstance();
                logicBuilder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>().SingleInstance();


                parserResult.WithParsed(opt =>
                    logicBuilder.Register(x => new ImageSettings(opt.Width, opt.Heigth)).AsSelf().SingleInstance());

                logicBuilder.RegisterType<TagCloudDrawer>().As<IDrawer>().SingleInstance();
                logicBuilder.RegisterType<ImageSaver>().As<IImageSaver>().SingleInstance();

                return Result.Of(() => logicBuilder.Build());
            }

            return Result.Fail<IContainer>("Can't build the container.");
        }
    }
}