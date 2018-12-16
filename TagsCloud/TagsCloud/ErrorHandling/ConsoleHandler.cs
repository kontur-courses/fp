using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloud.ErrorHandling
{
    public class ConsoleHandler:IErrorHandler
    {
        public void Handle(string error)
        {
            Console.WriteLine(error);
        }
    }
}
