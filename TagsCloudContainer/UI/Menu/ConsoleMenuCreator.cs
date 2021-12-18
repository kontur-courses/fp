using System.IO;

namespace TagsCloudContainer.UI.Menu
{
    public class ConsoleMenuCreator : IMenuCreator
    {
        public IMainMenu Menu { get; }
        public ConsoleMenuCreator(UiAction[] actions, TextReader reader, TextWriter writer)
        {
            Menu = actions.GetConsoleMenu(reader, writer);
        }
    }
}