﻿using System.Drawing;
using System.Drawing.Imaging;
using ResultOf;
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
                imageSaver.SaveBitmap(bitmap, filePath, imageFormat);
                resultDisplay.ShowResult(bitmap);
            }
            else
            {
                resultDisplay.ShowError(resultOfBitmap.Error);
            }
        }
    }
}