using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using Dn35x.Base;
using Dn35x.Database.Exceptions;

namespace Dn35x.Database
{
    public class SqlServerSession : IDisposable
    {
        public string ConnectionString { get; private set; }
        public string TablePrefix { get; private set; }
        public SqlConnection Connection { get; private set; }
        public SqlTransaction Transaction { get; private set; }
        public bool IsTransacted { get; private set; }

        public SqlServerSession(string server, string database, string user, string password, string prefix="")
        {
            ConnectionString = string.Format(
                "Server={0};" +
                "Database={1};" +
                "User Id={2};" +
                "Password={3};" +
                "Trusted_Connection=false",
                server,
                database,
                user,
                password
            );
            TablePrefix = prefix;
            Connection = null;
            Transaction = null;
        }

        public void Dispose()
        {
            if (Transaction != null && !IsTransacted)
            {
                Transaction.Rollback();
            }
            Connection?.Dispose();
        }

        public void EnsureConnection()
        {
            if (Connection == null)
            {
                Connection = new SqlConnection(ConnectionString);
            }
            switch (Connection.State)
            {
                case ConnectionState.Broken:
                    Connection.Close();
                    Connection.Open();
                    break;
                case ConnectionState.Closed:
                    Connection.Open();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<T> Search<T>(string sql, params SqlParameter[] args)
        {
            List<T> result = new List<T>();
            Type type = typeof(T);
            SqlCommand command = NewCommand(sql, args);
            SqlDataReader reader = command.ExecuteReader();
            PropertyInfo[] properties = type.GetProperties();
            while (reader.Read())
            {
                T one = (T)type.Assembly.CreateInstance(type.FullName);
                foreach (PropertyInfo p in properties)
                {
                    var v = reader[p.Name];
                    p.SetValue(one, v is DBNull ? null : v, null);
                }
                result.Add(one);
            }
            reader.Close();
            return result;
        }

        public bool Has(string sql, params SqlParameter[] args)
        {
            SqlCommand command = NewCommand(sql, args);
            SqlDataReader reader = command.ExecuteReader();
            bool result = reader.HasRows;
            reader.Close();
            return result;
        }

        public long Count(string sql, params SqlParameter[] args)
        {
            SqlCommand command = NewCommand(sql, args);
            return long.Parse(command.ExecuteScalar().ToString());
        }

        public int Add<T>(T one, string table = null)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO [");
            builder.Append(table ?? (TablePrefix + type.Name.Substring(0, -5).ToSnake()));
            builder.Append("](");
            List<string> fields = new List<string>();
            List<string> vhs = new List<string>();
            List<SqlParameter> vs = new List<SqlParameter>();
            foreach (PropertyInfo p in properties)
            {
                IDefined d = p.GetValue(one, null) as IDefined;
                if (d != null && d.IsDefined)
                {
                    string h = string.Format("@{0}", p.Name);
                    fields.Add(string.Format("[{0}]", p.Name));
                    vhs.Add(h);
                    vs.Add(new SqlParameter(h, d.Value ?? DBNull.Value));
                }
            }
            builder.Append(string.Join(",", fields.ToArray()));
            builder.Append(")VALUES(");
            builder.Append(string.Join(",", vhs.ToArray()));
            builder.Append(")");
            // builder.ToString().Log();
            SqlCommand command = NewCommand(builder.ToString(), vs.ToArray());
            return command.ExecuteNonQuery();
        }

        public int Edit<T>(T one, string tag = "ID", string table = null)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE [");
            builder.Append(table ?? (TablePrefix + type.Name.Substring(0, -5).ToSnake()));
            builder.Append("] SET ");
            List<string> sets = new List<string>();
            List<SqlParameter> vs = new List<SqlParameter>();
            foreach (PropertyInfo p in properties)
            {
                if (p.Name == tag) continue;
                IDefined d = p.GetValue(one, null) as IDefined;
                if (d != null && d.IsDefined)
                {
                    string h = string.Format("@{0}", p.Name);
                    vs.Add(new SqlParameter(h, d.Value ?? DBNull.Value));
                    sets.Add(string.Format("[{0}]=@{1}", p.Name, p.Name));
                }
            }
            builder.Append(string.Join(",", sets.ToArray()));

            PropertyInfo tagProperty = type.GetProperty(tag);
            if (tagProperty == null)
            {
                throw new InvalidSqlException(string.Format("字段 {0} 不存在", tag));
            }

            IDefined tagValue = tagProperty.GetValue(one, null) as IDefined;
            if (tagValue == null)
            {
                throw new InvalidSqlException(string.Format("字段 {0} 传入值无效", tag));
            }

            builder.Append(string.Format(" WHERE [{0}]={1}", tag, tagValue.Value));
            // builder.ToString().Log();
            SqlCommand command = NewCommand(builder.ToString(), vs.ToArray());
            return command.ExecuteNonQuery();
        }

        public SqlCommand NewCommand(string sql, params SqlParameter[] args)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            command.Transaction = Transaction;
            command.Parameters.AddRange(args);
            return command;
        }
    }
}
