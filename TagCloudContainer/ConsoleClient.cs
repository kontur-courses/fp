using System;
using System.Drawing;
using System.Linq;
using CommandLine;
using ResultOfTask;
using TagsCloudPreprocessor;

namespace TagCloudContainer
{
    public class ConsoleClient : IUserInterface
    {
        private readonly string[] args;
        private readonly IWordExcluder wordExcluder;
        private bool toExit = true;

        public ConsoleClient(string[] args, IWordExcluder wordExcluder)
        {
            this.args = args;
            this.wordExcluder = wordExcluder;
            var handleResult = HandleArgs();
            if (!handleResult.IsSuccess)
            {
                Console.WriteLine(handleResult.Error);
                Environment.Exit(0);
            }

            if (toExit)
                Environment.Exit(0);
        }

        public Config Config { get; private set; }

        private Result<Config> GetConfig(SaveOptions opts)
        {
            var center = new Point(500, 500);
            var pathToSave = opts.PathToSave;
            var count = opts.Count;
            if (!FontFamily.Families.Any(x =>
                x.Name.Equals(opts.FontName, StringComparison.InvariantCultureIgnoreCase)))
                return Result.Fail<Config>("Can not parse font name.");
            var font = new Font(opts.FontName, opts.FontSize);
            var fileName = opts.FileName;
            var outPath = opts.OutPath ?? Environment.CurrentDirectory;
            var color = Color.FromName(opts.Color);
            var backColor = Color.FromName(opts.BackgroundColor);
            var imageExtension = opts.ImageExtension;
            var inputExtension = opts.InputExtension;
            return Result.Ok(new Config(
                center,
                pathToSave,
                count,
                font,
                fileName,
                outPath,
                color,
                backColor,
                imageExtension,
                inputExtension));
        }

        private Result<None> HandleArgs()
        {
            var saveOptions = typeof(SaveOptions);
            var excludeOptions = typeof(ExcludeOptions);
            var config = default(Result<Config>);
            var configChanged = false;
            Parser.Default.ParseArguments(args, saveOptions, excludeOptions).WithParsed(
                opts =>
                {
                    if (opts.GetType() == typeof(SaveOptions))
                    {
                        config = GetConfig((SaveOptions) opts);
                        toExit = false;
                        configChanged = true;
                    }
                    else if (opts.GetType() == typeof(ExcludeOptions))
                    {
                        AddExcludingWord(((ExcludeOptions) opts).Word);
                    }
                });

            if (!config.IsSuccess)
                return Result.Fail(config.Error);
            if (configChanged) Config = config.GetValueOrThrow();

            return Result.Ok();
        }

        private void AddExcludingWord(string word)
        {
            wordExcluder.SetExcludedWord(word);
        }

        [Verb("save", HelpText = "Save tag cloud.")]
        private class SaveOptions
        {
            [Option('c', "count", Default = 70, HelpText = "Count of tags in cloud.")]
            public int Count { get; set; }

            [Option("font-name", Default = "Times New Roman", HelpText = "Font name.")]
            public string FontName { get; set; }

            [Option("font-size", Default = 40.0f, HelpText = "Font size in pt.")]
            public float FontSize { get; set; }

            [Option('n', "name", Default = "Cloud", HelpText = "File name.")]
            public string FileName { get; set; }

            [Option("color", Default = "Black", HelpText = "Name of color.")]
            public string Color { get; set; }

            [Option("back-color", Default = "White", HelpText = "Name of background color.")]
            public string BackgroundColor { get; set; }

            [Option("out-path", HelpText = "Path to output directory.")]
            public string OutPath { get; set; }

            [Option("img-ext", Default = "png", HelpText = "Extension of image to save.")]
            public string ImageExtension { get; set; }

            [Option("input-ext", Default = "txt", HelpText = "Extension of input file with text (txt or docx).")]
            public string InputExtension { get; set; }

            [Value(0, Required = true, HelpText = "Path to input file.")]
            public string PathToSave { get; set; }


            //ToDo Выбор разрешения сохраняемого файла
        }

        [Verb("exclude", HelpText = "Exclude word from drawing.")]
        private class ExcludeOptions
        {
            [Value(0, Required = true, HelpText = "Word to exclude.")]
            public string Word { get; set; }

            //ToDo Выбор разрешения сохраняемого файла
        }
    }
}