using System.Drawing;
using Autofac;
using CommandLine;
using ConsoleClient;
using FluentResults;
using TagCloud;
using TagCloud.Abstractions;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(options =>
    {
        var result = DrawerSettings.Create(new Size(options.ImageWidth, options.ImageHeight),
            options.MinFontSize, options.MaxFontSize,
            options.TextColorName,
            options.BackgroundColorName,
            options.FontFamilyName);
        if (result.IsSuccess)
        {
            var settings = result.Value;
            var client = ConfigureContainer(options, settings).Resolve<Client>();
            result = client.Execute(options.Result);
        }

        PrintResult(result);
    });

void PrintResult(IResultBase result)
{
    ConsoleColor color;
    IEnumerable<IReason> reasons;
    if (result.IsFailed)
    {
        color = ConsoleColor.Red;
        reasons = result.Errors;
    }
    else
    {
        color = ConsoleColor.Green;
        reasons = result.Successes;
    }

    Console.ForegroundColor = color;
    Console.WriteLine(string.Join(Environment.NewLine, reasons.Select(e => e.Message)));
    Console.ForegroundColor = default;
}


IContainer ConfigureContainer(Options options, DrawerSettings settings)
{
    var builder = new ContainerBuilder();

    builder.RegisterInstance(new TxtLinesWordsLoader(options.Source)).As<IWordsLoader>();

    ConfigureProcessors(builder, options);

    builder.RegisterType<CountWordsTagger>().As<IWordsTagger>().SingleInstance();

    ConfigureDrawer(builder, settings);

    ConfigureLayouter(builder, options);

    builder.RegisterType<DrawingCloudCreator>().As<ICloudCreator>();

    builder.RegisterType<Client>().AsSelf().SingleInstance();

    return builder.Build();
}

void ConfigureProcessors(ContainerBuilder builder, Options options)
{
    var trimToLowerProcessor = new FuncWordsProcessor(words => words.Select(w => w.Trim().ToLower()));
    builder.RegisterInstance(trimToLowerProcessor).As<IWordsProcessor>();

    builder.RegisterInstance(new MorphWordsProcessor(options.SelectedPartsOfSpeech)).As<IWordsProcessor>();
}

void ConfigureDrawer(ContainerBuilder builder, DrawerSettings settings)
{
    builder.RegisterInstance(settings).AsSelf();
    builder.RegisterType<BaseCloudDrawer>().As<ICloudDrawer>();
}

void ConfigureLayouter(ContainerBuilder builder, Options options)
{
    var pointGenerator = new SpiralPointGenerator(0.1, 0.1, options.XFlattening);
    var center = new Point(options.ImageWidth / 2, options.ImageHeight / 2);
    builder.RegisterInstance(new BaseCloudLayouter(center, pointGenerator)).As<ICloudLayouter>();
}