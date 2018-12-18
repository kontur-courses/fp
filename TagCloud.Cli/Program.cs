using System;
using TagCloud;
using TagCloud.Result;

namespace TagCloudCreator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string inputFile = null;
            string outputFile = null;
            var result = Configuration.FromArguments(args)
                .Then(config =>
                {
                    inputFile = config.InputFile;
                    outputFile = config.OutputFile;
                    return config;
                })
                .Then(ContainerBuilder.ConfigureContainer)
                .Then(c => c.Resolve<Application>())
                .Then(app => app.Run(inputFile, outputFile));
            result.OnFail(error =>
            {
                Console.WriteLine(error);
                Console.ReadKey();
            });
        }
    }
}