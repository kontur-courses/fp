using System;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.TextProcessing;

namespace TagsCloud_Test
{
    public class MyStemManager_Test
    {
        private MyStemManager _myStemManager;

        [SetUp]
        public void SetUp()
        {
            _myStemManager = new MyStemManager();
        }

        [Test]
        public void GetLexemesFrom_Failure_WhenMyStemErrorOutNotEmpty_BecauseOfIncorrectEncoding()
        {
            var lexemes = _myStemManager.GetLexemesFrom("text_ansi.txt");
            lexemes.IsSuccess.Should().BeFalse();
            Console.WriteLine(lexemes.Error);
        }

        [Test]
        public void GetLexemesFrom_Success_WhenMyStemWorkedNormally_OnCorrectFile()
        {
            var path = Path.Combine(Path.GetTempPath(), "test.txt");
            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            writer.WriteLine("Новый тестовый файл в кодировке UTF-8");
            var lexemes = _myStemManager.GetLexemesFrom(path);
            lexemes.IsSuccess.Should().BeTrue();
        }
    }
}