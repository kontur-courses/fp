public static class ErrorMessageBox
{
    public static void ShowError(string errorText)
    {
        MessageBox.Show(errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}