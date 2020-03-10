
namespace FrameTest
{
    class Program
    {
        static void Main()
        {
            LogRecorderTest.TestChange();
            //RedisTest.TestFind();
        }
        //static void Main(string[] args)
        //{

        //    var call = new HttpCaller()
        //    {
        //        Host = "http://zero.hpcang.com",
        //        Authorization = "*",
        //        ApiName = "/gateway/file",
        //        Query = new Dictionary<string, string>
        //        {
        //            {"action" ,"text"},
        //            { "url","/Anuexs/V1QLSGPJK1.bin"}
        //        },
        //        Method = "POST"
        //    };
        //    call.CreateRequest();
        //    var task = call.Call();
        //    task.Wait();
        //    var result = JsonConvert.DeserializeObject<ApiResult<string>>(task.Result);
        //    Console.ReadKey();
        //}
    }
}
