namespace TagsCloud.Interfaces
{
    public interface ITextPartsToExclude
    {
        public string[] SpeechPartsToExclude { get; }
        public string[] WordsToExclude { get; }
    }
}