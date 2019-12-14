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

        IEnumerable<Result<Document>> PrepareFilesToSend (FileContent[] files
            , X509Certificate certificate)
        {
            foreach (var file in files)
            {
                Document doc = recognizer.Recognize(file);
                if (!IsValidFormatVersion(doc))
                    yield return Result.Fail<Document>("Invalid format version");
                if (!IsValidTimestamp(doc))
                    yield return Result.Fail<Document>("Too old document");
                yield return Result.Ok(doc.ChangeContent(cryptographer.Sign(doc.Content, certificate)));
            }
        }
        public IEnumerable<FileSendResult> SendFiles(FileContent[] files, X509Certificate certificate)
        {
            foreach (var file in files)
            {
                string errorMessage = null;
                try
                {
                    foreach (var document in PrepareFilesToSend(files,certificate))
                        sender.Send(document.Value);
                }
                catch (FormatException e)
                {
                    errorMessage = "Can't prepare file to send. " + e.Message;
                }
                catch (InvalidOperationException e)
                {
                    errorMessage = "Can't send. " + e.Message;
                }
                yield return new FileSendResult(file, errorMessage);
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