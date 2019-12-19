namespace TagCloud
{
    public class BoringWordsListAction : IUiAction
    {
        private readonly BoringWord[] boringWords;

        public MenuCategory Category => MenuCategory.Lists;
        public string Name => "Boring words...";
        public string Description => "Boring words choice";

        public BoringWordsListAction(BoringWord[] boringWords)
        {
            this.boringWords = boringWords;
        }

        public void Perform()
        {
            BoringWordsForm.For(boringWords).ShowDialog();
        }
    }
}
