using System;
using Autofac;
using CloodLayouter.App;
using CloodLayouter.Infrastructer;
using CommandLine;
using ResultOf;

namespace CloodLayouter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var parserResult = Parser.Default.ParseArguments<Options>(args);
            new DIBilder()
                .Bild(parserResult)
                .Then(x => x.Resolve<IImageSaver>())
                .Then(x => x.Save(parserResult))
                .OnFail(t => Console.WriteLine(t));
        }
    }
}