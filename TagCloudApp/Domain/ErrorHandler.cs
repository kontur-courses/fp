using TagCloudCore.Interfaces;

namespace TagCloudApp.Domain;

public class ErrorHandler : IErrorHandler
{
    public void HandleError(string errorMessage)
    {
        MessageBox.Show(errorMessage, "Error!");
    }
}