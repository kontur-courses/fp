using TagsCloud.App.Infrastructure;
using TagsCloud.CloudLayouter;
using TagsCloud.Infrastructure;
using TagsCloud.Infrastructure.UiActions;

namespace TagsCloud.App.Actions;

public class CircleCloudAction : IUiAction
{
    private readonly IImageHolder imageHolder;
    private readonly TagCloudPainter painter;
    private readonly AppSettings settings;
    private readonly Spiral spiral;

    public CircleCloudAction(
        AppSettings appSettings, IImageHolder imageHolder, TagCloudPainter painter, Spiral spiral)
    {
        settings = appSettings;
        this.imageHolder = imageHolder;
        this.painter = painter;
        this.spiral = spiral;
    }

    public MenuCategory Category => MenuCategory.Types;
    public string Name => "Круг";
    public string Description => "";

    public void Perform()
    {
        if (settings.File == null)
        {
            ErrorHandler.HandleError("сначала загрузи файл");
            return;
        }
        painter.Paint(settings.File.FullName, spiral).OnFail(ErrorHandler.HandleError);
    }
}