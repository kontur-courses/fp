using TagCloudContainer.Core;
using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Core.Models;
using TagCloudContainer.Utils;

namespace TagCloudContainer;

public partial class TagCloud : Form
{
    private Graphics _graphics;
    private readonly ITagCloudProvider _tagCloudProvider;
    private readonly IImageCreator _imageCreator;
    private readonly ISelectedValues _selectedValues;
    private readonly ITagCloudContainerConfig _tagCloudContainerConfig;
    private readonly ITagCloudPlacerConfig _tagCloudPlacerConfig;

    public TagCloud(ITagCloudProvider tagCloudProvider,
        ITagCloudContainerConfig tagCloudContainerConfig,
        ISelectedValues selectedValues,
        IImageCreator imageCreator,
        ITagCloudPlacerConfig tagCloudPlacerConfig)
    {
        _tagCloudProvider = tagCloudProvider;
        _imageCreator = imageCreator;
        _tagCloudContainerConfig = tagCloudContainerConfig;
        _selectedValues = selectedValues;
        _tagCloudPlacerConfig = tagCloudPlacerConfig;

        InitializeComponent();
        SetupWindow();
    }

    private void SetupWindow()
    {
        Text = "Tag Cloud Container";
        Size = _selectedValues.ImageSize;
    }

    public void ChangeSize(Size size)
    {
        Size = size;
    }

    private void Render(object sender, PaintEventArgs e)
    {
        _graphics = e.Graphics;
        _graphics.Clear(_selectedValues.BackgroundColor);

        _tagCloudPlacerConfig.FieldCenter = new Point(Width / 2, Height / 2);
        _tagCloudContainerConfig.StandartFontSize = new Size(10, 10);
        var words = _tagCloudProvider.GetPreparedWords();

        if (!words.IsSuccess)
        {
            MessageBox.Show(words.Error, "Ошибка");
            return;
        }

        DrawWords(e, words);
        SaveImage();
    }

    private void DrawWords(PaintEventArgs e, Result<List<Word>> words)
    {
        var pen = new Pen(_selectedValues.WordsColor);
        try
        {
            foreach (var word in words.GetValueOrThrow())
            {
                var font = new Font(_selectedValues.FontFamily,
                    word.Weight * _tagCloudContainerConfig.StandartFontSize.Width);
                try
                {
                    _graphics.DrawString(word.Value, font, pen.Brush, word.Position);
                }
                finally
                {
                    font.Dispose();
                }
            }

        }
        finally
        {
            pen.Dispose();
        }
    }

    private void SaveImage()
    {
        var mainDirectoryPath  = PathAssistant.GetMainDirectoryPath();

        if (!mainDirectoryPath.IsSuccess || !Directory.Exists(mainDirectoryPath.GetValueOrThrow()))
        {
            MessageBox.Show("Неверный путь к каталогу сохранения изображения", "Ошибка");
            return;
        }
        
        _imageCreator.Save(this, Path.Combine(mainDirectoryPath.GetValueOrThrow(), _tagCloudContainerConfig.ImageName));
    }
}