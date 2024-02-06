using TagsCloudVisualization.WFApp.Infrastructure;

namespace TagsCloudVisualization.WFApp.Factories;

public interface ITopLevelMenuItemFactory
{
    public ToolStripItem Create(MenuCategory category, IEnumerable<IUiAction> actions);
}
