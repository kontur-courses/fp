using System.Collections.Generic;
using DeepMorphy;
using DeepMorphy.Model;
using TagsCloud.Interfaces;

namespace TagsCloud.TextWorkers
{
    public class MorphsParser : IMorphsParser
    {
        private readonly ITextSplitter textSplitter;
        private readonly IFileValidator fileValidator;
        private readonly IMorphsValidator morphsValidator;

        public MorphsParser(ITextSplitter textSplitter, IFileValidator fileValidator, IMorphsValidator morphsValidator)
        {
            this.textSplitter = textSplitter;
            this.fileValidator = fileValidator;
            this.morphsValidator = morphsValidator;
        }

        public IEnumerable<MorphInfo> GetMorphs(string filePath)
        {
            var morph = new MorphAnalyzer(true);

            var validation = fileValidator.VerifyFileExistence(filePath)
                .ThenDoWorkWithValue(x => TextReader.ReadFile(x))
                .ThenDoWorkWithValue(x => textSplitter.SplitTextOnWords(x));

            var morphInfoValidate = morphsValidator.ParseOnMorphs(validation.Value);

            return morphInfoValidate.Value;
        }
    }
}