namespace TagCloud2
{
    public interface ISillyWordSelector
    {
        public Result<bool> IsWordSilly(string word);
    }
}
