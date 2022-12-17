using System;
using System.IO;
using System.Text;
using TagsCloud.Interfaces;

namespace TagsCloud
{
    public class ConsoleClient : IClient
    {
        public string TextFilePath => basePath + textFileName;
        public string PicFilePath => basePath + picFileName;
        public string PicFileExtension { get; private set; }

        private readonly string basePath;
        private string textFileName;
        private string picFileName;

        private readonly StringBuilder textFileBuilder;

        private readonly IRecognizer<string> recognizer;
        private readonly IClientValidator validator;


        public ConsoleClient(IRecognizer<string> recognizer, IClientValidator validator, string basePath)
        {
            this.basePath = basePath;
            this.recognizer = recognizer;
            this.validator = validator;

            textFileBuilder = new StringBuilder();
        }

        public void StartClient()
        {
            EnterTextName();

            EnterPictureName();

            EnterPictureExtension();

            Console.WriteLine("Enter one word at a time in the stack. If you want to finish, type \"stop\"");

            FillTextFile();
        }

        private void EnterTextName()
        {
            while (true)
            {
                Console.WriteLine("Enter text file name to save input text:");
                var newLine = Console.ReadLine();

                var validResult = recognizer.Recognize(newLine)
                    .Then(validator.ValidateWrongSymbolsInPath)
                    .Then(validator.ValidateRightTextExtension)
                    .RefineError("Wrong text file name");

                if (validResult.IsSuccess)
                {
                    textFileName = newLine;
                    break;
                }

                Console.WriteLine(validResult.Error);
            }
        }

        private void EnterPictureName()
        {
            while (true)
            {
                Console.WriteLine("Enter picture file name to save:");
                var newLine = Console.ReadLine();

                var validResult = recognizer.Recognize(newLine)
                    .Then(validator.ValidateWrongSymbolsInPath);

                if (validResult.IsSuccess)
                {
                    picFileName = newLine;
                    break;
                }

                Console.WriteLine(validResult.Error);
            }
        }

        private void EnterPictureExtension()
        {
            while (true)
            {
                Console.WriteLine("Enter picture file extension to save:");
                var newLine = Console.ReadLine();

                var validResult = recognizer.Recognize(newLine)
                    .Then(validator.ValidateRightPictureExtension);

                if (validResult.IsSuccess)
                {
                    PicFileExtension = newLine;
                    break;
                }

                Console.WriteLine(validResult.Error);
            }
        }

        private void FillTextFile()
        {
            while (true)
            {
                var newLine = Console.ReadLine();

                if (newLine == "stop")
                {
                    break;
                }

                var validResult = recognizer.Recognize(newLine)
                    .Then(validator.ValidateRussianInput);

                if (validResult.IsSuccess) textFileBuilder.Append(newLine + '\n');
                else Console.WriteLine("Invalid word. Please, retype.");
            }

            SaveTextFile();
        }

        private void SaveTextFile()
        {
            using (var writer = new StreamWriter(basePath + textFileName, true, Encoding.Default))
            {
                var text = textFileBuilder.ToString();
                writer.WriteLine(text);
            }
        }
    }
}