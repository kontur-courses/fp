using System.Windows.Forms;

namespace TagCloud.ExceptionHandler
{
    public class MessageBoxHandler : IExceptionHandler
    {
        public void HandleException(string message)
        {
            MessageBox.Show(message, "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}