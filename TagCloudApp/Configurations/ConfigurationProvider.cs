using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CommandLine;
using TagCloud;
using TagCloud.PointGenerator;
using TagCloud.Templates.Colors;

namespace TagCloudApp.Configurations
{
    public static class CommandLineConfigurationProvider
    {
        public static Result<Configuration> GetConfiguration(IEnumerable<string> args)
        {
            return args.AsResult()
                .Then(a => Parser.Default.ParseArguments<Options>(a).Value)
                .Validate(o => o != null)
                .Then(CheckArguments)
                .Then(CreateConfiguration);
        }


        private static Options CheckArguments(Options arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentException("Invalid arguments");
            }

            if (!File.Exists(arguments.Filename))
            {
                throw new FileNotFoundException($"File {arguments.Filename} does not exist!");
            }

            return arguments;
        }

        private static Configuration CreateConfiguration(Options arguments)
        {
            var configuration = new Configuration(arguments.Filename, arguments.Output)
            {
                ImageSize = new Size(arguments.Width, arguments.Height),
                BackgroundColor = arguments.BackgroundColor
            };
            if (arguments.Color != Color.Empty)
                configuration.ColorGenerator = new OneColorGenerator(arguments.Color);
            if (arguments.FontFamily != null)
                configuration.FontFamily = arguments.FontFamily;
            if (arguments.CloudForm != null)
                configuration.PointGenerator = arguments.CloudForm.ToLower() switch
                {
                    "spiral" => Spiral.GetDefaultSpiral(),
                    "circle" => Circle.GetDefault(),
                    _ => Circle.GetDefault()
                };
            return configuration;
        }
    }
}