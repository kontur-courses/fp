using TagsCloudVisualization.Common.ResultOf;
using TagsCloudVisualization.PointsProviders;
using TagsCloudVisualization.WFApp.Common;
using TagsCloudVisualization.WFApp.Infrastructure;

namespace TagsCloudVisualization.WFApp.Actions;

public class TagsCloudAction : IUiAction
{
    private readonly TagsCloudVisualizator tagsCloudVisualizator;
    private readonly ArchimedeanSpiralSettings settings;

    public TagsCloudAction(TagsCloudVisualizator tagsCloudVisualizator, ArchimedeanSpiralSettings settings)
    {
        this.tagsCloudVisualizator = tagsCloudVisualizator;
        this.settings = settings;
    }

    public MenuCategory Category => MenuCategory.TagsClouds;
    public string Name => Resources.TagsCloudAction_Name;
    public string Description => Resources.TagsCloudAction_Name;

    public void Perform()
    {
        SettingsForm.For(settings).ShowDialog();
        tagsCloudVisualizator.DrawTagsCloud().OnFail(x => throw new Exception(x));
    }
}
