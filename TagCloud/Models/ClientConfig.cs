using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    public class ClientConfig
    {
        public ClientConfig(HashSet<string> availableFontNames,HashSet<string> availablePaletteNames
            ,ICloudVisualization visualization)
        {
            Visualization = visualization;
            ToCreateNewImage = false;
            ToExit = false;
            ImageToSave = null;
            IsRunning = false;
            AvailableFontNames = availableFontNames;
            AvailablePaletteNames = availablePaletteNames;
        }

        public ICloudVisualization Visualization { get; }
        public bool IsRunning { get; set; }
        public bool ToExit { get; set; }
        public bool ToCreateNewImage { get; set; }
        public Bitmap ImageToSave { get; set; }
        public HashSet<string> AvailableFontNames { get; }
        public HashSet<string> AvailablePaletteNames { get; }
    }
}