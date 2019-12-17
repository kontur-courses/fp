using System.IO;
using NHunspell;
using ResultOf;
using TagsCloudGenerator.Core.Filters;
using TagsCloudGenerator.Core.Layouters;
using TagsCloudGenerator.Core.Normalizers;
using TagsCloudGenerator.Core.Spirals;

namespace TagsCloudGenerator.Core.Translators
{
    public class TextToTagsTranslatorFactory
    {
        private const string AffFilename = @"HunspellDictionaries\ru.aff";
        private const string DicFilename = @"HunspellDictionaries\ru.dic";
        private const string PathToMyStem32 = "mystem32.exe";
        private const string PathToMyStem64 = "mystem64.exe";

        public static Result<TextToTagsTranslator> Create(float alpha, float stepPhi)
        {
            if (!File.Exists(AffFilename))
            {
                return Result.Fail<TextToTagsTranslator>($"Cannot find file {AffFilename}");
            }
            if (!File.Exists(DicFilename))
            {
                return Result.Fail<TextToTagsTranslator>($"Cannot find file {DicFilename}");
            }
            if (!File.Exists(PathToMyStem32))
            {
                return Result.Fail<TextToTagsTranslator>($"Cannot find file {PathToMyStem32}");
            }
            if (!File.Exists(PathToMyStem64))
            {
                return Result.Fail<TextToTagsTranslator>($"Cannot find file {PathToMyStem64}");
            }
            return Result.Ok(new TextToTagsTranslator(
                new WordsNormalizer(), 
                new Hunspell(AffFilename, DicFilename),
                new WordsFilter(PathToMyStem32, PathToMyStem64),
                new SpiralRectangleCloudLayouter(new ArchimedeanSpiral(alpha, stepPhi))
            ));                
        }
    }
}