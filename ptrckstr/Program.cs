namespace ptrckstr
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.TaskAsync().GetAwaiter().GetResult();
        }
    }
}
