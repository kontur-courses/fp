namespace CTV.Common.Preprocessors
{
    public interface IWordsPreprocessor
    {
        public string[] Preprocess(string[] rawWords);
    }
}