using System.Drawing;
using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudContainer.Settings
{
    public class DefaultColorSettings : IDefaultColorSettings
    {
        public Color Color { get; set; } = Color.Aqua;
    }
}