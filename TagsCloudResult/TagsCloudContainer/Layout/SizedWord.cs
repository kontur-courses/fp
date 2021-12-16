namespace TagsCloudContainer.Layout
{
    public class SizedWord
    {
        public string Word { get; }
        public float Size { get; }

        public SizedWord(string word, float size)
        {
            Word = word;
            Size = size;
        }
    }
}