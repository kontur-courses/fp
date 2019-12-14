namespace TagsCloudResult.TextParsing.CloudParsing
{
    public class CloudWord
    {
        public CloudWord(string word)
        {
            Word = word;
            Count = 1;
        }

        public string Word { get; }
        public int Count { get; set; }

        public void AddCount()
        {
            Count += 1;
        }
    }
}