using CommandLine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using TagsCloudContainer;
using TagsCloudContainer.ClientsInterfaces;
using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.PaintConfigs;
using TagsCloudContainer.TextParsers;

namespace CLI
{
    public class Client : IClient
    {
        private readonly Dictionary<string, Func<string, string>> modifiers;
        private readonly string[] args;
        public IUserConfig UserConfig { get; }

        public Client(string[] args)
        {
            this.args = args;
            modifiers = new Dictionary<string, Func<string, string>>
            {
                {"lower", s => s.ToLower()},
                {"trim", s => s.Trim()}
            };
            UserConfig = ParseArguments().GetValueOrThrow();
        }

        private Result<CommandLineConfig> ParseArguments()
        {
            var config = new Result<CommandLineConfig>();
            var result = Parser.Default.ParseArguments<Options>(args);
            result.WithParsed(options => config = GetConfigResult(options))
                .WithNotParsed(errs => throw new Exception($"Failed with errors:\n{string.Join("\n", errs)}"));

            return config;
        }

        private Result<CommandLineConfig> GetConfigResult(Options options)
        {
            var config = new CommandLineConfig();
            return config.AsResult()
                .Then(UseOutputPathFrom, options)
                .Then(UseImageSizeFrom, options)
                .Then(BuildImageCenter)
                .Then(UseFontParamsFrom, options)
                .Then(UseColorSchemeFrom, options)
                .Then(UseSpiralFrom, options)
                .Then(UseImageFormatFrom, options)
                .Then(UseTextFormatFrom, options)
                .Then(UseSourceReaderFrom, options)
                .Then(UseHandlerConveyorFrom, options)
                .Then(BuildTextParser);
        }
    }
