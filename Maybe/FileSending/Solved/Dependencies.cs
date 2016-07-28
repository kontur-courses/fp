using System;
using System.Security.Cryptography.X509Certificates;

namespace FileSending.Solved
{
	public class Document
	{
		public Document(string name, byte[] content, DateTime created, string format)
		{
			Name = name;
			Created = created;
			Format = format;
			Content = content;
		}

		public string Name { get; }
		public DateTime Created { get; }
		public string Format { get; }
		public byte[] Content { get; }
		public Document WithContent(byte[] newContent) 
			=> new Document(Name, newContent, Created, Format);
	}

	public class FileContent
	{
		public FileContent(string name, byte[] content)
		{
			Name = name;
			Content = content;
		}

		public string Name { get; }
		public byte[] Content { get; }
	}

	public class FileSendResult
	{
		public FileSendResult(FileContent file, CSharpFunctionalExtensions.Result result)
		{
			File = file;
			Result = result;
		}

		public FileContent File { get; }
		public CSharpFunctionalExtensions.Result Result { get; }
	}

	public interface ICryptographer
	{
		byte[] Sign(byte[] content, X509Certificate certificate);
	}

	public interface IRecognizer
	{
		CSharpFunctionalExtensions.Result<Document> Recognize(FileContent file);
	}

	public interface ISender
	{
		CSharpFunctionalExtensions.Result Send(Document content);
	}
}