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
    private readonly IProcessedWordProvider _processedWordProvider;

    public DefaultWordCloudDistributor(IProcessedWordProvider processedWordProvider,
        ICommonOptionsProvider commonOptionsProvider,
        IDrawingOptionsProvider drawingOptionsProvider)
    {
        _processedWordProvider = processedWordProvider;
        _cloudLayouter =
            CloudAlgorithmProvider.RegisteredProviders[commonOptionsProvider.CommonOptions.CloudBuildingAlgorithm];
        _drawingOptions = drawingOptionsProvider.DrawingOptions;
    }
    
    public Result<IReadOnlyDictionary<string, WordData>> DistributeWords()
    {
        var wordProcessingResult = _processedWordProvider.ProcessWords();
        if (!wordProcessingResult.IsSuccess)
            return Result.Fail<IReadOnlyDictionary<string, WordData>>(wordProcessingResult.Error);
        
        var distributed = new Dictionary<string, WordData>();
        
        foreach (var (word, frequency) in wordProcessingResult.Value)
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