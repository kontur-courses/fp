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
        public static void Main(string[] args)
        {
            if (args.Contains("-h") || args.Contains("--help"))
            {
                Console.WriteLine("HELP");
                return;
            }

            if (args.Length < 2 || args.Length > 4)
                throw new ArgumentException("Incorrect Number Of MystemArgs");

            var pathToFile = args.ElementAtOrDefault(0);
            var pathToSave = args.ElementAtOrDefault(1);

            if (pathToSave == null)
                throw new ArgumentException("incorrect path to save");

            var lastDotIndex = pathToSave.LastIndexOf('.');
            var possibleFormat = pathToSave.Substring(lastDotIndex);

            var pathWithoutFormat = pathToSave.Substring(0, lastDotIndex);

            var imageFormat = CheckPossibleFormat(possibleFormat);


            var excludedWordsDocument = args.ElementAtOrDefault(2);
            var excludedWordList = MakeExcludeWordList(excludedWordsDocument);

            TagsCloudVisualizationDI.Program.Main(pathToFile, pathWithoutFormat, imageFormat, excludedWordList);
        }

        private static Result<ImageFormat> CheckPossibleFormat(string possibleFormat)
        {
            return Result.Of(() => possibleFormat.ToLower() switch
            {
                ".png" => ImageFormat.Png,
                ".jpeg" => ImageFormat.Jpeg,
                ".jpg" => ImageFormat.Jpeg,
                ".bmp" => ImageFormat.Bmp,
                ".tiff" => ImageFormat.Tiff,
                ".ico" => ImageFormat.Icon,
                ".gif" => ImageFormat.Gif,
                _ => ImageFormat.Png,
            });
        }

        private static Result<List<string>> MakeExcludeWordList(string excludedWordsDocumentPath)
        {
            return Result.Of(() =>File.ReadLines(excludedWordsDocumentPath).Select(w => w.ToLower()).ToList(),
                $"Giving path to file: {excludedWordsDocumentPath} is not valid, NOTSYSTEM");
        }
    }
}
