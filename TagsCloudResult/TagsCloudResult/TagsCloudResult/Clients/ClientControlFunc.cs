using ResultOf;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public class ClientControlFunc
    {
        private readonly IClient client;

        public ClientControlFunc(IClient client)
        {
            this.client = client;
        }

        internal bool IsFinish()
        {
            return client.IsFinish();
        }

        public Result<ImageSettings> GetImageSettings()
        {
            var dict = Result.Ok(new Dictionary<string, object>());

            dict = dict.Then(d => d.CombineResults(client.GetImageSize(), "imageSize"))
                .Then(d => d.CombineResults(client.GetFontFamily(), "fontFamily"))
                .Then(d => d.CombineResults(client.GetTextColor(), "textColor"))
                .Then(d => d.CombineResults(client.GetBackgoundColor(), "backgroundColor"));

            if (!dict.IsSuccess)
            {
                return Result.Fail<ImageSettings>(dict.RefineError("Данные некорректны").Error);
            }

            return dict.Then(value => new ImageSettings(
                (Size)value["imageSize"],
                (FontFamily)value["fontFamily"],
                (Color)value["textColor"],
                (Color)value["backgroundColor"]
                ));
        }

        public Result<string> GetNameForImage() => client.GetNameForImage();

        public void ShowPathToImage(string path) => client.ShowPathToNewFile(path);
        public void ShowMessage(string path) => client.ShowMessage(path);
    }
}
