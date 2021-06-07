using System;
using System.Reflection;
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
            object result = method.Invoke(instance, new object[] { request });
        }
    }
}
