using System.Drawing;
using System.Globalization;
using Autofac;
using Autofac.Core;
using TagsCloudContainerCore;
using TagsCloudContainerCore.CircularLayouter;
using TagsCloudContainerCore.Helpers;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.StatisticMaker;
using TagsCloudContainerCore.TagCloudMaker;
using TagsCloudContainerCore.WordFilter;
using WinCloudLayouterConsoleUI.WindowsDependencies;

namespace WinCloudLayouterConsoleUI;

// ReSharper disable once InconsistentNaming
public static class DICloudLayouterContainerFactory
{
    public static IContainer GetContainer(LayoutSettings settings)
    {
        var backgroundColor = int.Parse("FF" + settings.BackgroundColor, NumberStyles.HexNumber);
        var excludedWords = FileReaderHelper
            .ReadLinesFromFile(settings.PathToExcludedWords, true)
            .SelectMany(line => line.Split());

        var builder = new ContainerBuilder();

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

        builder.RegisterType<WinSaver>().As<IBitmapHandler>();
        builder.RegisterType<TagStatisticMaker>().As<IStatisticMaker>();
        builder.RegisterType<WinTagMaker>().As<ITagMaker>();
        builder.RegisterType<TagCloudMaker>().As<ITagCloudMaker>();

        return builder.Build();
    }
}