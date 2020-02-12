using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Tests.SQL
{
    public class SQLCQParameter
    {
        public string Name { get; set; }
        public object value { get; set; }
    }

    public class SQLCQ
    {
        private string ConnectionString { get; set; }

        public SQLCQ(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<T> ExecuteQuery<T>(string query, SQLCQParameter[] param = null)
        {
            List<T> tl = Activator.CreateInstance<List<T>>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                if (param != null && param.Length != 0)
                    for (int i = 0; i < param.Length; i++)
                        command.Parameters.AddWithValue(param[i].Name, param[i].value);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int totalColumns = reader.FieldCount;
                        T t = Activator.CreateInstance<T>();
                        for (int i = 0; i < totalColumns; i++)
                            t.GetType()
                                .GetProperty(reader.GetName(i))?
                                .SetValue(t, reader.GetValue(i));
                        tl.Add(t);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return tl;
        }
    }
}
