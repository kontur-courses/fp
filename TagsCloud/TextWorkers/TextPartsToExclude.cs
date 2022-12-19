using TagsCloud.Interfaces;

namespace TagsCloud.TextWorkers
{
    public class TextPartsToExclude : ITextPartsToExclude
    {
        public string[] SpeechPartsToExclude { get; } = { "мест", "межд", "част", "предл", "союз" };

        public string[] WordsToExclude { get; }
    }
}