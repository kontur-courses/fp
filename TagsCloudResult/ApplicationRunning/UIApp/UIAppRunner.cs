using System.Windows.Forms;
using TagsCloudContainer.ApplicationRunning.UIApp.Forms;

namespace TagsCloudResult.ApplicationRunning.UIApp
{
    public class UIAppRunner : IAppRunner
    {
        private readonly TagsCloud cloud;
        private readonly SettingsManager settings;

        public UIAppRunner(TagsCloud cloud, SettingsManager settings)
        {
            this.cloud = cloud;
            this.settings = settings;
        }

        public void Run()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm(cloud, settings));
        }
    }
}