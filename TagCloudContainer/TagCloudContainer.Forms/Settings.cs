using TagCloudContainer.Core;
using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Core.Models;
using TagCloudContainer.Forms.Interfaces;
using TagCloudContainer.Utils;

namespace TagCloudContainer.Forms;

public partial class Settings : Form 
{
    private readonly TagCloud _tagCloud;
    private readonly ITagCloudContainerConfig _tagCloudContainerConfig;
    private readonly ISelectedValues _selectedValues;
    private readonly ITagCloudPlacerConfig _tagCloudPlacerConfig;
    private readonly IConfigValidator<ITagCloudPlacerConfig> _tagCloudPlacerConfigValidator;
    private readonly IConfigValidator<ITagCloudContainerConfig> _tagCloudContainerConfigValidator;
    private readonly IConfigValidator<ISelectedValues> _selectedValuesValidator;

    public Settings(
        TagCloud tagCloud, 
        ITagCloudContainerConfig tagCloudContainerConfig,
        ITagCloudPlacerConfig tagCloudPlacerConfig,
        ISelectedValues selectedValues,
        IConfigValidator<ITagCloudPlacerConfig> tagCloudPlacerConfigValidator,
        IConfigValidator<ISelectedValues> selectedValuesValidator,
        IConfigValidator<ITagCloudContainerConfig> tagCloudContainerConfigValidator)
    {
        _tagCloud = tagCloud;
        _tagCloudContainerConfig = tagCloudContainerConfig;
        _tagCloudPlacerConfig = tagCloudPlacerConfig;
        _selectedValues = selectedValues;
        _tagCloudPlacerConfigValidator = tagCloudPlacerConfigValidator;
        _tagCloudContainerConfigValidator = tagCloudContainerConfigValidator;
        _selectedValuesValidator = selectedValuesValidator;

        InitializeComponent();
    }

    private void randomStart_Click(object sender, EventArgs e)
    {
        RunTagCloudForm(true);
    }
    
    private void biggerInCenter_Click(object sender, EventArgs e)
    {
        RunTagCloudForm(false);
    }

    private void AddUserSelectedValues(bool random)
    {
        var parsedUserSelectedSizeValue = Sizes
            .Text
            .Split("x")
            .Select(i => int.Parse(i))
            .ToArray();
        var userSelectedSize = Screens.Sizes.First(size =>
            size.Width == parsedUserSelectedSizeValue[0] && size.Height == parsedUserSelectedSizeValue[1]);

        var colorResult = GetColorsByChoosedName(Colors.Text);
        var backgroundColorResult = GetColorsByChoosedName(BackgroundColors.Text);
        
        if (!colorResult.IsSuccess || !backgroundColorResult.IsSuccess)
            return;

        _selectedValues.WordsColor = colorResult.GetValueOrThrow();
        _selectedValues.BackgroundColor = backgroundColorResult.GetValueOrThrow();
        _selectedValues.FontFamily = Fonts.Text;
        _selectedValues.ImageSize = userSelectedSize;
        _selectedValues.PlaceWordsRandomly = random;
        
        var wordsFilePath = PathAssistant.GetFullFilePath("words.txt");
        var excludeWordsFilePath = PathAssistant.GetFullFilePath("boring_words.txt");

        _tagCloudContainerConfig.WordsFilePath = wordsFilePath.IsSuccess ? wordsFilePath.GetValueOrThrow() : null;
        _tagCloudContainerConfig.ExcludeWordsFilePath = excludeWordsFilePath.IsSuccess ? excludeWordsFilePath.GetValueOrThrow() : null;
        
        _tagCloudPlacerConfig.NearestToTheFieldCenterPoints = new SortedList<float, Point>();
        _tagCloudPlacerConfig.PutRectangles = new List<Rectangle>();
    }

    private void RunTagCloudForm(bool random)
    {
        AddUserSelectedValues(random);
        ValidateUserParameters();
        
        _tagCloud.ChangeSize(_selectedValues.ImageSize);
        _tagCloud.ShowDialog(this);
    }

    private Result<Color> GetColorsByChoosedName(string colorName)
    {
        var colorResult = Core.Models.Colors.Get(colorName);
        colorResult.OnFail(error => MessageBox.Show(error, "Ошибка"));
        
        return colorResult;
    }

    private void ValidateUserParameters()
    {
        _tagCloudContainerConfigValidator
            .Validate(_tagCloudContainerConfig)
            .OnFail(error => MessageBox.Show("Invalid container options: " + error, "Ошибка"));
        _tagCloudPlacerConfigValidator
            .Validate(_tagCloudPlacerConfig)
            .OnFail(error => MessageBox.Show("Invalid place config options: " + error, "Ошибка"));
        _selectedValuesValidator 
            .Validate(_selectedValues)
            .OnFail(error => MessageBox.Show("Invalid selected values: " + error, "Ошибка"));
    }
}