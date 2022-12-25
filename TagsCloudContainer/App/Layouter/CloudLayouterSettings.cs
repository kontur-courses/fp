using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.App.Layouter
{
    public class CloudLayouterSettings
    {
        public Point Center { get; set; }
        public double OffsetPoint { get; set; }
        public double SpiralStep { get; set; }
        public bool IsOffsetToCenter { get; set; }

        public CloudLayouterSettings()
        {
            SetDefaultSetting();
        }
       
        public void SetDefaultSetting()
        {
            Center = new Point(600, 350);
            OffsetPoint = 0.01;
            SpiralStep = -0.3;
            IsOffsetToCenter = false;
        }

        public Result<CloudLayouterSettings> CheckIsSettingValid()
        {
           return (Center.X < 0 ||Center.Y < 0)
                ? Result.Fail<CloudLayouterSettings>("Настройки неверные. Координаты точки должны быть положительными числами")
                : Result.Ok(this);
        }
    }
}