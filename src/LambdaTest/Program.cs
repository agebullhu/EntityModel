using System;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Lambda testing");
            await LambdaTester.Test();
            Console.ReadKey();
        }
    }
}
