using TagsCloudCore.BuildingOptions;
using TagsCloudCore.Common;
using TagsCloudCore.Providers;
using TagsCloudCore.Utils;
using TagsCloudCore.WordProcessing.WordGrouping;
using TagsCloudVisualization;

namespace TagsCloudCore.TagCloudForming;

public class DefaultWordCloudDistributor : IWordCloudDistributorProvider
{
    private readonly ICloudLayouter _cloudLayouter;
    private readonly DrawingOptions _drawingOptions;
    private readonly Result<IReadOnlyDictionary<string, int>> _words;

    public DefaultWordCloudDistributor(IProcessedWordProvider processedWord,
        ICommonOptionsProvider commonOptionsProvider,
        IDrawingOptionsProvider drawingOptionsProvider)
    {
        _words = processedWord.ProcessedWords;
        _cloudLayouter =
            CloudAlgorithmProvider.RegisteredProviders[commonOptionsProvider.CommonOptions.CloudBuildingAlgorithm];
        _drawingOptions = drawingOptionsProvider.DrawingOptions;
    }

    public Result<IReadOnlyDictionary<string, WordData>> DistributedWords => DistributeWords();

    private Result<IReadOnlyDictionary<string, WordData>> DistributeWords()
    {
        if (!_words.IsSuccess)
            return Result.Fail<IReadOnlyDictionary<string, WordData>>(_words.Error);
        
        var distributed = new Dictionary<string, WordData>();
        
        foreach (var (word, frequency) in _words.Value)
        {
            var stringSizeResult = DrawingUtils.GetStringSize(word, frequency,
                _drawingOptions.FrequencyScaling, _drawingOptions.Font);

            if (!stringSizeResult.IsSuccess)
                return Result.Fail<IReadOnlyDictionary<string, WordData>>(stringSizeResult.Error);
            
            var newWord = new WordData(_cloudLayouter.PutNextRectangle(stringSizeResult.Value), frequency);
            distributed.Add(word, newWord);
        }

        return distributed.AsReadOnly();
    }
}