using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ResultOf;

namespace FileSenderRailway
{
    public class FileSender
    {
        private readonly ICryptographer cryptographer;
        private readonly Func<DateTime> now;
        private readonly IRecognizer recognizer;
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
            foreach (var file in files)
            {
                var doc = PrepareFileToSend(file, certificate)
                    .RefineError("Can't prepare file to send")
                    .Then(document => sender.Send(document)
                        .RefineError("Can't send"));

                yield return new FileSendResult(file, doc.Error);
            }
        }

        private Result<Document> PrepareFileToSend(
            FileContent file,
            X509Certificate certificate)
        {
            return Result.Of(() => recognizer.Recognize(file))
                .Then(IsValidFormatVersion)
                .Then(IsValidTimestamp)
                .Then(doc => SignDocument(doc, certificate));
        }

        private Result<Document> SignDocument(Document doc, X509Certificate certificate)
        {
            var result = doc with { Content = cryptographer.Sign(doc.Content, certificate) };
            return result.AsResult();
        }

        private Result<Document> IsValidFormatVersion(Document doc)
        {
            return doc.Format == "4.0" || doc.Format == "3.1"
                ? doc.AsResult()
                : Result.Fail<Document>("Invalid format version");
        }

        private Result<Document> IsValidTimestamp(Document doc)
        {
            var oneMonthBefore = now().AddMonths(-1);
            return doc.Created > oneMonthBefore
                ? doc.AsResult()
                : Result.Fail<Document>("Too old document");
        }
    }
}