using TagsCloudVisualization.WFApp.Infrastructure;

namespace TagsCloudVisualization.WFApp.Factories;

public class DefaultTopLevelMenuItemFactory : ITopLevelMenuItemFactory
{
    public ToolStripItem Create(MenuCategory category, IEnumerable<IUiAction> actions)
    {
        var menuItems = actions.Select(ToMenuItem).ToArray();
        return new ToolStripMenuItem(category.GetDescription(), null, menuItems);
    }

    private static ToolStripItem ToMenuItem(IUiAction action)
    {
        return new ToolStripMenuItem(action.Name, null, (_, _) => action.Perform())
        {
            ToolTipText = action.Description,
            Tag = action
        };
    }
}


