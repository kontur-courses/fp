using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ResultOf;

namespace FileSenderRailway
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

        private Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate)
        {
            var doc = recognizer.Recognize(file)
                .Then(IsValidFormatVersion)
                .Then(IsValidTimestamp)
                .Then(doc => new Document(doc.Name,
                    cryptographer.Sign(doc.Content, certificate), doc.Created,
                    doc.Format))
                .RefineError("Can't prepare file to send");
            return doc;
        }

        public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate)
        {
            return from file in files
                let doc = PrepareFileToSend(file, certificate).Then(doc => sender.Send(doc))
                let errorMessage = doc.Error
                select new FileSendResult(file, errorMessage);
        }

        private static Result<Document> IsValidFormatVersion(Document doc)
        {
            return CheckDocument(doc, d => d.Format == "4.0" || d.Format == "3.1", "Invalid format version");
            ;
        }

        private static Result<Document> CheckDocument(Document doc, Func<Document, bool> usl, string error)
        {
            return usl(doc) ? doc : Result.Fail<Document>(error);
        }

        private Result<Document> IsValidTimestamp(Document doc)
        {
            var oneMonthBefore = now().AddMonths(-1);
            return CheckDocument(doc, d => doc.Created > oneMonthBefore, "Too old document");
        }
    }
}