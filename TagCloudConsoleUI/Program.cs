using System;
using System.Drawing;
using CommandLine;
using TagCloud;
using System.Linq;
using TagCloud.settings;

namespace TagCloudConsoleUI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = TagCloudBuilder.Create();
            while (!args.Contains("exit"))
            {
                Parser.Default.ParseArguments<TagOptions, DrawOptions, RunOptions>(args)
                    .MapResult<TagOptions, DrawOptions, RunOptions, TagCloudBuilder>(
                        o => builder.SetTagSettings(new TagSettings(o.FontFamily, o.FontSize)),
                        o => builder.SetDrawSettings(
                            new DrawSettings(o.InnerColors.ToList(), o.BackgroundColor, new Size(o.Width, o.Height))
                        ),
                        o =>
                        {
                            var result = builder
                                .SetInputFile(o.InputFilePath)
                                .SetOutputFile(o.OutputFilePath)
                                .Build()
                                .Run();
                            Console.WriteLine(result.IsSuccess ? "Красота" : $"Что-то пошло не по плану: {result.Error}");
                            return builder;
                        },
                errors => null!);
                args = Console.ReadLine()!.Split();
            }
        }
    }
}