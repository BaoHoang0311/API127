namespace API127.Logging
{
    public class Logg : ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine("error-" + message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
