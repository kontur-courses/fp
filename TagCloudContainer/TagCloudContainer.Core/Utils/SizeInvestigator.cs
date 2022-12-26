using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Core.Models;

namespace TagCloudContainer.Core.Utils;

public class SizeInvestigator : ISizeInvestigator
{
    private readonly ISelectedValues _selectedValues;

    public SizeInvestigator(ISelectedValues selectedValues)
    {
        _selectedValues = selectedValues;
    }
    
    public bool DidFit(Word word) => 
        OutOfBounds(_selectedValues.ImageSize, new Rectangle(word.Position, word.Size));

    private bool OutOfBounds(Size formSize, Rectangle rectangle)
    {
        return
            rectangle.Bottom > formSize.Height
            || rectangle.Right > formSize.Width
            || rectangle.Top < 0
            || rectangle.Left < 0;
    }
}