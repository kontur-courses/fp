using TagsCloudVisualization.WFApp.Factories;

namespace TagsCloudVisualization.WFApp.Infrastructure;

public static class UiActionExtensions
{
    public static ToolStripItem[] ToMenuItems(this IEnumerable<IUiAction> actions, ITopLevelMenuItemFactory factory)
    {
        var items = actions.GroupBy(a => a.Category)
            .OrderBy(a => a.Key)
            .Select(g => factory.Create(g.Key, g.ToList()))
            .ToArray();
        
        return items;
    }
}
