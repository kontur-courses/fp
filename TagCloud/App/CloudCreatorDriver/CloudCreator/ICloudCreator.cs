using System.Drawing;
using TagCloud.App.WordPreprocessorDriver.InputStream;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.BoringWords;

namespace TagCloud.App.CloudCreatorDriver.CloudCreator;

public interface ICloudCreator
{
    Result<Bitmap> CreatePicture(FromFileStreamContext streamContext);

    Result<None> AddBoringWordManager(IBoringWords boringWordsManager);
}