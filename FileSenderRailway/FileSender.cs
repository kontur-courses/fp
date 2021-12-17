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

        public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate)
        {
            return from file in files
                let sendResult = PrepareFileToSend(file, certificate).Then(sender.Send)
                select new FileSendResult(file, sendResult.Error);
        }

        private Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate)
        {
            return Result.Of(() => recognizer.Recognize(file))
                .Then(IsValidFormatVersionResult)
                .Then(IsValidTimestampResult)
                .Then(x => SignDocument(x, certificate))
                .RefineError("Can't prepare file to send");
        }

        private static bool IsValidFormatVersion(Document doc)
        {
            return doc.Format == "4.0" || doc.Format == "3.1";
        }

        private static Result<Document> IsValidFormatVersionResult(Document doc)
        {
            return !IsValidFormatVersion(doc)
                ? Result.Fail<Document>("Invalid format version")
                : doc;
        }

        private bool IsValidTimestamp(Document doc)
        {
            var oneMonthBefore = now().AddMonths(-1);
            return doc.Created > oneMonthBefore;
        }

        private Result<Document> IsValidTimestampResult(Document doc)
        {
            return !IsValidTimestamp(doc)
                ? Result.Fail<Document>("Too old document")
                : doc;
        }

        private Result<Document> SignDocument(Document doc, X509Certificate certificate)
        {
            return Result.Of(() => doc.WithContent(cryptographer.Sign(doc.Content, certificate)));
        }
    }
}