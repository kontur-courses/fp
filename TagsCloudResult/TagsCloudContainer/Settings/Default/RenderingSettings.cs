﻿using System.Drawing;

namespace TagsCloudContainer.Settings.Default
{
    public class RenderingSettings : IRenderingSettings
    {
        public Size? DesiredImageSize { get; set; }
        public float Scale { get; set; }
        public Brush Background { get; set; } = new SolidBrush(Color.Transparent);
    }
}