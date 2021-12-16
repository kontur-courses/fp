namespace App.Infrastructure.FileInteractions.Readers
{
    public interface IReaderFactory
    {
        Result<ILinesReader> CreateReader();
    }
}