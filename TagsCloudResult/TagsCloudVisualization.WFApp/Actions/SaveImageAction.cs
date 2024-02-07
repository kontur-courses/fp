using TagsCloudVisualization.Common;
using TagsCloudVisualization.WFApp.Infrastructure;

namespace TagsCloudVisualization.WFApp.Actions;

public class SaveImageAction : IUiAction
{
    private readonly IImageHolder imageHolder;

    public SaveImageAction(IImageHolder imageHolder)
    {
        this.imageHolder = imageHolder;
    }

    public MenuCategory Category => MenuCategory.File;
    public string Name => Resources.SaveImageAction_Name;
    public string Description => Resources.SaveImageAction_Description;

    public void Perform()
    {
        var dialog = new SaveFileDialog
        {
            CheckFileExists = false,
            InitialDirectory = Path.GetFullPath("/"),
            DefaultExt = Resources.SaveImageAction_Perform_DefaultExt,
            FileName = Resources.SaveImageAction_Perform_FileName,
            Filter = Resources.SaveImageAction_Perform_Filter 
        };

        var res = dialog.ShowDialog();

        if (res == DialogResult.OK)
        {
            imageHolder.SaveImage(dialog.FileName);
        }
    }
}
