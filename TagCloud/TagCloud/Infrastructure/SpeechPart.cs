namespace TagCloud
{
    public class SpeechPart : ICheckable
    {
        public bool IsChecked { get; set; }

        public string Name { get; }

        public SpeechPartEnum Value { get; }

        public SpeechPart(string name, SpeechPartEnum value)
        {
            IsChecked = true;
            Name = name;
            Value = value;
        }
    }
}
