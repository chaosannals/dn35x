using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Threading;
using System.Data.SQLite;
using Dn35x.Base;
using Dn35x.Database;
using Dn35x.Website;
using Dn35x.Demo.Server.Models;
using System.Diagnostics;
using System.Reflection;

namespace Dn35x.Demo.Server
{
    class TestA
    {
        public string @operator { get; set; }
        public string @class { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            foreach(var r in Assembly.GetAssembly(typeof(Program)).GetManifestResourceNames())
            {
                Console.WriteLine(r);
            }
            Console.ReadKey();
            /*
            string dbpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dn35x.Demo.Server.db");
            if (!File.Exists(dbpath))
            {
                File.WriteAllBytes(dbpath, Properties.Resources.Dn35x_Demo_Server);
            }
            using (SqliteSession session = new SqliteSession(dbpath))
            {
                if (session.Count("SELECT COUNT(*) FROM d35_account") == 0)
                {
                    session.Add(new AccountModel {
                        Account = "admin",
                        Password = "123456",
                        CreatedAt = DateTime.Now,
                    }, "d35_account");
                }
                var am = session.Find<AccountModel>("SELECT * FROM d35_account WHERE account=@account", new SQLiteParameter {
                    ParameterName = "account",
                    Value = "admin",
                });
                Console.WriteLine(am.Password.Value);
            }

            foreach(var p in typeof(TestA).GetProperties())
            {
                Console.WriteLine(p.Name);
            }
            Console.ReadLine();
            */

            /*
            // 创建客户端
            for (int i = 0; i < 5; ++i)
            {
                Visiter.Luanch(i);
            }

            // 复制密钥
            string pfxpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dn35x.pfx");
            if (!File.Exists(pfxpath))
            {
                File.WriteAllBytes(pfxpath, Properties.Resources.dn35x);
            }

            // 创建服务端
            
            ApiServer server = new ApiServer(12345, pfxpath);
            server.UseDispatcher<Dispatcher>("/api")
                .UseStaticServer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "www"))
                .Start();
            while(true)
            {
                Thread.Sleep(1000);
            }
            */
        }
    }
}
