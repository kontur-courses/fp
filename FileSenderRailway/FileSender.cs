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

        public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate) =>
            from file in files 
            let doc = recognizer.Recognize(file) 
            let result = PrepareDocument(doc, certificate).Then(sender.Send) 
            select new FileSendResult(file, result.Error);

        private Result<Document> PrepareDocument(Document doc, X509Certificate certificate) =>
            ValidateDocument(doc)
                .Then(d => SignFileToSend(certificate, d))
                .RefineError("Can't prepare file to send");

        private Result<Document> ValidateDocument(Document document)
        {
            if (!IsValidFormatVersion(document))
                return Result.Fail<Document>("Invalid format version");
            if (!IsValidTimestamp(document))
                return Result.Fail<Document>("Too old document");
            return Result.Ok(document);
        }

        public Result<Document> SignFileToSend(X509Certificate certificate, Document doc)
        {
            var signedContent = cryptographer.Sign(doc.Content, certificate);
            return new Document(doc.Name, signedContent, doc.Created, doc.Format);
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