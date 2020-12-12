using System;
using System.IO;

namespace TagCloud.App.CLI
{
    public class TextBridge : IIOBridge
    {
        private readonly TextWriter writer;
        private readonly TextReader reader;
        private readonly BridgeClearer clear;
        
        public delegate void BridgeClearer();

        public TextBridge(TextWriter writer, TextReader reader, BridgeClearer clear)
        {
            this.writer = writer;
            this.reader = reader;
            this.clear = clear;
        }
        
        public void Write(string line) => writer.Write(line);

        public void WriteLine(string line) => writer.WriteLine(line);

        public string Read() => reader.ReadLine();

        public void Next() => clear();
    }
}