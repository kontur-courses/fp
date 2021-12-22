using System.Drawing;
using System.Globalization;
using Autofac;
using Autofac.Core;
using TagsCloudContainerCore.CircularLayouter;
using TagsCloudContainerCore.Helpers;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.LayoutSettingsDir;
using TagsCloudContainerCore.Result;
using TagsCloudContainerCore.StatisticMaker;
using TagsCloudContainerCore.TagCloudMaker;
using TagsCloudContainerCore.WordFilter;
using WinCloudLayouterConsoleUI.WindowsDependencies;

namespace WinCloudLayouterConsoleUI.DI;

// ReSharper disable once InconsistentNaming
internal static class DICloudLayouterContainerFactory
{
    public static Result<IContainer> GetContainer(LayoutSettings settings)
    {
        var backgroundColor = int.Parse("FF" + settings.BackgroundColor, NumberStyles.HexNumber);
        var excludedWordResult = FileReaderHelper
            .ReadLinesFromFile(settings.PathToExcludedWords, true);
        if (!excludedWordResult.IsSuccess)
        {
            return ResultExtension.Fail<IContainer>(excludedWordResult.Error);
        }

        var excludedWords = FileReaderHelper
            .ReadLinesFromFile(settings.PathToExcludedWords, true)
            .GetValueOrThrow()
            .SelectMany(line => line.Split());

        var builder = new ContainerBuilder();
        try
        {
            builder.RegisterInstance(settings).As<LayoutSettings>();

            builder.RegisterType<WinPainter>().As<IPainter>()
                .WithParameter(new NamedParameter("backgroundColorHex", backgroundColor));


            builder.RegisterType<ExcludeWordFilter>().As<IWordSelector>()
                .WithParameter(new PositionalParameter(0, excludedWords));

            builder.RegisterType<CircularCloudLayouter>().As<ILayouter>()
                .WithParameters(new List<Parameter>
                {
                    new PositionalParameter(0, Point.Empty),
                });

            builder.RegisterType<WinFontChecker>().As<IFontChecker>();
            builder.RegisterType<WinSaver>().As<IBitmapHandler>();
            builder.RegisterType<TagStatisticMaker>().As<IStatisticMaker>();
            builder.RegisterType<WinTagMaker>().As<ITagMaker>();
            builder.RegisterType<TagCloudMaker>().As<ITagCloudMaker>();
            return ResultExtension.Ok(builder.Build());
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<IContainer>($"{e.GetType().Name} {e.Message}");
        }
    }
}