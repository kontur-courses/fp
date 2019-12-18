using NUnit.Framework;
using System;
using TagsCloudContainer.Readers;

namespace TagsCloudContainerTests
{
    [TestFixture]
    class SimpleReaderTests
    {
        [Test]
        public void ReadAllLines_Docx()
        {
            var simpleReader = new SimpleReader(System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Words.docx"));

            var result = simpleReader.ReadAllLines();

            var expect = new[] { "A", "A", "A", "A", "D", "D", "B", "B", "D", "A" };

            Assert.AreEqual(expect.Length, result.GetValueOrThrow().Length);
            Assert.AreEqual(expect, result.GetValueOrThrow());
        }

        [Test]
        public void ReadAllLines_Doc()
        {
            var simpleReader = new SimpleReader(System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Words.doc"));
            Console.WriteLine(TestContext.CurrentContext.TestDirectory);

            var result = simpleReader.ReadAllLines();

            var expect = new[] { "A", "A", "A", "A", "D", "D", "B", "B", "D", "A", "D" };

            Assert.AreEqual(expect.Length, result.GetValueOrThrow().Length);
            Assert.AreEqual(expect, result.GetValueOrThrow());
        }

        [Test]
        public void ReadAllLines_txt()
        {
            var simpleReader = new SimpleReader(System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Words.txt"));

            var result = simpleReader.ReadAllLines();

            var expect = new[] { "A", "A", "A", "A", "D", "D", "B", "B", "D", "A"};

            Assert.AreEqual(expect.Length, result.GetValueOrThrow().Length);
            Assert.AreEqual(expect, result.GetValueOrThrow());
        }
    }
}
