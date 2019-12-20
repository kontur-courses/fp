using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using CommandLine;
using TagsCloudContainer.Filters;
using TagsCloudContainer.RectangleGenerator;
using TagsCloudContainer.RectangleGenerator.PointGenerator;
using TagsCloudContainer.TokensGenerator;
using TagsCloudContainer.Visualization;
using YandexMystem.Wrapper.Enums;

namespace TagsCloudContainer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ArgumentParser.ParseArguments(args)
                .WithNotParsed(PrintErrors)
                .WithParsed(Execute);
        }

        private static void Execute(ArgumentParser.Options options)
        {
            var setting = new TagsCloudSetting(options);
            //var setting = TagsCloudSetting.GetDefault();

            var container = BuildContainer(setting);
            var tagCloudVisualizator = container.Resolve<TagCloudVisualizator>();
            Result.Of(() => File.ReadAllText(options.InputFile))
                .Then(text => tagCloudVisualizator.DrawTagCloud(text, setting))
                .Then(img => img.Save(options.OutputFile))
                .Then((_) => Console.WriteLine($"Image save in {options.OutputFile}"))
                .OnFail(Console.WriteLine);
        }

        private static void PrintErrors(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
        }

        private static IContainer BuildContainer(TagsCloudSetting setting)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Visualizer>().As<IVisualizer>();
            builder.RegisterType<Mysteam>().As<IMysteam>().SingleInstance();
            builder.RegisterType<MyStemParser>().As<ITokensParser>().SingleInstance();
            builder.RegisterType<MyStemFilter>().As<IFilter>().SingleInstance()
                .WithParameter("allowedWorldType",
                    new[] {GramPartsEnum.Noun, GramPartsEnum.Verb, GramPartsEnum.Adjective, GramPartsEnum.Adverb});
            builder.Register(c => setting).As<ICloudSetting>().SingleInstance();
            builder.RegisterType<SpiralGenerator>().As<IPointGenerator>()
                .WithParameter("center", setting.GetCenterImage());
            builder.RegisterType<CircularCloudLayouter>().As<IRectangleGenerator>();
            builder.RegisterType<TagCloudVisualizator>().AsSelf();

            return builder.Build();
        }
    }
}