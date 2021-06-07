using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Dn35x.Website
{
    public class ApiRequest
    {
        public JObject All { get; private set; }
        public string Id { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }

        public ApiRequest(string data)
        {
            All = JObject.Parse(data);
            Id = All["id"].ToString();
            Controller = All["controller"].ToString();
            Action = All["action"].ToString();
        }

        public T AsData<T>(string name="data")
        {
            return All[name].ToObject<T>();
        }
    }
}
