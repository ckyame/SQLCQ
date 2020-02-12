using System;
using System.Data.SqlClient;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.SQL;

namespace Tests
{
    [TestClass]
    public class ConnectionTest
    {
        string connectionString = "Data Source=TEST123\\SQLEXPRESS;Initial Catalog=Northwind;Trusted_Connection=true;";

        [TestMethod]
        public void CanConnectToDatabase()
        {
            ConnectToDatabase();
        }

        [TestMethod]
        public void CanUseSQLCQ()
        {
            SQLCQ sqlcq = new SQLCQ(connectionString);
            List<NorthWindObject> d = sqlcq.ExecuteQuery<NorthWindObject>("SELECT * FROM [Categories]");
            var x = 1;
        }

        [TestMethod]
        public void CanUseParamsWithSQLCQ()
        {
            SQLCQ sqlcq = new SQLCQ(connectionString);         
            List<NorthWindObject> d = sqlcq.ExecuteQuery<NorthWindObject>(
                "SELECT * FROM [Categories] WHERE CategoryID = @catId", new[] {
                    new SQLCQParameter() {
                        Name = "@catId", 
                        value = 2
                    }
                });
            var x = 1;
        }

        private void ConnectToDatabase()
        {

            string queryString = "SELECT ProductID, UnitPrice, ProductName from dbo.products WHERE UnitPrice > @pricePoint ORDER BY UnitPrice DESC;";
            int paramValue = 5;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@pricePoint", paramValue);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        NorthWindObject o = new NorthWindObject();
                        string n = o.GetType().GetFields()[0].Name;
                        Console.WriteLine("\t{0}\t{1}\t{2}", reader[0], reader[1], reader[2]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    public class NorthWindObject
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public NorthWindObject()
        {
            CategoryName = string.Empty;
            CategoryID = default(int);
        }
    }
}
