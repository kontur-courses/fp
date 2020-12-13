using TagsCloud.ResultOf;

namespace TagsCloud.Options
{
    public static class CommandLineOptionsExtensions
    {
        public static Result<ImageOptions> ToImageOptions(this CommandLineOptions options) =>
            Result.Of(() => new ImageOptions(options.Width, options.Height, options.OutputDirectory,
                    options.OutputFileName, options.OutputFileExtension))
                .Then(imageOptions => imageOptions.Width < 0 || imageOptions.Height < 0
                    ? Result.Fail<ImageOptions>("")
                    : imageOptions)
                .ReplaceError(e => "Incorrect image size.");

        public static Result<FilterOptions> ToFilterOptions(this CommandLineOptions options) =>
            Result.Of(() => new FilterOptions(options.MystemLocation, options.BoringWords));
        
        public static Result<FontOptions> ToFontOptions(this CommandLineOptions options) =>
            Result.Of(() => new FontOptions(options.FontFamily, options.FontColor))
                .ReplaceError(e => "Incorrect color.");
    }
}