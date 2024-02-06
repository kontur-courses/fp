namespace ResultOf
{
    public static class ErrorParser
    {
        public static void Critical(string error)
        {
            Console.WriteLine(error);
            Environment.Exit(0);
        }
    }
}
