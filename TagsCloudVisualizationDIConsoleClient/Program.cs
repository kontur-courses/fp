using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TagsCloudVisualizationDI;

namespace TagsCloudVisualizationDIConsoleClient
{
    public class Program
    {
        private static readonly Dictionary<string, ImageFormat> ImageFormats= new Dictionary<string, ImageFormat>()
        {
            [".png"] = ImageFormat.Png,
            [".jpeg"] = ImageFormat.Jpeg,
            [".jpg"] = ImageFormat.Jpeg,
            [".bmp"] = ImageFormat.Bmp,
            [".tiff"] = ImageFormat.Tiff,
            [".ico"] = ImageFormat.Icon,
            [".gif"] = ImageFormat.Gif,
        };
        public static void Main(string[] args)
        {
            if (args.Contains("-h") || args.Contains("--help"))
            {
                Console.WriteLine("HELP");
                return;
            }


            if (args.Length < 2 || args.Length > 3)
            {
                var resultPathToFile = Result.Fail<string>($"Incorrect number of arguments ({args.Length})");
                var resultPathWithoutFormat = Result.Fail<string>($"Incorrect number of arguments ({args.Length})");
                var imageFormat = Result.Fail<ImageFormat>($"Incorrect number of arguments ({args.Length})");
                var excludedWordList = Result.Fail<List<string>>($"Incorrect number of arguments ({args.Length})");
                TagsCloudVisualizationDI.Program.Main(resultPathToFile, resultPathWithoutFormat, imageFormat, excludedWordList);
            }
            else
            {
                var pathToFile = args[0];
                var pathToSave = args[1];
                var lastDotIndex = pathToSave.LastIndexOf('.'); 
                var possibleFormat = pathToSave[lastDotIndex..];
                var pathWithoutFormat = pathToSave[..lastDotIndex];
                var imageFormat = CheckPossibleFormat(possibleFormat);

                var excludedWordsDocument = args.ElementAtOrDefault(2); 
                var excludedWordList 
                    = excludedWordsDocument == null ? new Result<List<string>>() : MakeExcludeWordList(excludedWordsDocument);

                var resultPathToFile = pathToFile.AsResult();
                var resultPathWithoutFormat = pathWithoutFormat.AsResult();

                TagsCloudVisualizationDI.Program.Main(resultPathToFile, resultPathWithoutFormat, imageFormat, excludedWordList);
            }
        }

        private static Result<ImageFormat> CheckPossibleFormat(string possibleFormat)
        {
            var lowerFormat = possibleFormat.ToLower();
            return (ImageFormats.ContainsKey(lowerFormat))
                ? ImageFormats[lowerFormat]
                : Result.Fail<ImageFormat>("Incorrect image format");
        }

        private static Result<List<string>> MakeExcludeWordList(string excludedWordsDocumentPath)
        {
            return Result.Of(() =>File.ReadLines(excludedWordsDocumentPath).Select(w => w.ToLower()).ToList(),
                $"Giving path to file: {excludedWordsDocumentPath} is not valid, NOTSYSTEM");
        }
    }
}
