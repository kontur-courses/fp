using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ErrorHandler;
using TagsCloudVisualization.Logic;

namespace TagsCloudVisualization.Services
{
    public class AppSettings : IImageSettingsProvider, IDocumentPathProvider, IBoringWordsProvider
    {
        private string textDocumentPath;
        public HashSet<string> BoringWords { get; set; }
        public ImageSettings ImageSettings { get; private set; }

        public AppSettings()
        {
            BoringWords = ExtractBoringWordsFromFile();
            ImageSettings = ImageSettings.InitializeDefaultSettings();
        }

        public Result<None> SetImageSettings(ImageSettings imageSettings)
        {
            if (imageSettings.ImageSize.Width <= 0 || imageSettings.ImageSize.Height <= 0)
                return Result.Fail<None>("Image sizes can't be non-positive");
            ImageSettings = imageSettings;
            return Result.Ok();
        }

        public Result<string> GetPath()
        {
            return File.Exists(textDocumentPath)
                ? textDocumentPath
                : Result.Fail<string>("Couldn't get text document path");
        }

        public void SetPath(string path)
        {
            textDocumentPath = path;
        }

        private HashSet<string> ExtractBoringWordsFromFile()
        {
            var boringWordsPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BoringWords.txt"));
            var boringWordsText = TextRetriever.RetrieveTextFromFile(boringWordsPath);
            return boringWordsText.IsSuccess ? 
                boringWordsText
                    .GetValueOrThrow()
                    .Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .ToHashSet()
                : new HashSet<string>();
        }
    }
}