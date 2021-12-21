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
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var file in files)
            {
                var result = PrepareFileToSend(file, certificate)
                    .Then(doc => sender.Send(doc));

                yield return new FileSendResult(file, result.Error);
                // try
                // {
                //
                //     Document doc = recognizer.Recognize(file);
                //     if (!IsValidFormatVersion(doc))
                //         throw new FormatException("Invalid format version");
                //     if (!IsValidTimestamp(doc))
                //         throw new FormatException("Too old document");
                //     doc.Content = cryptographer.Sign(doc.Content, certificate);
                //     sender.Send(doc);
                // }
                // catch (FormatException e)
                // {
                //     errorMessage = "Can't prepare file to send. " + e.Message;
                // }
                // catch (InvalidOperationException e)
                // {
                //     errorMessage = "Can't send. " + e.Message;
                // }
                //
                // yield return new FileSendResult(file, errorMessage);
            }
        }

        // private Result<Document> VerifyDocument(Document document)
        // {
        //     if (!IsValidFormatVersion(document))
        //         return Result.Fail<Document>("Invalid format version");
        //
        //     return !IsValidTimestamp(document)
        //         ? Result.Fail<Document>("Too old document")
        //         : document;
        // }

        private Result<Document> PrepareFileToSend(FileContent file, X509Certificate certificate)
        {
            return Result.Of(() => recognizer.Recognize(file))
                .Then(VerifyVersion)
                .Then(VerifyTime)
                .RefineError("Can't prepare file to send")
                .Then(doc =>
                    new Document(doc.Name
                        , cryptographer.Sign(doc.Content, certificate),
                        doc.Created,
                        doc.Format));
        }

        private Result<Document> VerifyVersion(Document document)
            => IsValidFormatVersion(document)
                ? document
                : Result.Fail<Document>("Invalid format version");

        private Result<Document> VerifyTime(Document document)
            => !IsValidTimestamp(document)
                ? Result.Fail<Document>("Too old document")
                : document;

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