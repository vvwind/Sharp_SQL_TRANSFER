using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.Json;
namespace JSON_DB_APP
{




    class Program
    {
        private static string fileName = "all_heroes_sets.json";
        private static string jsonString = File.ReadAllText(fileName);

        private static Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);


        private static string connectionString = ConfigurationManager.ConnectionStrings["JSON_DB"].ConnectionString;
        private static SqlConnection sqlConnection = null;
        static void Main(string[] args)
        {
            Console.WriteLine($"Date: {result["abaddon"]}");
            Console.WriteLine("JSON DB");
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
           
            SqlDataReader sqlDataReader = null;

            string command = string.Empty;

            while (true)
            {
                try
                {
                    Console.Write("Write smth \n");
                    command = Console.ReadLine();


                    if (command.ToLower().Equals("exit"))
                    {
                        if (sqlConnection.State == ConnectionState.Open)
                        {
                            sqlConnection.Close();
                        }

                        if (sqlDataReader != null)
                        {
                            sqlDataReader.Close();
                        }

                        break;

                    }

                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

                    switch (command.Split()[0].ToLower())
                    {

                        case "select":

                            sqlDataReader = sqlCommand.ExecuteReader();
                            while (sqlDataReader.Read())
                            {
                                Console.WriteLine($"{sqlDataReader["Id"]} + {sqlDataReader["hero"]} + {sqlDataReader["set_name"]} + {sqlDataReader["set_items"]}");
                                Console.WriteLine(new string('-', 30));

                            }
                            break;

                        case "insert":
                            Console.WriteLine($"Added {sqlCommand.ExecuteNonQuery()} row(s)");
                            break;

                        case "update":
                            Console.WriteLine($"Updated {sqlCommand.ExecuteNonQuery()} row(s)");

                            break;

                        case "delete":

                            Console.WriteLine($"Deleted {sqlCommand.ExecuteNonQuery()} row(s)");
                            break;

                        case "json":

                            break;
                        default:

                            Console.WriteLine($"Command {command} is not correct");

                            break;


                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"Exception: {exc}");
                }

            }

        }

    
    }
  
}
