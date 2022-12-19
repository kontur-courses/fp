using DeepMorphy;
using DeepMorphy.Model;
using System.Collections.Generic;
using TagsCloud.Interfaces;

namespace TagsCloud.TextWorkers
{
    public class MorphsParser : IMorphsParser
    {
        private ITextSplitter textSplitter;
        private IFileValidator fileValidator;
        private IMorphsValidator morphsValidator;

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
                .ThenDoWorkWithValue((x) => TextReader.ReadFile(x))
                .ThenDoWorkWithValue((x) => textSplitter.SplitTextOnWords(x));

            var morphInfoValidate = morphsValidator.ParseOnMorphs(validation.GetValueOrThrow());

            return morphInfoValidate.GetValueOrThrow();
        }
    }
}
