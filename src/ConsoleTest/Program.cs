using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yizuan.Service.Api;
using Yizuan.Service.Api.WebApi;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var caller = new WebApiCaller
            {
                Host = "http://10.5.202.234/"
            };
            caller.Get("age.html");
        }
        //private static ApiValueResult<DeviceArgument> test = new ApiValueResult<DeviceArgument>
        //{
        //    Result = true,
        //    ResultData = new DeviceArgument
        //    {
        //        DeviceId = "abc",
        //        DeviceId2 = new[] { "a", "b" },
        //        DeviceId3 = new byte[] { (byte)1, 2, 3 },

        //    }
        //};
        /*static void Main(string[] args)
        {
            string result = "{}";//JsonConvert.SerializeObject(test);
            StringBuilder code = new StringBuilder();
            using (var textReader = new StringReader(result))
            {
                var reader = new JsonTextReader(textReader);
                bool isResultData = false;
                int levle = 0;
                while (reader.Read())
                {
                    if (!isResultData && reader.TokenType == JsonToken.PropertyName)
                    {
                        var name = reader.Value.ToString();
                        if (name == "ResultData")
                        {
                            isResultData = true;
                        }
                        continue;
                    }
                    if (!isResultData)
                        continue;
                    switch (reader.TokenType)
                    {
                        case JsonToken.StartArray:
                            code.Append('[');
                            continue;
                        case JsonToken.StartObject:
                            code.Append('{');
                            levle++;
                            continue;
                        case JsonToken.PropertyName:
                            code.Append($"\"{reader.Value}\"=");
                            continue;
                        case JsonToken.Bytes:
                            code.Append($"\"{reader.Value}\"");
                            break;
                        case JsonToken.Date:
                        case JsonToken.String:
                            code.Append($"\"{reader.Value}\"");
                            break;
                        case JsonToken.Integer:
                        case JsonToken.Float:
                        case JsonToken.Boolean:
                            code.Append("null");
                            break;
                        case JsonToken.Null:
                            code.Append("null");
                            break;
                        case JsonToken.EndObject:
                            if (code.Length > 0 && code[code.Length - 1] == ',')
                                code[code.Length - 1] = '}';
                            else
                                code.Append('}');
                            levle--;
                            break;
                        case JsonToken.EndArray:
                            if (code.Length > 0 && code[code.Length - 1] == ',')
                                code[code.Length - 1] = ']';
                            else
                                code.Append(']');
                            break;
                        case JsonToken.Raw:
                            code.Append(reader.Value);
                            break;
                        case JsonToken.Undefined:
                            break;
                        case JsonToken.StartConstructor:
                            break;
                        case JsonToken.None:
                            break;
                        case JsonToken.EndConstructor:
                            break;
                        case JsonToken.Comment:
                            break;
                    }
                    if (levle == 0)
                        break;
                    code.Append(',');
                }

            }
            if (code.Length > 0 && code[code.Length - 1] == ',')
                code[code.Length - 1] = ' ';
            Console.WriteLine(code.ToString());
            Console.ReadKey();
        }*/
    }
}
