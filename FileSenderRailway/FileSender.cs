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

        Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate)
        {
            Document doc = recognizer.Recognize(file);
            if (!IsValidFormatVersion(doc))
                return Result.Fail<Document>("Invalid format version");
            if (!IsValidTimestamp(doc))
                return Result.Fail<Document>("Too old document");
            return Result.Ok(doc.ChangeContent(cryptographer.Sign(doc.Content, certificate)));
        }

        public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate)
        {
            foreach (var file in files)
            {
                string errorMessage = null;

                var result = PrepareFileToSend(file, certificate);

                if (result.IsSuccess)
                {
                    var res = sender.Send(result.Value);
                    if(!res.IsSuccess)
                        errorMessage = "Can't send";
                }

                errorMessage = errorMessage ?? "Can't prepare file to send";
                yield return new FileSendResult(file, result.RefineError(errorMessage).Error);
            }
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