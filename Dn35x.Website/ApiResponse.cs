using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dn35x.Website
{
    public class ApiResponse
    {
        public string Id { get; private set; }
        public bool Keep { get; private set; }
        public ApiResponse(string id, bool keep=false)
        {
            Id = id;
            Keep = keep;
        }

        public virtual string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; private set; }

        public ApiResponse(string id, T data, bool keep=false): base(id, keep)
        {
            Data = data;
        }
    }

    public class ApiRawResponse : ApiResponse
    {
        public string Data { get; private set; }

        public ApiRawResponse(string id, string data, bool keep=false): base(id, keep)
        {
            Data = data;
        }

        public override string ToJSON()
        {
            string data = Data;
            Data = null;
            JObject jo = new JObject(base.ToJSON());
            jo["data"] = JObject.Parse(data);
            return JsonConvert.SerializeObject(jo);
        }
    }
}
