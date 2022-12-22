using System;
using TagCloudContainer.Result;

namespace TagCloudContainer
{
    /// <summary>
    /// Считает коэффицент для размера шрифта в зависимости от размера холста.
    /// Гарантирует что картинка не поедет при нестандартных размерах.
    /// </summary>
    public static class ScaleCoefficientCalculator
    {
        public static Result<int> CalculateScaleCoefficient(int canvasWidth, int canvasHeight, int canvasBorder)
        {
            if (canvasBorder < 0)
                return new Result<int>("Borders can't be less than zero");
            if (canvasHeight < canvasBorder * 2)
                return new Result<int>("Too small canvas height");
            if (canvasWidth < canvasBorder * 2)
                return new Result<int>("Too small canvas width");
            return new Result<int>(null, Math.Min(canvasWidth - canvasBorder, canvasHeight - canvasBorder) / 100 * 2);
        }
    }
}