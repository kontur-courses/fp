using System;
using System.Drawing;
using CommandLine;
using TagsCloudContainer.ImageCreators;
using TagsCloudContainer.ImageSavers;
using TagsCloudContainer.ProjectSettings;
using TagsCloudContainer.Readers;
using TagsCloudContainer.Settings;
using TagsCloudContainer.WordsHandlers;
using Size = System.Drawing.Size;

namespace TagsCloudContainer.Clients
{
    public class ConsoleClient : IClient
    {
        private readonly IImageCreator imageCreator;
        private readonly IWordsHandler wordsHandler;
        private readonly IImageSaver imageSaver;
        private readonly IReader filesReader;
        private readonly ISettingsManager settings;

        public ConsoleClient(IWordsHandler wordsHandler, IImageCreator imageCreator, IReader filesReader,
            ISettingsManager settings, IImageSaver imageSaver)
        {
            this.wordsHandler = wordsHandler;
            this.imageCreator = imageCreator;
            this.imageSaver = imageSaver;
            this.filesReader = filesReader;
            this.settings = settings;
        }

        public void Execute(string[] args)
        {
            Parser.Default.ParseArguments<ConsoleClientOptions>(args)
                .WithParsed(o =>
                {
                    UpdateSettings(o);
                    Result.Of(() => filesReader.Read(o.Source))
                        .Then(wordsHandler.HandleWords)
                        .Then(imageCreator.GetImage)
                        .Then((x) => imageSaver.SaveImage(x, o.Destination))
                        .OnFail(FailApplication);
                    Console.WriteLine("Image successfully created");
                });
        }

        private void UpdateSettings(ConsoleClientOptions options)
        {
            SetColor(settings, options.Color)
                .Then(x => SetImageSize(x, new Size(options.Width, options.Height)))
                .Then(x => SetFontSize(x, options.BaseFontSize))
                .Then(x => SetFontFamily(x, options.FontFamily))
                .RefineError("Settings are incorrect. ")
                .OnFail(FailApplication);
        }

        private Result<ISettingsManager> SetFontSize(ISettingsManager settingsManager, int font)
        {
            settings.ImageSettings.BaseFontSize = font;
            return Validate(settingsManager, x => IsFontSizeCorrect(x.ImageSettings.BaseFontSize), "Font size is incorrect.");
        }

        private Result<ISettingsManager> SetColor(ISettingsManager settingsManager, string color)
        {
            settingsManager.Palette.FontColor = Color.FromName(color);
            return Validate(settingsManager, x => IsColorCorrect(x.Palette.FontColor), "Font color is unknown.");
        }

        private Result<ISettingsManager> SetImageSize(ISettingsManager settingsManager, Size size)
        {
            settingsManager.ImageSettings.ImageSize = size;
            return Validate(settingsManager, x => IsSizeCorrect(settingsManager.ImageSettings.ImageSize), "Size is incorrect");
        }

        private Result<ISettingsManager> SetFontFamily(ISettingsManager settingsManager, string fontFamily)
        {
            return Result.Of(() => new FontFamily(fontFamily))
                .Then(x => ChangeFont(settingsManager, x));
        }

        private ISettingsManager ChangeFont(ISettingsManager settingsManager, FontFamily fontFamily)
        {
            settingsManager.TextSettings.Family = fontFamily;
            return settingsManager;
        }

        private bool IsFontSizeCorrect(int baseFontSize)
            => baseFontSize >= 10 && baseFontSize <= 100;

        private bool IsColorCorrect(Color color)
            => color.IsKnownColor;

        private bool IsSizeCorrect(Size size)
            => size.Height > 0 && size.Width > 0;

        private void FailApplication(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(1);
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage)
            => predicate(obj) ? Result.Ok(obj) : Result.Fail<T>(errorMessage);

    }
}
