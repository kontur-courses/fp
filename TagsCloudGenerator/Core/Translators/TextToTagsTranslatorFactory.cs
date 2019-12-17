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
            return CheckFileExists(AffFilename)
                .Then((none) => CheckFileExists(DicFilename))
                .Then((none) => CheckFileExists(PathToMyStem32))
                .Then((none) => CheckFileExists(PathToMyStem64))
                .Then((none) => new TextToTagsTranslator(
                    new WordsNormalizer(),
                    new Hunspell(AffFilename, DicFilename),
                    new WordsFilter(PathToMyStem32, PathToMyStem64),
                    new SpiralRectangleCloudLayouter(new ArchimedeanSpiral(alpha, stepPhi)))
                );
        }

        private static Result<None> CheckFileExists(string filename)
        {
            return File.Exists(filename)
                ? Result.Ok()
                : Result.Fail<None>($"Cannot find file {filename}");
        }
    }
}