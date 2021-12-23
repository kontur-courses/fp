using FluentAssertions;
using NUnit.Framework;
using System.IO;
using ConsoleRunner;

namespace TagCloud2Tests
{
    public class BadSituationsTests
    {
        [Test]
        public void NoInputTxtFile_ShouldBeError()
        {
            Program.Main(new[] { "-p", "nosuchfile" });
            Program.outResult.Error.Should().Be("No such file to open");
        }

        [Test]
        public void IncorrectInputFormat_ShouldBeError()
        {
            File.Create("input1.txt");
            Program.Main(new[] {"-p", "input1.txt", "-f", "abobus" });
            Program.outResult.Error.Should().Be("no such mode with InputFormat");
        }

        [Test]
        public void IncorrectArgumentKeys_ShouldBeError()
        {
            File.Create("input2.txt");
            Program.Main(new[] { "-p", "input2.txt", "--sdd", "asadas" });
            Program.outResult.Error.Should().Be("arguments are incorrect");
        }

        [Test]
        public void IncorrectFontName_ShouldBeError()
        {
            File.Create("input3.txt");
            Program.Main("-p input3.txt --font NotExistingFont".Split());
            Program.outResult.Error.Should().Be("No such font!");
        }

        [Test]
        public void CloudIsTooBig_ShouldBeError()
        {
            File.WriteAllText("input4.txt", "verybigwordthatmustnotfitinthescreenatleastihopeso,thismustnotbemachinedependentihopelmaowhatimdoingnowitwillprobablyfitinbutiwillcontinuetowriteallthisweirdsymbolsthroughtihavenoideawhythishavenospaceslmao");
            Program.Main("-p input4.txt".Split());
            Program.outResult.Error.Should().Be("Cloud is bigger than image");
        }

        [Test]
        public void FontSizeIsNegative_ShouldBeError()
        {
            File.Create("input5.txt");
            Program.Main("-p input5.txt --fontsize -1".Split());
            Program.outResult.Error.Should().Be("FontSize is zero or negative");
        }

        [Test]
        public void ReadingTxtAsDocx_ShouldBeError()
        {
            File.WriteAllText("input6.txt", "this is a simple txt file 123:aboba lmao /r/n\n\t nothing to see here");
            Program.Main("-p input6.txt -f docx".Split());
            Program.outResult.Error.Should().Be("Something is wrong with docx file");
        }
    }
}
