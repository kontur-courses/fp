using System.ComponentModel;
using System.Drawing;
using CommandLine;

namespace TagsCloud.ConsoleOptions;

public class ConsoleArguments
{
    [Option('i', "inputfile", Default = "C:\\Users\\hellw\\Desktop\\C#\\di\\TagsCloud\\source\\input.txt", HelpText = "Set input file name.")]
    public string InputFileName { get; set; }

    [Option('o', "outputfile", Default = "C:\\Users\\hellw\\Desktop\\C#\\fp\\TagsCloud\\source\\output.png", HelpText = "Set output file name.")]
    public string OutputFileName { get; set; }

    [Option('f', "fontname", Default = "Arial", HelpText = "Set tagsCloud word Font")]
    public string FontName { get; set; }

    [Option('w', "width", Default = "0", HelpText = "Set output image width")]
    public string ImageWidth { get; set; }

    [Option('h', "height", Default = "0", HelpText = "Set output image height")]
    public string ImageHeight { get; set; }
    
    [Option('x', "centerX", Default = "0", HelpText = "Set layouter center x position")]
    public string CenterX { get; set; }

    [Option('y', "centerY", Default = "0", HelpText = "Set layouter center y position")]
    public string CenterY { get; set; }

    [Option('b', "background", Default = "White", HelpText = "Set tagsCloud output image background color")]
    public string BackgroundColor { get; set; }
}