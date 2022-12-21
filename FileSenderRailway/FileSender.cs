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
            return files.Select(file => new FileSendResult(file,
                PrepareFileToSend(file, certificate).Then(doc => sender.Send(doc)).Error));
        }

        public Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate)
        {
            return Result.Of(()=>recognizer.Recognize(file))
                .Then(CheckFormatVersion)
                .Then(CheckTimestamp)
                .Then(doc => doc.WithContent(cryptographer.Sign(doc.Content, certificate)))
                .RefineError("Can't prepare file to send");
        }

        private Result<Document> CheckFormatVersion(Document doc)
        {
            return doc.Format == "4.0" || doc.Format == "3.1"
                ? Result.Ok(doc)
                : Result.Fail<Document>("Invalid format version");
        }

        private Result<Document> CheckTimestamp(Document doc)
        {
            var oneMonthBefore = now().AddMonths(-1);
            return doc.Created > oneMonthBefore
                ? Result.Ok(doc)
                : Result.Fail<Document>("Too old document");
        }
    }
}