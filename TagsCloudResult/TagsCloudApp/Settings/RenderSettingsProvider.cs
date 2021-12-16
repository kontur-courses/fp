// using System.Collections.Generic;
// using System.Drawing;
// using System.Linq;
// using TagsCloudApp.Parsers;
// using TagsCloudApp.RenderCommand;
// using TagsCloudContainer;
// using TagsCloudContainer.ColorMappers;
// using TagsCloudContainer.MathFunctions;
// using TagsCloudContainer.Preprocessing;
// using TagsCloudContainer.Settings;
//
// namespace TagsCloudApp.Settings
// {
//     public class RenderSettingsProvider : IRenderSettingsProvider
//     {
//         private readonly IRenderArgs renderArgs;
//         private readonly IArgbColorParser colorParser;
//         private readonly IEnumParser enumParser;
//         private readonly IKeyValueParser keyValueParser;
//
//         public RenderSettingsProvider(
//             IRenderArgs renderArgs,
//             IArgbColorParser colorParser,
//             IEnumParser enumParser,
//             IKeyValueParser keyValueParser)
//         {
//             this.renderArgs = renderArgs;
//             this.colorParser = colorParser;
//             this.enumParser = enumParser;
//             this.keyValueParser = keyValueParser;
//         }
//
//         public Result<RenderSettings> GetSettings()
//         {
//             var renderSettings = new RenderSettings()
//             {
//                 FontFamily = renderArgs.FontFamily,
//                 MaxFontSize = renderArgs.MaxFontSize,
//                 MinFontSize = renderArgs.MinFontSize,
//                 ImageSize = renderArgs.ImageSize,
//                 ImageScale = renderArgs.ImageScale,
//             };
//
//             return SetBackgroundColor(renderSettings)
//                 .Then(SetDefaultColor)
//                 .Then(SetColorMapperType)
//                 .Then(SetSpeechPartColorMap)
//                 .Then(SetWordScale)
//                 .Then(SetIgnoredSpeechParts);
//         }
//
//         private Result<RenderSettings> SetBackgroundColor(RenderSettings settings) =>
//             colorParser.TryParse(renderArgs.BackgroundColor)
//                 .Then(color => settings with {BackgroundColor = color});
//
//         private Result<RenderSettings> SetDefaultColor(RenderSettings settings) =>
//             colorParser.TryParse(renderArgs.DefaultColor)
//                 .Then(color => settings with {DefaultColor = color});
//
//         private Result<RenderSettings> SetColorMapperType(RenderSettings settings) =>
//             enumParser.TryParse<WordColorMapperType>(renderArgs.ColorMapperType)
//                 .Then(type => settings with {ColorMapperType = type});
//
//         private Result<RenderSettings> SetSpeechPartColorMap(RenderSettings settings) =>
//             keyValueParser.Parse(renderArgs.SpeechPartColorMap)
//                 .Select(keyValue => enumParser.TryParse<SpeechPart>(keyValue.Key)
//                     .Then(speechPart => colorParser.TryParse(keyValue.Value)
//                         .Then(color => new KeyValuePair<SpeechPart, Color>(speechPart, color))))
//                 .CombineResults()
//                 .Then(keyValues =>
//                     settings with {SpeechPartColorMap = settings.SpeechPartColorMap.AddRange(keyValues)});
//
//         private Result<RenderSettings> SetWordScale(RenderSettings settings) =>
//             enumParser.TryParse<MathFunctionType>(renderArgs.WordsScale)
//                 .Then(type => settings with {WordsScale = type});
//
//         private Result<RenderSettings> SetIgnoredSpeechParts(RenderSettings settings) =>
//             renderArgs.IgnoredSpeechParts
//                 .Select(s => enumParser.TryParse<SpeechPart>(s))
//                 .CombineResults()
//                 .Then(ignoreSpeechParts => settings with
//                 {
//                     IgnoredSpeechParts = settings.IgnoredSpeechParts.Union(ignoreSpeechParts)
//                 });
//     }
// }