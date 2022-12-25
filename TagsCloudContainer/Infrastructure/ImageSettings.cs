using ResultOf;

namespace TagsCloudContainer.Infrastructure
{
    public class ImageSettings
    {
        public int Width { get; set; } = 1200;
        public int Height { get; set; } = 700;

        public ImageSettings()
        {
            SetDefaultSetting();
        }

        public void SetDefaultSetting()
        {
            Width = 1200;
            Height = 700;
        }

        public Result<ImageSettings> CheckIsSettingValid()
        {
            return (Width < 0 || Height < 0)
                ? Result.Fail<ImageSettings>("Настройки неверные. Высота и ширина должны быть положительными числами")
                : Result.Ok(this);
        }
    }
}