using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common;

namespace CoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ContextHelper.LogicalSetData("test", "abc");
            Task.Factory.StartNew(test);
            var data = ContextHelper.LogicalGetData<string>("test");
            Console.Write(data);
            Console.ReadKey();
        }

        private static void test()
        {
            var data = ContextHelper.LogicalGetData<string>("test");
            Console.Write(data);

        }
    }
}
