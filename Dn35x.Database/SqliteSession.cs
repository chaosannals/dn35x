using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Data;
using System.Data.SQLite;
using Dn35x.Base;
using Dn35x.Database.Exceptions;

namespace Dn35x.Database
{
    public class SqliteSession :IDisposable
    {
        public string FilePath { get; private set; }
        public string TablePrefix { get; private set; }
        public SQLiteConnection Connection { get; private set; }

        public SqliteSession(string path, string prefix="")
        {
            FilePath = path;
            TablePrefix = prefix;
        }

        public void EnsureConnection()
        {
            if (Connection == null)
            {
                Connection = new SQLiteConnection(string.Format("Data Source={0}", FilePath));
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

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public List<T> Search<T>(string sql, params SQLiteParameter[] args)
        {
            SQLiteCommand command = NewCommand(sql, args);
            SQLiteDataReader reader = command.ExecuteReader();
            List<T> result = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            while (reader.Read())
            {
                T one = (T)type.Assembly.CreateInstance(type.FullName);
                foreach (PropertyInfo p in properties)
                {
                    object v = Definition.Cast(reader[p.Name], p.PropertyType);
                    p.SetValue(one, v is DBNull ? null : v, null);
                }
                result.Add(one);
            }
            reader.Close();
            return result;
        }

        public T Find<T>(string sql, params SQLiteParameter[] args) where T : class
        {
            SQLiteCommand command = NewCommand(sql, args);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            Type type = typeof(T);
            T result = type.Assembly.CreateInstance(type.FullName) as T;
            for (int i = 0; i < reader.FieldCount; ++i)
            {
                string pn = reader.GetName(i);
                PropertyInfo p = type.GetProperty(pn.ToPascal());
                if (p != null)
                {
                    object v = Definition.Cast(reader[pn], p.PropertyType);
                    p.SetValue(result, v, null);
                }
            }
            reader.Close();
            return result;
        }

        public bool Has(string sql, params SQLiteParameter[] args)
        {
            SQLiteCommand command = NewCommand(sql, args);
            SQLiteDataReader reader = command.ExecuteReader();
            bool result = reader.HasRows;
            reader.Close();
            return result;
        }

        public long Count(string sql, params SQLiteParameter[] args)
        {
            SQLiteCommand command = NewCommand(sql, args);
            return long.Parse(command.ExecuteScalar().ToString());
        }

        /// <summary>
        /// 添加一条记录。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="one"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public int Add<T>(T one, string table = null)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO ");
            builder.Append(table ??(TablePrefix + type.Name.Substring(0, -5).ToSnake()));
            builder.Append("(");
            List<string> fields = new List<string>();
            List<string> vhs = new List<string>();
            List<SQLiteParameter> vs = new List<SQLiteParameter>();
            foreach (PropertyInfo p in properties)
            {
                IDefined v = p.GetValue(one, null) as IDefined;
                if (v == null || !v.IsDefined) continue;
                fields.Add(string.Format("[{0}]", p.Name.ToSnake()));
                string h = string.Format("@{0}", p.Name);
                vhs.Add(h);
                vs.Add(new SQLiteParameter(h, v.Value ?? DBNull.Value));
            }
            builder.Append(string.Join(",", fields.ToArray()));
            builder.Append(")VALUES(");
            builder.Append(string.Join(",", vhs.ToArray()));
            builder.Append(")");
            // builder.ToString().Log();
            SQLiteCommand command = NewCommand(builder.ToString(), vs.ToArray());
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 修改。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="one"></param>
        /// <param name="tag"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public int Edit<T>(T one, string tag = "ID", string table = null)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE ");
            builder.Append(table ?? (TablePrefix + type.Name.Substring(0, -5).ToSnake()));
            builder.Append(" SET ");
            List<string> sets = new List<string>();
            List<SQLiteParameter> vs = new List<SQLiteParameter>();
            foreach (PropertyInfo p in properties)
            {
                if (p.Name == tag) continue;
                IDefined field = p.GetValue(one, null) as IDefined;
                if (!field.IsDefined) continue;
                string h = string.Format("@{0}", p.Name);
                vs.Add(new SQLiteParameter(h, field.Value ?? DBNull.Value));
                sets.Add(string.Format("[{0}]=@{1}", p.Name.ToSnake(), p.Name));
            }
            builder.Append(string.Join(",", sets.ToArray()));

            PropertyInfo tagProperty = type.GetProperty(tag);
            if (tagProperty == null)
            {
                throw new InvalidSqlException(string.Format("字段 {0} 无效", tag));
            }

            IDefined tagValue = tagProperty.GetValue(one, null) as IDefined;
            if (tagValue == null)
            {
                throw new InvalidSqlException(string.Format("字段 {0} 数据不可空",tag));
            }

            builder.Append(string.Format(" WHERE [{0}]={1}", tag, tagValue.Value));
            // builder.ToString().Log();
            SQLiteCommand command = NewCommand(builder.ToString(), vs.ToArray());
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 生成一个新命令。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public SQLiteCommand NewCommand(string sql, params SQLiteParameter[] args)
        {
            EnsureConnection();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            command.Parameters.AddRange(args);
            return command;
        }
    }
}
