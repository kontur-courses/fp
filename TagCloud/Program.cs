using System;
using Autofac;
using TagCloud.App;
using TagCloud.AppConfiguration;
using TagCloud.ResultMonade;

namespace TagCloud
{
    public class Program
    {
        static void Main(string[] args)
        {

            args = "-i C:\\VIKTOR\\Kontur\\10_FP\\Text.txt -o C:\\VIKTOR\\Kontur\\10_FP\\TagCloudImages\\Output.png -z ellipse -w 800 -h 400 -b white -f arial -l 5 -p 40 -k random".Split(' ');

            new ConsoleAppConfigProvider().GetAppConfig(args)
                                          .Then(config => ContainerConfig.Configure(config))
                                          .Then(container => container.Resolve<IApp>())
                                          .Then(app => app.Run())
                                          .RefineError("OMG! Application failed")
                                          .OnFail(Console.WriteLine);
        }
    }
}

