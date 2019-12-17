namespace TagCloud
{
    public class BoringWord : ICheckable
    {
        public bool IsChecked { get; set; }

        public string Name { get; }

        public BoringWord(string name)
        {
            IsChecked = true;
            Name = name;
        }
    }
}
