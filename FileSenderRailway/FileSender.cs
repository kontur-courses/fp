using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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
                var prepareResult = PrepareFileToSend(file, certificate);
                if (!prepareResult.IsSuccess)
                {
                    yield return new FileSendResult(file, prepareResult.Error);
                    continue;
                }                   
                
                var sendResult = prepareResult
                    .Then(sender.Send)
                    .RefineError("Can't send");

                yield return new FileSendResult(file, sendResult.Error);
            }
        }

        private Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate)
        {
            return recognizer.Recognize(file)
                    .Then(ValidateFormatVersion)
                    .Then(ValidateTimestamp)
                    .RefineError("Can't prepare file to send")
                    .Then(result => result with { Content = cryptographer
                    .Sign(result.Content, certificate) });
        }

        private Result<Document> ValidateFormatVersion(Document doc)
        {
            if (doc.Format == "4.0" || doc.Format == "3.1")
                return Result.Ok(doc);
            return Result.Fail<Document>("Invalid format version");
        }

        private Result<Document> ValidateTimestamp(Document doc)
        {
            if (doc.Created > now().AddMonths(-1))
                return Result.Ok(doc);
            return Result.Fail<Document>("Too old document");
        }
    }
}