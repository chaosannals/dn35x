using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json.Linq;

namespace Dn35x.Demo.Server
{
    /// <summary>
    /// 接口调度器。
    /// </summary>
    public class Dispatcher : WebSocketBehavior
    {
        public Dispatcher()
        {
        }

        protected override void OnOpen()
        {
            
        }

        /// <summary>
        /// 消息接收。
        /// </summary>
        /// <param name="args"></param>
        protected override void OnMessage(MessageEventArgs arg)
        {
            Send(arg.Data);
        }

        protected override void OnClose(CloseEventArgs args)
        {
            
        }
    }
}
