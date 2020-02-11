using System;
using Commons;

namespace Ling.Extension.Test
{
    internal static class LoggerInitializer
    {
        public static void Init()
        {
            Logger.SetLogHandler(
                new LogHandler(
                    Console.WriteLine,
                    Console.WriteLine,
                    Console.WriteLine,
                    Console.WriteLine,
                    ex =>
                    {
                        if (ex == null) throw new ArgumentNullException(nameof(ex));
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                ));
        }
    }
}