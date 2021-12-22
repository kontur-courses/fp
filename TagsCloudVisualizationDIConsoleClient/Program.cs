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
        private static readonly Dictionary<string, ImageFormat> _imageFormats= new Dictionary<string, ImageFormat>()
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

            Result.OnFalse(!(args.Length < 2 || args.Length > 3), (er) => PrintAboutFail(er), $"Incorrect number of arguments ({args.Length})" +
                "but should be between 2 and 3");

            var pathToFile = args[0];
            var pathToSave = args[1];


            var lastDotIndex = pathToSave.LastIndexOf('.');
            var possibleFormat = pathToSave[lastDotIndex..];

            var pathWithoutFormat = pathToSave[..lastDotIndex];

            var imageFormat = CheckPossibleFormat(possibleFormat);


            var excludedWordsDocument = args.ElementAtOrDefault(2);
            var excludedWordList 
                = excludedWordsDocument == null ? new Result<List<string>>() : MakeExcludeWordList(excludedWordsDocument);

            TagsCloudVisualizationDI.Program.Main(pathToFile, pathWithoutFormat, imageFormat, excludedWordList);
        }

        private static Result<ImageFormat> CheckPossibleFormat(string possibleFormat)
        {
            var lowerFormat = possibleFormat.ToLower();
            return (_imageFormats.ContainsKey(lowerFormat))
                ? _imageFormats[lowerFormat]
                : Result.Fail<ImageFormat>("Incorrect image format");
            /*
            return Result.Of(() => possibleFormat.ToLower() switch
            {
                ".png" => ImageFormat.Png,
                ".jpeg" => ImageFormat.Jpeg,
                ".jpg" => ImageFormat.Jpeg,
                ".bmp" => ImageFormat.Bmp,
                ".tiff" => ImageFormat.Tiff,
                ".ico" => ImageFormat.Icon,
                ".gif" => ImageFormat.Gif,
                _ => throw new FormatException("Incorrect image format"),
            });
            */
        }

        private static Result<List<string>> MakeExcludeWordList(string excludedWordsDocumentPath)
        {
            return Result.Of(() =>File.ReadLines(excludedWordsDocumentPath).Select(w => w.ToLower()).ToList(),
                $"Giving path to file: {excludedWordsDocumentPath} is not valid, NOTSYSTEM");
        }

        private static void PrintAboutFail(string error)
        {
            throw new Exception(error);
        }
    }
}
