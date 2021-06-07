using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WebSocketSharp.Server;

namespace Dn35x.Website
{
    public class ApiServer<T> where T: ApiDispatcher, new()
    {
        private static Dictionary<string, string> MIME = new Dictionary<string, string>() {
            { ".html", "text/html" },
            { ".css", "text/css" },
            { ".js", "application/x-javascript" },
            { ".json", "application/json" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".ico", "image/x-icon" },
            { ".ttf", "application/octet-stream" },
            { ".woff", "application/octet-stream" },
            { ".map", "application/json" },
        };

        public HttpServer Server { get; private set; }
        public string Folder { get; private set; }

        public ApiServer(int port=80, string certification=null, string password=null)
        {
            bool secure = certification != null;
            Server = new HttpServer(IPAddress.Any, port, secure);
            Folder = null;
            if (secure)
            {
                Server.SslConfiguration.ServerCertificate = new X509Certificate2(
                    certification,
                    password ?? "",
                    X509KeyStorageFlags.PersistKeySet |
                    X509KeyStorageFlags.MachineKeySet |
                    X509KeyStorageFlags.Exportable
                );
                // 因为 .net framework 3.5 TLS 版本太低。
                // 使用强制转换 新版本 TLS 1.2 的枚举值。
                Server.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls | (SslProtocols)0xC00;
            }
            Server.AddWebSocketService<T>("/api");
            Server.OnGet += new EventHandler<HttpRequestEventArgs>(ServeStatic);
        }

        public void UseStaticServer(string folder)
        {
            if (Folder == null)
            {
                Server.OnGet += new EventHandler<HttpRequestEventArgs>(ServeStatic);
            }
            Folder = Path.GetFileName(folder);
        }

        public void ServeStatic(object sender, HttpRequestEventArgs e)
        {
            string location = e.Request.Url.AbsolutePath.Trim('/').Replace('/', '\\');
            string filePath = Path.Combine(Folder, location);
            string suffix = Path.GetExtension(filePath).ToLower();
            if (!File.Exists(filePath) || !MIME.ContainsKey(suffix))
            {
                filePath = Path.Combine(Folder, "index.html");
                suffix = ".html";
            }
            byte[] contents = File.ReadAllBytes(filePath);
            e.Response.ContentType = MIME[suffix];
            e.Response.ContentLength64 = contents.Length;
            e.Response.StatusCode = 200;
            e.Response.Close(contents, true);
        }
    }
}
