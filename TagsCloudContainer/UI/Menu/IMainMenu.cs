namespace TagsCloudContainer.UI.Menu
{
    public interface IMainMenu
    {
        public Category[] Categories { get; }

        public void ChooseCategory();
    }
}