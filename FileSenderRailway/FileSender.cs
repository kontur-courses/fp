using System;
using System.Collections.Generic;
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
            foreach (var file in files)
            {
                var doc = PrepareFileToSend(file, certificate).RefineError("Can't prepare file to send");
                var res = Result.OfAction(() => sender.Send(doc.Value)).RefineError("Can't send");
                if (!doc.IsSuccess)
                    yield return new FileSendResult(file, doc.Error);
                else if (!res.IsSuccess)
                    yield return new FileSendResult(file, res.Error);
                else
                    yield return new FileSendResult(file);
            }
        }

        public Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate)
        {
            var doc = recognizer.Recognize(file)
                .Then(d => IsValidFormatVersion(d))
                .Then(d => IsValidTimestamp(d));
            return !doc.IsSuccess 
                ? doc 
                : doc.Value.WithContent(cryptographer.Sign(doc.Value.Content, certificate));
        }

        private Result<Document> IsValidFormatVersion(Result<Document> doc)
        {
            if (doc.Value == null || doc.Value.Format == "4.0" || doc.Value.Format == "3.1")
                return doc;
            return Result.Fail<Document>("Invalid format version");
        }

        private Result<Document> IsValidTimestamp(Result<Document> doc)
        {
            var oneMonthBefore = now().AddMonths(-1);
            return doc.Value == null || doc.Value.Created > oneMonthBefore 
                ? doc 
                : Result.Fail<Document>("Too old document");
        }
    }
}