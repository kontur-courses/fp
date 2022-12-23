using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.InfrastructureUI;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization
{
    public class CloudForm : Form
    {
        private readonly ImageSettings imageSettings;

        public CloudForm(IEnumerable<IUiAction> actions,
            PictureBoxImageHolder pictureBox,
            ImageSettings imageSettings)
        {
            this.imageSettings = imageSettings;

            ClientSize = new Size(imageSettings.Width, imageSettings.Height);

            pictureBox.RecreateImage(imageSettings)
                .RefineError("измените размеры изображения")
                .OnFail(Error.HandleError<ErrorHandlerUi>);

            var mainMenu = new MenuStrip();
            mainMenu.Items.AddRange(actions.ToMenuItems());
            Controls.Add(mainMenu);


            pictureBox.Dock = DockStyle.Fill;
            Controls.Add(pictureBox);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            imageSettings.Height = Size.Height;
            imageSettings.Width = Size.Width;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Text = "Cloud painter";
        }
    }
}