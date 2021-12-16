using System;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using TagsCloudApp.RenderCommand;
using TagsCloudContainer;

namespace TagsCloudApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<RenderArgs>(args).Value;
            if (options == null)
                return;

            var render = new ServicesProvider(options)
                .BuildProvider()
                .GetRequiredService<RenderAction>();

            render.Perform()
                .OnFail(Console.WriteLine);
        }
    }
}