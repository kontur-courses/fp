using System.Windows.Forms;

namespace TagCloud.ErrorHandler
{
    public class ErrorHandler : IErrorHandler
    {
        public void HandleError(string errorText)
        {
            MessageBox.Show(errorText);
        }
    }
}