using System;

namespace TagsCloudVisualizationDI
{
    public class Word
    {
        public string WordText { get; set; }
        public int CntOfWords { get; set; }

        public Word(string word)
        {
            WordText = word;
            CntOfWords = 1;
        }
    }
}
