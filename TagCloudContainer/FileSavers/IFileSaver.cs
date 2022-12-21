using System.Drawing;

namespace TagCloudContainer.FileSavers
{
    /// <summary>
    ///  Нужен чтобы можно было реализовать сохранение разных форматов
    /// </summary>
    public interface IFileSaver
    {
        public void SaveCanvas(string pathToSave, Bitmap canvas);
    }
}