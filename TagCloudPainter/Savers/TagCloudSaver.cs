using System.Drawing.Imaging;
using TagCloudPainter.Builders;
using TagCloudPainter.FileReader;
using TagCloudPainter.Painters;
using TagCloudPainter.Preprocessors;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Savers;

public class TagCloudSaver : ITagCloudSaver
{
    private readonly ITagCloudElementsBuilder _builder;
    private readonly ICloudPainter _painter;
    private readonly IFileReader _reader;
    private readonly IWordPreprocessor _wordPreprocessor;

    public TagCloudSaver(IFileReader reader, IWordPreprocessor wordPreprocessor, ITagCloudElementsBuilder builder,
        ICloudPainter painter)
    {
        _reader = reader;
        _wordPreprocessor = wordPreprocessor;
        _builder = builder;
        _painter = painter;
    }

    public Result<None> SaveTagCloud(string inputPath, string outputPath, ImageFormat format)
    {
        return _reader.ReadFile(inputPath).Then(_wordPreprocessor.GetWordsCountDictionary).Then(_builder.GetTags)
            .Then(_painter.PaintTagCloud).Then(p => p.Save(outputPath, format));
    }
}