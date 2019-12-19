using ResultOf;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer.ResultProcessing.ImageSaving;
using TagsCloudContainer.UserInterface;

namespace TagsCloudContainer.ResultProcessing
{
    public class ResultProcessor : IResultProcessor
    {
        private readonly IImageSaver imageSaver;
        private readonly IResultDisplay resultDisplay;

        public ResultProcessor(IImageSaver imageSaver, IResultDisplay resultDisplay)
        {
            this.imageSaver = imageSaver;
            this.resultDisplay = resultDisplay;
        }

        public void ProcessResult(Result<Bitmap> resultOfBitmap, string filePath, ImageFormat imageFormat)
        {
            if (resultOfBitmap.IsSuccess)
            {
                var bitmap = resultOfBitmap.GetValueOrThrow();
                Result.OfAction(() => imageSaver.SaveBitmap(bitmap, filePath, imageFormat))
                    .RefineError("Failed to save result")
                    .Then(none => resultDisplay.ShowResult(bitmap))
                    .OnFail(error => resultDisplay.ShowError(error));
            }
            else
            {
                resultDisplay.ShowError(resultOfBitmap.Error);
            }
        }
    }
}