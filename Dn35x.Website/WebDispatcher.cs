﻿using System;
using System.Reflection;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json.Linq;
using Dn35x.Website.Exceptions;

namespace Dn35x.Website
{
    public abstract class WebDispatcher : WebSocketBehavior
    {
        public Assembly Space { get; private set; }
        public string Prefix { get; private set; }

        public WebDispatcher(Assembly space, string prefix="")
        {
            Space = space;
            Prefix = prefix;
        }

        protected override void OnMessage(MessageEventArgs args)
        {
            JObject request = JObject.Parse(args.Data);
            string controller = request["controller"].ToString();
            string action = request["action"].ToString();
            string type = string.Format("{0}.{1}", Prefix, controller);
            object instance = Space.CreateInstance(type);
            if (instance == null)
            {
                throw new WebQueryException();
            }
            MethodInfo method = instance.GetType().GetMethod(action);
            if (method == null)
            {
                throw new WebQueryException();
            }
            object result = method.Invoke(instance, new object[] { request });
        }
    }
}
