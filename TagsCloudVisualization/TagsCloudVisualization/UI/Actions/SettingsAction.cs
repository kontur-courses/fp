using System;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.UI.Actions
{
    public class SettingsAction : IUiAction
    {
        private readonly IImageSettingsProvider imageSettingsProvider;
        public string Name { get; }

        public SettingsAction(IImageSettingsProvider imageSettingsProvider)
        {
            Name = "Settings";
            this.imageSettingsProvider = imageSettingsProvider;
        }

        public void Perform(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm(imageSettingsProvider.ImageSettings);
            settingsForm.ShowDialog();
            //TODO PROCESS FAIL
            imageSettingsProvider.SetImageSettings(settingsForm.ModifiedSettings);
        }
    }
}