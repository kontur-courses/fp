using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI.Menu
{
    public class MenuCreator : IMenuCreator
    {
        public IMainMenu Menu { get; }
        public MenuCreator(IUiAction[] actions, IResultHandler handler)
        {
            Menu = actions.GetMenu(handler);
        }
    }
}