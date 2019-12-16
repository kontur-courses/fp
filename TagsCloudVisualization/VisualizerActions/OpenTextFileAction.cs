using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.VisualizerActions
{
    public abstract class OpenTextFileAction : IVisualizerAction
    {
        protected readonly AppSettings appSettings;

        public OpenTextFileAction(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public abstract string GetActionDescription();

        public abstract string GetActionName();

        protected abstract bool TryGetCorrectFileToOpen(out string filepath);

        public void Perform()
        {
            if (TryGetCorrectFileToOpen(out var filepath))
            {
                appSettings.CurrentFile = filepath;
                if (appSettings.CurrentInterface.TryGetTagCloud(out var newImage))
                    appSettings.ImageHolder.SetImage(newImage);
            }
        }
    }
}