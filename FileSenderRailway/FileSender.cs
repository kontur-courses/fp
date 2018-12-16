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

                var doc = PrepareFileToSend(file, certificate);
                if (doc.IsSuccess)
                    sender.Send(doc.Value);

                yield return new FileSendResult(file, doc.Error);
            }
        }

        public Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate)
        {           
            var resultDoc = recognizer.Recognize(file);
            if (!resultDoc.IsSuccess)
                return Result.Fail<Document>("Can't recognize");
            if (!IsValidFormatVersion(resultDoc.Value))
                return resultDoc.RefineError($"Can't prepare file to send. Invalid format version {resultDoc.Value.Format}");
            if (!IsValidTimestamp(resultDoc.Value))
                return resultDoc.RefineError($"Can't prepare file to send. Too old document {resultDoc.Value.Created}");
                return resultDoc.Value.DocumentUpdate(cryptographer.Sign(resultDoc.Value.Content, certificate));
        }

        private bool IsValidFormatVersion(Document doc)
        {
            return doc.Format == "4.0" || doc.Format == "3.1";
        }

        private bool IsValidTimestamp(Document doc)
        {
            var oneMonthBefore = now().AddMonths(-1);
            return doc.Created > oneMonthBefore;
        }
    }
}