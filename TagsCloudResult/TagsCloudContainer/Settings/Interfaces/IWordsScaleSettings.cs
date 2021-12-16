using TagsCloudContainer.MathFunctions;

namespace TagsCloudContainer.Settings.Interfaces
{
    public interface IWordsScaleSettings
    {
        IMathFunction Function { get; set; }
    }
}