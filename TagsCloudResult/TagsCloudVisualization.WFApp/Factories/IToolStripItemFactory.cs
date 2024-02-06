using TagsCloudVisualization.WFApp.Infrastructure;

namespace TagsCloudVisualization.WFApp.Factories;

public interface IToolStripItemFactory
{
    public ToolStripItem Create(MenuCategory category, IEnumerable<IUiAction> actions);
}
