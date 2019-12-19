namespace TagCloud
{
    public class SpeechPart : ICheckable
    {
        public bool IsChecked { get; set; }

        public SpeechPartEnum Value { get; }

        public SpeechPart(SpeechPartEnum value)
        {
            IsChecked = true;
            Value = value;
        }
    }
}
