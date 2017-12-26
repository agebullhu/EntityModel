using System;
using System.Net;
using System.Threading.Tasks;

namespace CoreTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.ReadKey();
            for (int i = 0; i < 16; i++)
            {
                Task.Factory.StartNew(test);
            }
            Console.ReadKey();
        }

        private static void test()
        {
            for (int i = 0; i < 1024; i++)
            {
                var req =
                    (HttpWebRequest)WebRequest.Create(
                        @"http://10.5.206.218:5000/GoodLin-OAuth-Api/v1/oauth/getdid?Browser=APP&Os=Android&DeviceId=&v=1513782311187");
                using (var res = req.GetResponse())
                {
                    res.Close();
                }
            }
            Console.WriteLine("ENd");
        }
    }
}
