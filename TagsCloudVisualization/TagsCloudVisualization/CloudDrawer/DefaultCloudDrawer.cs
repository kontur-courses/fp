﻿using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.CloudDrawer;

public class DefaultCloudDrawer : ICloudDrawer
{
    private const string DefaultPath = "..\\..\\..\\Result.png";

    private Bitmap bitmap;
    private Graphics graphics;
    private SolidBrush[] brushes;

    public DefaultCloudDrawer(int windowWidth, int windowHeight, IEnumerable<Color> colors)
    {
        bitmap = new Bitmap(windowWidth, windowHeight);
        graphics = Graphics.FromImage(bitmap);
        if (colors.Count() == 0)
            colors = new[] { Config.DefaultTextColor };
        brushes = colors.Select(c => new SolidBrush(c)).ToArray();
    }

    public void Draw(List<TextLabel> wordsInPoint)
    {
        int i = 0;
        foreach (var textLabel in wordsInPoint)
        {
            graphics.DrawString(textLabel.Content, textLabel.Font, brushes[i++ % brushes.Length],
                textLabel.Rectangle.Location);
            if (!textLabel
                    .GetСornersPositions()
                    .All(point => 0 <= point.X && point.X <= bitmap.Width && 0 <= point.Y && point.Y <= bitmap.Height))
                throw new Exception("Cloud does not fit into the window");
        }

        SaveImage();
    }

    public void SaveImage()
    {
        bitmap.Save(DefaultPath, ImageFormat.Png);
    }
}