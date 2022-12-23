using System.Windows.Forms;

namespace TagsCloudVisualization.InfrastructureUI
{
    public class ErrorHandlerUi : IErrorHandler
    {
        public void HandleError(string error)
        {
            MessageBox.Show(error);
        }
    }

    public static class Error
    {
        public static void HandleError<T>(string error)
            where T : IErrorHandler, new()
        {
            new T().HandleError(error);
        }
    }
}