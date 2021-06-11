using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebSocketSharp;
using Newtonsoft.Json;

namespace Dn35x.Demo.Server
{
    public static class Visiter
    {
        public static void Luanch(int id)
        {
            new Thread(() =>
            {
                using (var ws = new WebSocket("wss://127.0.0.1:12345/api"))
                {
                    ws.OnMessage += (sender, e) => Console.WriteLine("server says: " + e.Data);
                    ws.Connect();
                    for (int i = 0; i < 100; ++i)
                    {
                        ws.Send(JsonConvert.SerializeObject(new
                        {
                            client = id,
                            times = i,
                        }));
                        Thread.Sleep(1000);
                    }
                }
            }).Start();
        }
    }
}
