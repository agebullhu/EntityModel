using System;
using System.Threading.Tasks;

namespace EM5_Dapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await Tester.Test();
            
        }
    }
}
