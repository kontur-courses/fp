using System;
using System.Security.Cryptography.X509Certificates;
using ResultOf;

namespace FileSenderRailway
{
    public interface ICryptographer
    {
        byte[] Sign(byte[] content, X509Certificate certificate);
    }

    public interface IRecognizer
    {
        /// <exception cref="FormatException">Not recognized</exception>
        Document Recognize(FileContent file);
    }

    public interface ISender
    {
        /// <exception cref="InvalidOperationException">Can't send</exception>
        Result<Document> Send(Document document);
    }

    public record Document(string Name, byte[] Content, DateTime Created, string Format);

    public record FileContent(string Name, byte[] Content);

    public class FileSendResult
    {
        public FileSendResult(FileContent file, string error = null)
        {
            File = file;
            Error = error;
        }

        public FileContent File { get; }
        public string Error { get; }
        public bool IsSuccess => Error == null;
    }
}