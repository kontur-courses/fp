﻿using CommandLine;

namespace TagsCloud.CLI;

public class Options
{
	[Option('w', "words", Required = true, HelpText = "Path to file with words")]
	public string PathToWordsFile { get; set; }

	[Option('s', "save", Required = true, HelpText = "Path for saving image")]
	public string PathToImage { get; set; }

	[Option("excluding", Required = false, HelpText = "Path to file with words for excluding")]
	public string? PathToExcludedWords { get; set; }

	[Option("layouter", Required = false, Default = "circle", HelpText = "Layouter name")]
	public string Layouter { get; set; }

	[Option("use-random-color", Required = false, Default = false, HelpText = "Use random color for each word")]
	public bool UseRandomColor { get; set; }

	[Option("font-color", Required = false, Default = "BlueViolet", HelpText = "Font color name")]
	public string FontColor { get; set; }

	[Option("background-color", Required = false, Default = "AliceBlue", HelpText = "Background color name")]
	public string BackgroundColor { get; set; }

	[Option("font-size", Required = false, Default = 14, HelpText = "Font size for smallest word")]
	public int MinFontSize { get; set; }

	[Option("font-name", Required = false, Default = "Arial", HelpText = "Font family name")]
	public string FontName { get; set; }

	[Option("width", Required = false, Default = 1000, HelpText = "Image width")]
	public int ImageWidth { get; set; }

	[Option("height", Required = false, Default = 1000, HelpText = "Image height")]
	public int ImageHeight { get; set; }

	[Option("use-auto-size", Required = false, Default = false, HelpText = "Use auto size")]
	public bool UseAutoSize { get; set; }
}