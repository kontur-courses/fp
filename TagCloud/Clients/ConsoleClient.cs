using System;
using System.Linq;
using System.Collections.Generic;
using CommandLine;
using TagCloud.TextConverters.TextReaders;
using TagCloud.TextConverters.TextProcessors;
using TagCloud.WordsMetrics;
using TagCloud.Visualization;
using TagCloud.Visualization.WordsColorings;
using TagCloud.CloudLayoters;
using TagCloud.PointGetters;
using ResultOf;

namespace TagCloud.Clients
{
    internal class ConsoleClient : IClient
    {
        private Result<ITextProcessor> processor;
        private Result<IWordsMetric> metric;
        private Result<ICloudLayoter> layoter;
        private Result<IPointGetter> pointGetter;

        private VisualizationInfo vizInfo;

        public void Run()
        {
            Console.WriteLine("Hello, I'm your personal visualization client");
            while (true)
            {
                Console.WriteLine("Write path to file with text or \"exit\" to exit");
                var answear = Console.ReadLine();
                if (answear == "exit")
                    break;
                var reader = TextReaderAssosiation.GetTextReader(answear);
                if(!reader.IsSuccess)
                {
                    Console.WriteLine(reader.Error);
                    continue;
                }
                var text = reader.GetValueOrThrow().ReadText(answear);
                if (text == null)
                {
                    Console.WriteLine("something was wrong, please, try again");
                    continue;
                }
                Console.WriteLine("write params cloud configuration");
                var answears = new List<string>() 
                { 
                    Console.ReadLine(), 
                    Console.ReadLine(), 
                    Console.ReadLine(), 
                    Console.ReadLine() 
                };
                Parser.Default.ParseArguments<OptionsTagInfo>(answears)
                    .WithParsed(SetTagInfo);
                if(!processor.IsSuccess)
                {
                    Console.WriteLine(processor.Error);
                    continue;
                }
                if (!metric.IsSuccess)
                {
                    Console.WriteLine(metric.Error);
                    continue;
                }
                if (!pointGetter.IsSuccess)
                {
                    Console.WriteLine(pointGetter.Error);
                    continue;
                }
                if (!layoter.IsSuccess)
                {
                    Console.WriteLine(layoter.Error);
                    continue;
                }
                layoter.GetValueOrThrow().SetPointGetterIfNull(pointGetter.GetValueOrThrow());
                Console.WriteLine("write Visualizate configuration or 'end' if you finish write");
                answears.Clear();
                while (true)
                {
                    answear = Console.ReadLine();
                    if (answear == "end")
                        break;
                    answears.Add(answear);
                }
                Parser.Default.ParseArguments<OptionsVisualizate>(answears)
                    .WithParsed(SetVisualizateInfo);
                Console.WriteLine("write path file to save");
                answear = Console.ReadLine();
                Visualize(text, answear);
                Console.WriteLine("file save");
            }
        }

        private void SetTagInfo(OptionsTagInfo info)
        {
            processor = TextProcessorAssosiation
                .GetProcessor(SkipSpaces(info.Processor));
            metric = WordsMetricAssosiation
                .GetMetric(SkipSpaces(info.Metric));
            pointGetter = PointGetterAssosiation
                .GetPointGetter(SkipSpaces(info.PointGetter));
            layoter = CloudLayoterAssosiation
                .GetCloudLayoter(SkipSpaces(info.Layoter));
        }

        private void SetVisualizateInfo(OptionsVisualizate info)
        {
            var sizeResult = VisualizationInfo.ReadSize(info.Size);
            if (!sizeResult.IsSuccess)
            {
                Console.WriteLine(sizeResult.Error);
                Console.WriteLine("Was using dynamic size");
            }
            var size = sizeResult.IsSuccess ? sizeResult.GetValueOrThrow() : null;
            var coloringResult = WordsColoringAssosiation.GetColoring(SkipSpaces(info.Coloring));
            if (!coloringResult.IsSuccess)
            {
                Console.WriteLine(coloringResult.Error);
                Console.WriteLine("Was using coloring \"random\"");
            }
            var coloring = coloringResult.IsSuccess ? coloringResult.GetValueOrThrow() :  
                WordsColoringAssosiation.GetColoring("random").GetValueOrThrow();
            string font = SkipSpaces(info.Font);
            if (!VisualizationInfo.FontIsCorrect(font))
            {
                Console.WriteLine("Not correct font, was using fong \"Arial\"");
                font = "Arial";
            }
            vizInfo = new VisualizationInfo(coloring, size, font);
        }

        private string SkipSpaces(string str) => string.Join("", str.SkipWhile(c => c == ' '));

        public void Visualize(string text, string picturePath)
        {
            var tagCloud = AlgorithmTagCloud.GetTagCloud(text, layoter.GetValueOrThrow(), 
                processor.GetValueOrThrow(), metric.GetValueOrThrow());
            TagCloudVisualization.Visualize(tagCloud, picturePath, vizInfo);
        }

        internal class OptionsTagInfo
        {
            [Option('m', "metric", Required = true, HelpText = "Name of metric. Write \"count\"")]
            public string Metric { get; set; }

            [Option('p', "processor", Required = true, 
                HelpText = "type of text processor, write " + TextProcessorAssosiation.paragraph + " or " + TextProcessorAssosiation.words)]
            public string Processor { get; set; }

            [Option('g', "getter", Required = true,
                HelpText = "type of point getter, write " + PointGetterAssosiation.circle + " or " + PointGetterAssosiation.spiral)]
            public string PointGetter { get; set; }

            [Option('l', "layoter", Required = true,
                HelpText = "type of layoter, write " + CloudLayoterAssosiation.density + " or " + CloudLayoterAssosiation.identity)]
            public string Layoter { get; set; }
        }

        internal class OptionsVisualizate
        {
            [Option('s', "size", Required = false, Default = "", 
                HelpText = "size tu cut picture, write two numbers")]
            public string Size { get; set; }

            [Option('f', "font", Required = false, Default = "Arial",
                HelpText = "font of words, write Arial, Calibri, ...")]
            public string Font { get; set; }

            [Option('c', "coloring", Required = false, Default = "random",
                HelpText = "coloring text, write red, geen, blue, black, random, multi, line red, line green, line blue, line random")]
            public string Coloring { get; set; }
        }
    }
}
