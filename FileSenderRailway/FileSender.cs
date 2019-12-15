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

        internal Document TryPrepareFileToSend(FileContent file, X509Certificate certificate, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                var doc = recognizer.Recognize(file);
                if (!IsValidFormatVersion(doc))
                    throw new FormatException("Invalid format version");
                if (!IsValidTimestamp(doc))
                    throw new FormatException("Too old document");
                doc = doc.SetContent(cryptographer.Sign(doc.Content, certificate));
                
                return doc;
                
            }
            catch (FormatException e)
            {
                errorMessage = "Can't prepare file to send. " + e.Message;
            }

            return null;
        }
        
        public IEnumerable<Result<Document>> SendFiles(FileContent[] files, X509Certificate certificate)
        {
            foreach (var file in files)
            {
                var doc = TryPrepareFileToSend(file, certificate, out var errorMessage);

                if (doc != null)
                {
                    try
                    {
                        sender.Send(doc);
                    }
                    catch (InvalidOperationException e)
                    {
                        errorMessage = "Can't send. " + e.Message;
                    }
                }
                
                yield return new Result<Document>(errorMessage, doc);
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