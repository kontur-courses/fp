using System;
using ErrorHandler;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.UI.Actions
{
    public class SettingsAction : IUiAction
    {
        private readonly IImageSettingsProvider imageSettingsProvider;
        private readonly IUiErrorHandler errorHandler;
        public string Name { get; }

        public SettingsAction(IImageSettingsProvider imageSettingsProvider, IUiErrorHandler errorHandler)
        {
            Name = "Settings";
            this.imageSettingsProvider = imageSettingsProvider;
            this.errorHandler = errorHandler;
        }

        public void Perform(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm(imageSettingsProvider.ImageSettings);
            settingsForm.ShowDialog();
            imageSettingsProvider
                .SetImageSettings(settingsForm.ModifiedSettings)
                .RefineError("Couldn't set new settings")
                .OnFail(errorHandler.PostError);
        }
    }
}