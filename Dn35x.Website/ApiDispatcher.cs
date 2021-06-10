using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json.Linq;
using Dn35x.Website.Exceptions;

namespace Dn35x.Website
{
    public abstract class ApiDispatcher : WebSocketBehavior
    {
        public Assembly Space { get; private set; }
        public string Prefix { get; private set; }

        public ApiDispatcher(Assembly space, string prefix="")
        {
            Space = space;
            Prefix = prefix;
        }

        /// <summary>
        /// 消息接收。
        /// </summary>
        /// <param name="args"></param>
        protected override void OnMessage(MessageEventArgs args)
        {
            ApiRequest request = new ApiRequest(args.Data);
            string type = string.Format("{0}.{1}", Prefix, request.Controller);
            object instance = Space.CreateInstance(type);
            if (instance == null)
            {
                throw new WebQueryException();
            }
            MethodInfo method = instance.GetType().GetMethod(request.Action);
            if (method == null)
            {
                throw new WebQueryException();
            }

            ThreadPool.QueueUserWorkItem((state) => {
                object result = method.Invoke(instance, new object[] { request });
                if (result is IEnumerable<ApiResponse>)
                {
                    foreach (ApiResponse response in result as IEnumerable<ApiResponse>)
                    {
                        Respond(response.ToString());
                    }
                }
                else
                {
                    ApiResponse response = result as ApiResponse;

                }
            }, this);
        }

        protected override void OnClose(CloseEventArgs args)
        {
            base.OnClose(args);
        }

        public void Respond(byte[] data)
        {
            Send(data);
        }

        public void Respond(string text)
        {
            Send(text);
        }
    }
}
