using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TagCloud
{
    public class ShowImageForm : Form
    {
        private Bitmap image;
        public ShowImageForm(Bitmap image)
        {
            this.image = image;
            ClientSize = new Size(image.Width, image.Height);
            var pictureBox = new PictureBox
            {
                Image = image,
                Location = new Point(0, 0),
                Size = ClientSize
            };
            Controls.Add(pictureBox);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ShowImageForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "ShowImageForm";
            this.Load += new System.EventHandler(this.ShowImageForm_Load_1);
            this.ResumeLayout(false);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            image.Dispose();
            base.OnClosing(e);
        }

        private void ShowImageForm_Load(object sender, EventArgs e)
        {
        }

        private void ShowImageForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}