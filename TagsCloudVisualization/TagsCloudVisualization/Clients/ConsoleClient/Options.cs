﻿using System.ComponentModel.DataAnnotations;
using System.Drawing;
using CommandLine;

namespace TagsCloudVisualization.Clients;

public class Options
{
    [Option('p', "path", Required = false, Default = Config.DefaultPath, HelpText = "Path to input txt.")]
    public string Path { get; set; }

    [Option('w', "width", Required = false, Default = Config.WindowWidth, HelpText = "Image width")]
    [Range(1, int.MaxValue)]
    public int ImageWidth { get; set; }

    [Option('h', "height", Required = false, Default = Config.WindowHeight, HelpText = "Image height")]
    [Range(1, int.MaxValue)]
    public int ImageHeight { get; set; }

    [Option('c', "colors", Required = false, HelpText = "Colors")]
    public IEnumerable<Color> Colors { get; set; }

    [Option('f', "font", Required = false, Default = Config.DefaultFont, HelpText = "Font")]
    [Range(1, int.MaxValue)]
    public string Font { get; set; }

    [Option('s', "font-size", Required = false, Default = Config.DefaultFontSize, HelpText = "Font size")]
    [Range(1, int.MaxValue)]
    public int FontSize { get; set; }
}