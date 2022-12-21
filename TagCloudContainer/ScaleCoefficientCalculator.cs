using System;

namespace TagCloudContainer
{
    /// <summary>
    /// Считает коэффицент для размера шрифта в зависимости от размера холста.
    /// Гарантирует что картинка не поедет при нестандартных размерах.
    /// </summary>
    public static class ScaleCoefficientCalculator
    {
        public static int CalculateScaleCoefficient(int canvasWidth, int canvasHeight, int canvasBorder)
        {
            if (canvasBorder < 0)
                throw new ArgumentException("Borders can't be less than zero");
            if (canvasHeight < canvasBorder * 2)
                throw new ArgumentException("Too small canvas height");
            if (canvasWidth < canvasBorder * 2)
                throw new ArgumentException("Too small canvas width");
            
            return Math.Min(canvasWidth - canvasBorder, canvasHeight - canvasBorder) / 100 * 2;
        }
    }
}