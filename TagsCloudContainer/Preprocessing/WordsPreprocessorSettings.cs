namespace TagsCloudContainer.Preprocessing
{
    public class WordsPreprocessorSettings
    {
        public int BoringWordsLength { get; set; } = 4;
        public string[] ExcludedWords { get; set; } = new string[0];
        public bool BringInTheInitialForm { get; set; }
    }
}