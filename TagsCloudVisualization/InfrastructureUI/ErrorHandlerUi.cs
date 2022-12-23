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
}