﻿using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace FP
{
    [TestFixture]
    public class SummatorTests
    {
        [SetUp]
        public void SetUp()
        {
            Directory.SetCurrentDirectory(
                TestContext.CurrentContext.WorkDirectory);
            summator = new Summator(
                () => new DataSource(LargeInputFilename),
                new HexSumFormatter(),
                OutputFilename);
        }

        private Summator summator;
        private const string LargeInputFilename = "process-large-file.txt";
        private const string OutputFilename = "process-result.txt";
        private const string ExpectedOutputFilename = "expected-process-result.txt";

        [Test]
        [Explicit("Генератор данных. Не нужен для выполнения задания")]
        public void GenerateInput()
        {
            var r = new Random();
            File.WriteAllLines(LargeInputFilename,
                Enumerable.Range(0, 1000).Select(i =>
                    string.Join(
                        " ",
                        Enumerable.Range(0, 4).Select(j => Convert.ToString(r.Next(1000), 16))
                    )));
        }


        [Test]
        public void Process_GeneratesCorrectOutputFile()
        {
            var actualResultFile = new FileInfo(OutputFilename);
            if (actualResultFile.Exists) actualResultFile.Delete();

            summator.ProcessOld();

            CollectionAssert.AreEqual(
                File.ReadAllLines(ExpectedOutputFilename),
                File.ReadAllLines(actualResultFile.FullName));
        }

        [Test]
        public void Process_ShowProgressOnConsole()
        {
            var stdOut = Console.Out;
            try
            {
                var consoleOutput = new StringWriter();
                Console.SetOut(consoleOutput);

                summator.ProcessOld();

                var actualOutput = consoleOutput.ToString()
                    .TrimEnd()
                    .Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
                Assert.AreEqual("processed 100 items", actualOutput.First());
                Assert.AreEqual("processed 1000 items", actualOutput.Last());
                Assert.AreEqual(10, actualOutput.Length);
            }
            finally
            {
                Console.SetOut(stdOut);
            }
        }
    }
}