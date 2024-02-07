using TagsCloud.App.Infrastructure;
using TagsCloud.CloudLayouter;
using TagsCloud.Infrastructure;
using TagsCloud.Infrastructure.UiActions;

namespace TagsCloud.App.Actions;

public class FlowerCloudAction : IUiAction
{
    private readonly FlowerSpiral flowerSpiral;
    private readonly TagCloudPainter painter;
    private readonly AppSettings settings;

    public FlowerCloudAction(TagCloudPainter painter, AppSettings settings, FlowerSpiral spiral)
    {
        flowerSpiral = spiral;
        this.settings = settings;
        this.painter = painter;
    }

    public MenuCategory Category => MenuCategory.Types;
    public string Name => "Цветок";
    public string Description => "";

    public void Perform()
    {
        if (settings.File == null)
        {
            ErrorHandler.HandleError("сначала загрузи файл");
            return;
        }

        painter.Paint(settings.File.FullName, flowerSpiral).OnFail(ErrorHandler.HandleError);
    }
}