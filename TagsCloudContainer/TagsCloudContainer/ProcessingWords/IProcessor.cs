using CloudDrawing;
using ResultOf;

namespace TagsCloudContainer.ProcessingWords
{
    public interface IProcessor
    {
        Result<None> Run(string pathToFile, string pathToSaveFile, ImageSettings imageSettings,
            WordDrawSettings wordDrawSettings);
    }
}