using CommandLine;
using System.Collections.Generic;
using System.IO;

namespace CLI
{
    public class Options
    {
        [Option("input", Default = null,
            HelpText = "set path to input file " +
                       "(in other case you should list words after --words key)")]
        public string Input { get; set; }

        [Option("words", Default = null,
            HelpText = "set list of words to be paint in the cloud " +
                       "(in other case you should set file path after --input key)")]
        public IEnumerable<string> Tags { get; set; }

        [Option('o', "output", 
            HelpText = "set path to output file directory (application local directory by default)")]
        public string OutputFilePath { get; set; } = Directory.GetCurrentDirectory();

        [Option('p', "paintname", Default = "tagcloud",
            HelpText = "set path to output file directory (application local directory by default)")]
        public string OutputFileName { get; set; }

        [Option('w', "width", Default = 1000, HelpText = "tag cloud image width")]
        public int Width { get; set; }

        [Option('h', "height", Default = 1000, HelpText = "tag cloud image height")]
        public int Height { get; set; }

        [Option('n', "fontName", Default = "Arial", HelpText = "tags fontName")]
        public string FontName { get; set; }

        [Option('s', "fontSize", Default = 20, HelpText = "tags fontSize")]
        public int FontSize { get; set; }

        [Option('c', "color", Default = 0, HelpText =
            "tag's color scheme (0 - Black and White, 1 - Camouflage, 2 - Cyberpunk")]
        public int Color { get; set; }

        [Option('f', "inputformat", Default = "txt", HelpText = "input file format")]
        public string InputFileFormat { get; set; }

        [Option('i', "outputformat", Default = "png", HelpText = "output file format")]
        public string OutputFileFormat { get; set; }

        [Option('r', "spiral", Default = "log", HelpText =
            "tag cloud spiral form (log for logarithm, sqr for square and rnd for random)")]
        public string Spiral { get; set; }

        [Option('m', "mod", Default = new[] { "lower", "trim" },
            HelpText = "enumerates string functions which will be apply to all tags\n" +
                       "lower - ToLower(), trim - Trim()")]
        public IEnumerable<string> Modifications { get; set; }

        [Option('e', "exclude", Default = null,
            HelpText = "words that will be excluded from parsing result")]
        public IEnumerable<string> ExcludedWords { get; set; }
    }
}