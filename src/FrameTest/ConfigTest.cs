using Agebull.Common.Configuration;
using System;


namespace FrameTest
{
    public class ConfigTest
    {
        public static void TestChange()
        {
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["Redis"]);
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Q);
        }

        public static void TestMulit()
        {
            Console.WriteLine(ConfigurationManager.ConnectionStrings["Redis"]);
            ConfigurationManager.Load(@"E:\Agebull\EntityModel\src\FrameTest\bin\Debug\netcoreapp3.1\test.json");
            Console.WriteLine(ConfigurationManager.ConnectionStrings["Redis"]);
            ConfigurationManager.UpdateAppsettings();
            Console.WriteLine(ConfigurationManager.ConnectionStrings["Redis"]);
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["Redis"]);
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Q);
        }
    }
}
