using System.Windows.Forms;

namespace TagsCloud.App.Infrastructure;

public static class ErrorHandler
{
    public static void HandleError(string error)
    {
        MessageBox.Show(error, "Ошибочная ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}