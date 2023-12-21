using MySql.Data.MySqlClient;
using System;
// See https://aka.ms/new-console-template for more information
namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static string _connectionString = string.Empty;
        static void Main(string[] args)
        {
            _connectionString = "server=192.168.1.5;user=root;password=123456;database=db_test;";
            MySqlConnection conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connect Successful!");
                }
                else
                {
                    Console.WriteLine("Connect Failed!");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Connect Failed!");
            }
            finally { conn.Close(); }
            Console.ReadLine();
        }
    }
}
