// using System.Collections.Generic;
// using System.Collections.Immutable;
// using System.Drawing;
// using TagsCloudContainer.ColorMappers;
// using TagsCloudContainer.MathFunctions;
// using TagsCloudContainer.Preprocessing;
//
// namespace TagsCloudContainer.Settings
// {
//     public class RenderSettings
//     {
//         public FontFamily FontFamily { get; init; } = FontFamily.GenericMonospace;
//         public int MaxFontSize { get; init; }
//         public int MinFontSize { get; init; }
//         public Size? ImageSize { get; init; }
//         public float ImageScale { get; init; }
//         public Color BackgroundColor { get; init; }
//         public Color DefaultColor { get; init; }
//         public WordColorMapperType ColorMapperType { get; init; }
//         public ImmutableDictionary<SpeechPart, Color> SpeechPartColorMap { get; init; } =
//             ImmutableDictionary<SpeechPart, Color>.Empty;
//         public MathFunctionType WordsScale { get; init; }
//         public ImmutableHashSet<SpeechPart> IgnoredSpeechParts { get; init; } = ImmutableHashSet<SpeechPart>.Empty;
//     }
// }