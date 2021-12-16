// using TagsCloudApp.Parsers;
// using TagsCloudApp.RenderCommand;
// using TagsCloudContainer;
//
// namespace TagsCloudApp.Settings
// {
//     public class SaveSettingsProvider
//     {
//         private readonly IRenderArgs renderArgs;
//         private readonly IImageFormatParser formatParser;
//
//         public SaveSettingsProvider(IRenderArgs renderArgs, IImageFormatParser formatParser)
//         {
//             this.renderArgs = renderArgs;
//             this.formatParser = formatParser;
//         }
//
//         public Result<SaveSettings> GetSettings()
//         {
//             return formatParser.Parse(renderArgs.ImageFormat)
//                 .Then(format => new SaveSettings(renderArgs.OutputPath, format));
//         }
//     }
// }