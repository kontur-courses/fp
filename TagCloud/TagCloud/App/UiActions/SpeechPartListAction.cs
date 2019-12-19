namespace TagCloud
{
    public class SpeechPartListAction : IUiAction
    {
        private readonly SpeechPart[] speechParts;

        public MenuCategory Category => MenuCategory.Lists;
        public string Name => "Speech parts...";
        public string Description => "Speech parts choice";

        public SpeechPartListAction(SpeechPart[] speechParts)
        {
            this.speechParts = speechParts;
        }

        public void Perform()
        {
            SpeechPartsForm.For(speechParts).ShowDialog();
        }
    }
}
