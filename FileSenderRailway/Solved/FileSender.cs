using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CSharpFunctionalExtensions;

namespace FileSenderRailway.Solved
{
	public class FileSender
	{
		private readonly ICryptographer cryptographer;
		private readonly IRecognizer recognizer;
		private readonly Func<DateTime> now;
		private readonly ISender sender;

		public FileSender(
			ICryptographer cryptographer,
			ISender sender,
			IRecognizer recognizer,
			Func<DateTime> now)
		{
			this.cryptographer = cryptographer;
			this.sender = sender;
			this.recognizer = recognizer;
			this.now = now;
		}

		public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate)
		{
			return files.Select(file => new FileSendResult(file,
				PrepareFileToSend(file, certificate).OnSuccess(doc => sender.Send(doc))));
		}

		public Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate)
		{
			return recognizer.Recognize(file)
				.OnSuccess(doc => ValidateFormatIsSupported(doc))
				.OnSuccess(doc => ValidateIsNotTooOld(doc))
				.OnSuccess(doc => doc.WithContent(cryptographer.Sign(doc.Content, certificate)));
		}

		private Result<Document> ValidateFormatIsSupported(Document doc)
		{
			return Validate(doc, d => d.Format == "4.0" || d.Format == "3.1", $"Неверный формат Format={doc.Format}");
		}

		private Result<Document> ValidateIsNotTooOld(Document doc)
		{
			var oneMonthBefore = now().AddMonths(-1);
			return Validate(doc, d => d.Created > oneMonthBefore, $"Слишком старый документ. Created={doc.Created} ");
		}

		private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage)
		{
			return predicate(obj)
				? Result.Ok(obj)
				: Result.Fail<T>(errorMessage);
		}
	}
}