using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.VisualizerActions
{
    public abstract class RestrictionsSetAction : IVisualizerAction
    {
        protected readonly AppSettings appSettings;

        public RestrictionsSetAction(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public abstract string GetActionDescription();

        public abstract string GetActionName();

        protected abstract Restrictions GetRestrictions();

        public void Perform()
        {
            var newRestrictions = GetRestrictions();
            appSettings.Restrictions = newRestrictions;
            if (appSettings.CurrentFile != null)
            {
                if (appSettings.CurrentInterface.TryGetTagCloud(out var newImage))
                    appSettings.ImageHolder.SetImage(newImage);
            }
        }
    }
}