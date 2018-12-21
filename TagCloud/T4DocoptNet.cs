using System.Collections.Generic;
using DocoptNet;

namespace TagCloud
{

    // Generated class for Main.usage.txt
	public class MainArgs
	{
		public const string USAGE = @"Usage:
  tagcloud <source-path> <stopwords-path> <image-path> [--text-color=COLOR] [--background-color=COLOR] [--font=FONT] [--image-size=SIZE]

Options:
-t=COLOR --text-color=COLOR  Text color for words [default: Black]
-b=COLOR --background-color=COLOR  Background color [default: White]
-f=FONT --font=FONT  Text font [default: Calibri]
-i=SIZE --image-size=SIZE  Image size, coordinates separated by x";
	    private readonly IDictionary<string, ValueObject> _args;
		public MainArgs(ICollection<string> argv, bool help = true,
                                                      object version = null, bool optionsFirst = false, bool exit = false)
		{
			_args = new Docopt().Apply(USAGE, argv, help, version, optionsFirst, exit);
		}

        public IDictionary<string, ValueObject> Args
        {
            get { return _args; }
        }

public string ArgSourcePath  { get { return null == _args["<source-path>"] ? null : _args["<source-path>"].ToString(); } }
		public string ArgStopwordsPath  { get { return null == _args["<stopwords-path>"] ? null : _args["<stopwords-path>"].ToString(); } }
		public string ArgImagePath  { get { return null == _args["<image-path>"] ? null : _args["<image-path>"].ToString(); } }
		public string OptTextColor { get { return null == _args["--text-color"] ? "Black" : _args["--text-color"].ToString(); } }
		public string OptBackgroundColor { get { return null == _args["--background-color"] ? "White" : _args["--background-color"].ToString(); } }
		public string OptFont { get { return null == _args["--font"] ? "Calibri" : _args["--font"].ToString(); } }
		public string OptImageSize { get { return null == _args["--image-size"] ? null : _args["--image-size"].ToString(); } }
	
	}

	
}

