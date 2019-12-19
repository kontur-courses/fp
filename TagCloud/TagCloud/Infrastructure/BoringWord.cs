namespace TagCloud
{
    public class BoringWord : ICheckable
    {
        public bool IsChecked { get; set; }

        public string Value { get; }

        public BoringWord(string value)
        {
            Value = value;
            IsChecked = true;
        }
    }
}
