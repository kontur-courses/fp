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
                vizInfo = null;
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
                var textResut = reader.GetValueOrThrow().ReadText(answear);
                if (!textResut.IsSuccess)
                {
                    Console.WriteLine(textResut.Error);
                    continue;
                }
                var text = textResut.GetValueOrThrow();
                if (text == null)
                {
                    Console.WriteLine("something was wrong, please, try again");
                    continue;
                }
                if (!ReadTagInfoIsSuccess())
                    continue;
                if (!ReadVisualizationInfoIsSuccess())
                    continue;
                Console.WriteLine("write path file to save");
                answear = Console.ReadLine();
                Visualize(text, answear);
                Console.WriteLine("file save");
            }
        }

        private bool ReadTagInfoIsSuccess()
        {
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
            if (!processor.IsSuccess)
            {
                Console.WriteLine(processor.Error);
                return false;
            }
            if (!metric.IsSuccess)
            {
                Console.WriteLine(metric.Error);
                return false;
            }
            if (!pointGetter.IsSuccess)
            {
                Console.WriteLine(pointGetter.Error);
                return false;
            }
            if (!layoter.IsSuccess)
            {
                Console.WriteLine(layoter.Error);
                return false;
            }
            layoter.GetValueOrThrow().SetPointGetterIfNull(pointGetter.GetValueOrThrow());
            return true;
        }

        private bool ReadVisualizationInfoIsSuccess()
        {
            Console.WriteLine("write Visualizate configuration (3 parametrs - size, coloring and font)");
            var answears = new List<string>()
            {
                Console.ReadLine(),
                Console.ReadLine(),
                Console.ReadLine()
            };
            Parser.Default.ParseArguments<OptionsVisualizate>(answears)
                .WithParsed(SetVisualizateInfo);
            return vizInfo != null;
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
            if(!TryGetValueOrWriteError(VisualizationInfo.ReadSize(SkipSpaces(info.Size)), out var size))
                return;
            if (TryGetValueOrWriteError(WordsColoringAssosiation.GetColoring(SkipSpaces(info.Coloring)), out var coloring))
                return;
            string font = SkipSpaces(info.Font);
            if (!VisualizationInfo.FontIsCorrect(font))
            {
                Console.WriteLine("Not correct font");
                return;
            }
            vizInfo = new VisualizationInfo(coloring, size, font);
        }

        private bool TryGetValueOrWriteError<T>(Result<T> result, out T value)
        {
            if (!result.IsSuccess)
            {
                Console.WriteLine(result.Error);
                value = default;
            }
            else
                value = result.GetValueOrThrow();
            return result.IsSuccess;
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
                HelpText = "size to cut picture, write two numbers or \"dynamic\" for dynamic size")]
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
