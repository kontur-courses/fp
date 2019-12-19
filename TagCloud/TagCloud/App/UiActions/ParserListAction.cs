namespace TagCloud
{
    public class ParserListAction : IUiAction
    {
        private readonly IParser[] parsers;

        public MenuCategory Category => MenuCategory.Lists;
        public string Name => "Parsers...";
        public string Description => "Parsers choice";

        public ParserListAction(IParser[] parsers)
        {
            this.parsers = parsers;
        }

        public void Perform()
        {
            CheckedListForm.For(parsers).ShowDialog();
        }
    }
}
