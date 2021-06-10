using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Data.SQLite;
using Dn35x.Base;
using Dn35x.Database;
using Dn35x.Website;
using Dn35x.Demo.Server.Models;

namespace Dn35x.Demo.Server
{
    class Program
    {
        static void Main(string[] args)
        {
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
                Console.ReadLine();
            }
        }
    }
}
