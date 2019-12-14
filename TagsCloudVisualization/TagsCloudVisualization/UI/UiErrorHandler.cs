using System.Windows.Forms;

namespace TagsCloudVisualization.UI
{
    public class UiErrorHandler : IUiErrorHandler
    {
        public void PostError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "ErrorHandler", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}