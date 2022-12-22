using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml.XPath;
using ResultOf;

namespace FileSenderRailway
{
    class FileSender 
    {
        private readonly ICryptographer cryptographer;
        private readonly IRecognizer recognizer;
        private readonly Func<DateTime> now;
        private readonly ISender sender;
        private Result<Document> _document = new Result<Document>();

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
                PrepareFileToSend(file, certificate);
                if (_document.IsSuccess) 
                    sender.Send(_document.Value);
                yield return new FileSendResult(file, _document.Error);
            }
        }

        private FileSender IsValidFormatVersion()
        {
            if (_document.Value.Format == "4.0" || _document.Value.Format == "3.1")
                _document = Result.Fail<Document>("Invalid format version");
            return this;
        }

        private FileSender IsValidTimestamp()
        {
            var oneMonthBefore = now().AddMonths(-1);
            if (_document.Value.Created > oneMonthBefore)
                _document = Result.Fail<Document>("Too old document");
            return this;
        }

        private FileSender For(Result<Document> document)
        {
            _document = document; 
            return this;
        }

        private FileSender IsSuccessed()
        {
            if (!_document.IsSuccess)
                _document.RefineError("Can't prepare file to send. ");
            return this;
        }

        private FileSender WithContent(Result<byte[]> content)
        {
            if (!content.IsSuccess)
                content.RefineError("Can't send. " + content.Error);
            _document = _document.Value.WithContent(content.Value);
            return this;
        }

        public void PrepareFileToSend(FileContent file, X509Certificate certificate)
        {
            For(recognizer.Recognize(file))
                .IsValidTimestamp()
                .IsValidFormatVersion()
                .IsSuccessed()
                .WithContent(cryptographer.Sign(_document.Value.Content, certificate).AsResult());
        }
    }
}