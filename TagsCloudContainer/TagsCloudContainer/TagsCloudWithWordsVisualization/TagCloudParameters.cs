using System.Collections.Generic;
using TagsCloudContainer.TagsCloudVisualization;

namespace TagsCloudContainer.TagsCloudWithWordsVisualization
{
    public class TagCloudParameters
    {
        public readonly List<string> Words;
        public readonly CircularCloudLayouter Layouter;
        public readonly double ReductionCoefficient;
        public readonly VisualizationParameters VisualizationParameters;
        public readonly string SaveFormat;

        public TagCloudParameters(List<string> words, CircularCloudLayouter layouter, double reductionCoefficient, VisualizationParameters visualizationParameters, string saveFormat)
        {
            Words = words;
            Layouter = layouter;
            ReductionCoefficient = reductionCoefficient;
            VisualizationParameters = visualizationParameters;
            SaveFormat = saveFormat;
        }
    }
}