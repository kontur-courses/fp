using System;
using Autofac;
using TagsCloudContainer.Models;
using edu.stanford.nlp.tagger.maxent;

namespace TagsCloudContainer
{
    public class Program
    { 
        private static ILogger logger = new ConsoleLogger();

        public static void Main(string[] args)
        {
            var userHandler = new ConsoleUserHandler(args);
            var inputInfoResult = userHandler.GetInputInfo();
            if (!inputInfoResult.IsSuccess)
            {
                logger.LogOut(inputInfoResult.Error);
                return;
            }
            var container = GetInjectionContainer(inputInfoResult.Value);
            if (!container.IsSuccess)
            {
                logger.LogOut(container.Error);
                return;
            }
            var programResult = ExecuteProgram(container.Value, inputInfoResult.Value);
            if (!programResult.IsSuccess)
                logger.LogOut(programResult.Error);
        }

        private static Result<IContainer> GetInjectionContainer(InputInfo inputInfo)
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterInstance(logger).As<ILogger>();
                builder.RegisterInstance(new OnlyNounDullWordsEliminator())
                    .As<IDullWordsEliminator>();
                builder.RegisterInstance(new TextFileReader(inputInfo.TextFileName)).As<ITextReader>();
                builder.RegisterType<TextHandler>().AsSelf();
                builder.RegisterType<DefaultTagCloudBuildingAlgorithm>().As<ITagCloudBuildingAlgorithm>();
                builder.RegisterType<TagCloudBuilder>().As<ITagCloudBuilder>();
                builder.RegisterInstance(new PictureInfo(inputInfo.ImageFileName, inputInfo.ImageFormat)).AsSelf();
                builder.RegisterType<DefaultTagsPaintingAlgorithm>().As<ITagsPaintingAlgorithm>();
                builder.RegisterInstance(new CircularTagsCloudLayouter()).As<ITagsLayouter>();
                builder.RegisterType<TagCloudDrawer>().AsSelf();
                return Result.Ok(builder.Build());
            }
            catch (TagCloudException e)
            {
                return Result.Fail<IContainer>(e.Message);
            }
            catch (Exception e)
            {
                return Result.Fail<IContainer>("Program inner exception:\n" + e.Message);
            }
        }

        private static Result<None> ExecuteProgram(IContainer container, InputInfo inputInfo)
        {
            try
            {
                var drawer = container.Resolve<TagCloudDrawer>();
                var drawingResult = drawer.DrawTagCloud(inputInfo.MaxWordsCnt);
                if (!drawingResult.IsSuccess)
                    return Result.Fail<None>(drawingResult.Error);
                return new Result<None>();
            }
            catch (TagCloudException e)
            {
                return Result.Fail<None>(e.Message);
            }
            catch (Exception e)
            {
                return Result.Fail<None>("Program inner exception:\n" + e.Message);
            }
        }
    }
}