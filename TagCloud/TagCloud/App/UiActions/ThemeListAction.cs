namespace TagCloud
{
    public class ThemeListAction : IUiAction
    {
        private readonly ITheme[] themes;

        public MenuCategory Category => MenuCategory.Lists;
        public string Name => "Themes...";
        public string Description => "Themes choice";

        public ThemeListAction(ITheme[] themes)
        {
            this.themes = themes;
        }

        public void Perform()
        {
            CheckedListForm.For(themes).ShowDialog();
        }
    }
}
