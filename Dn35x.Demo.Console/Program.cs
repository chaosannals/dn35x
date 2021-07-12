using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Dn35x.Crypting;

namespace Dn35x.Demo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            WebRequest request = WebRequest.Create("");
            request.Method = "POST";
            request.ContentType = "application/json";
            string text = JsonConvert.SerializeObject(new
            {
                data = "".EncryptAes256(new
                {

                })
            });
            byte[] data = Encoding.UTF8.GetBytes(text);
            request.ContentLength = data.Length;
            using (var writer = request.GetRequestStream())
            {
                writer.Write(data, 0, data.Length);
            }
            WebResponse response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    System.Console.WriteLine(result);
                }
            }
            
            System.Console.ReadLine();
        }
    }
}
