using System;

namespace TagCloudContainer
{
    public class CliElementAttribute : Attribute
    {
        public string CliName;

        public CliElementAttribute(string cliName)
        {
            CliName = cliName;
        }
    }
}