using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.Visualizators;

namespace TagsCloudContainer
{
    public class VisualizatorSettingsProvider : IVisualizatorSettingsProvider
    {
        public IVisualizatorSettings GetVisualizatorSettings()
        {
            return new TagsVisualizatorSettings(AppSettings.ImageFilename,
                AppSettings.ImageSize,
                AppSettings.BackgroundColor,
                AppSettings.FontFamily,
                AppSettings.MinMargin,
                AppSettings.FillTags);
        } 
    }
}