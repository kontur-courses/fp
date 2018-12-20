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
                string errorMessage = null;

                var docResult = PrepareFileToSend(
                    file,
                    recognizer.Recognize,
                    ValidateDocument,
                    cryptographer.Sign, 
                    certificate);
                docResult
                    Скотт Влашин Railway
                    .Then(preparedDocument => 
                        sender.Send(preparedDocument).RefineError("Can't send"))
                    .OnFail(message => errorMessage = message);
                yield return new FileSendResult(file, errorMessage);
            }
        }
        
        private static Result<Document> PrepareFileToSend(FileContent inputFile, 
            Func<FileContent, Result<Document>> recognizeFile,
            Func<Document, Result<Document>> validateDocument, Func<Document, X509Certificate, Document> signDocument,
            X509Certificate certificate)
        {
            var rec = Result.Of(()=> recognizeFile(inputFile)
                .Then(recognizedFile => recognizedFile)
                

            return recognizeFile(inputFile)
                .Then(doc => validateDocument(doc))
                .Then(doc => signDocument(doc, certificate))
                .RefineError("Can't prepare file to send");
        }

        private Result<Document> ValidateDocument(Document doc)
        {
            if (!IsValidFormatVersion(doc))
                return new Result<Document>("Invalid format version " + doc.Format);
            if (!IsValidTimestamp(doc))
                return new Result<Document>("Too old document " + doc.Created);
            return doc;
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