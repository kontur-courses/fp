using ResultLibrary;
using TagsCloudPainter;
using TagsCloudPainter.CloudLayouter;
using TagsCloudPainter.Drawer;
using TagsCloudPainter.FileReader;
using TagsCloudPainter.Parser;
using TagsCloudPainter.Tags;
using TagsCloudPainterApplication.Infrastructure;
using TagsCloudPainterApplication.Infrastructure.Settings;
using TagsCloudPainterApplication.Infrastructure.Settings.FilesSource;
using TagsCloudPainterApplication.Infrastructure.Settings.Image;
using TagsCloudPainterApplication.Infrastructure.Settings.TagsCloud;

namespace TagsCloudPainterApplication.Actions;

public class DrawTagCloudAction : IUiAction
{
    private readonly ICloudDrawer cloudDrawer;
    private readonly ICloudLayouter cloudLayouter;
    private readonly IFilesSourceSettings filesSourceSettings;
    private readonly IImageHolder imageHolder;
    private readonly IImageSettings imageSettings;
    private readonly Palette palette;
    private readonly ITagsBuilder tagsBuilder;
    private readonly ITagsCloudSettings tagsCloudSettings;
    private readonly IFormatFileReader<string> textFileReader;
    private readonly ITextParser textParser;

    public DrawTagCloudAction(
        IImageSettings imageSettings,
        ITagsCloudSettings tagsCloudSettings,
        IFilesSourceSettings filesSourceSettings,
        IImageHolder imageHolder,
        ICloudDrawer cloudDrawer,
        ICloudLayouter cloudLayouter,
        ITagsBuilder tagsBuilder,
        ITextParser textParser,
        IFormatFileReader<string> textFileReader,
        Palette palette)
    {
        this.cloudDrawer = cloudDrawer ?? throw new ArgumentNullException(nameof(cloudDrawer));
        this.cloudLayouter = cloudLayouter ?? throw new ArgumentNullException(nameof(cloudLayouter));
        this.tagsBuilder = tagsBuilder ?? throw new ArgumentNullException(nameof(tagsBuilder));
        this.textParser = textParser ?? throw new ArgumentNullException(nameof(textParser));
        this.textFileReader = textFileReader ?? throw new ArgumentNullException(nameof(textFileReader));
        this.imageSettings = imageSettings ?? throw new ArgumentNullException(nameof(imageSettings));
        this.tagsCloudSettings = tagsCloudSettings ?? throw new ArgumentNullException(nameof(tagsCloudSettings));
        this.imageHolder = imageHolder ?? throw new ArgumentNullException(nameof(imageHolder));
        this.filesSourceSettings = filesSourceSettings ?? throw new ArgumentNullException(nameof(filesSourceSettings));
        this.palette = palette ?? throw new ArgumentNullException(nameof(palette));
    }

    public string Category => "Облако тэгов";

    public string Name => "Нарисовать";

    public string Description => "Нарисовать облако тэгов";

    public void Perform()
    {
        var wordsFilePath = GetFilePath().OnFail(HandleError);
        if (!wordsFilePath.IsSuccess)
            return;

        SettingsForm.For(tagsCloudSettings).ShowDialog();
        tagsCloudSettings.CloudSettings.BackgroundColor = palette.BackgroundColor;
        tagsCloudSettings.TagSettings.TagColor = palette.PrimaryColor;

        var wordsText = textFileReader.ReadFile(wordsFilePath.GetValueOrThrow())
            .OnFail(error => HandleError("File reading error: " + error));
        if (!wordsText.IsSuccess)
            return;

        var boringText = textFileReader.ReadFile(filesSourceSettings.BoringTextFilePath)
            .OnFail(error => HandleError("File reading error: " + error));
        if (!boringText.IsSuccess)
            return;

        tagsCloudSettings.TextSettings.BoringText = boringText.GetValueOrThrow();
        var parsedWords = textParser.ParseText(wordsText.GetValueOrThrow())
            .OnFail(error => HandleError("File parsing error: " + error));
        if (!parsedWords.IsSuccess)
            return;

        var cloud = GetCloud(parsedWords.GetValueOrThrow()).OnFail(HandleError);
        if (!cloud.IsSuccess)
            return;

        var drawingResult = DrawCloud(cloud.GetValueOrThrow()).OnFail(HandleError);
    }

    private static Result<string> GetFilePath()
    {
        OpenFileDialog fileDialog = new()
        {
            InitialDirectory = Environment.CurrentDirectory,
            Filter =
                "Текстовый файл txt (*.txt)|*.txt|Текстовый файл doc (*.doc)|*.doc|Текстовый файл docx (*.docx)|*.docx",
            FilterIndex = 0,
            RestoreDirectory = true
        };
        var dialogResult = fileDialog.ShowDialog();
        if (dialogResult == DialogResult.Cancel || string.IsNullOrEmpty(fileDialog.FileName))
            return Result.Fail<string>("File hasn't been selected");

        return fileDialog.FileName.AsResult();
    }

    private Result<TagsCloud> GetCloud(List<string> words)
    {
        var tags = tagsBuilder.GetTags(words);
        if (!tags.IsSuccess)
            return Result.Fail<TagsCloud>(tags.Error);

        cloudLayouter.Reset();
        var layoutResult = cloudLayouter.PutTags(tags.GetValueOrThrow());
        if (!layoutResult.IsSuccess)
            return Result.Fail<TagsCloud>(layoutResult.Error);

        var cloud = cloudLayouter.GetCloud();

        return cloud;
    }

    private Result<None> DrawCloud(TagsCloud cloud)
    {
        var bitmap = cloudDrawer.DrawCloud(cloud, imageSettings.Width, imageSettings.Height);
        if (!bitmap.IsSuccess)
            return Result.Fail<None>(bitmap.Error);

        using (var graphics = imageHolder.StartDrawing())
        {
            graphics.DrawImage(bitmap.GetValueOrThrow(), new Point(0, 0));
            bitmap.Then(bitmap => bitmap.Dispose());
        }
        imageHolder.UpdateUi();

        return Result.Ok();
    }

    private void HandleError(string error)
    {
        MessageBox.Show(error);
    }
}