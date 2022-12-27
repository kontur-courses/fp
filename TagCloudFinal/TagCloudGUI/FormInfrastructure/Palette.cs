using System.ComponentModel;

namespace TagCloudGUI
{
    public class Palette
    {
        private Color primary = Color.Yellow;
        private Color secondary = Color.DeepPink;
        private Color background = Color.Black;

        [DisplayName("Цвет максимума")]
        public Color PrimaryColor 
        { 
            get => primary;
            set
            {
                try
                {
                    primary = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        } 
        [DisplayName("Цвет минимума")]
        public Color SecondaryColor 
        {
            get => secondary;
            set
            {
                try
                {
                    secondary = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        
        }
        [DisplayName("Цвет фона")]
        public Color BackgroundColor 
        {
            get => background;
            set
            {
                try
                {
                    background = value;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}
