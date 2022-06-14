using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace JSON_DB_APP
{




    class Program
    {
        public static string fileName = "all_heroes_sets.json";
        public static string jsonString = File.ReadAllText(fileName);
        
        


        public static Dictionary<string, string> new_result;
        public static Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
        public static string a;
        public static string chor = "\"";
        public static string connectionString = ConfigurationManager.ConnectionStrings["JSON_DB"].ConnectionString;
        public static SqlConnection sqlConnection = null;
        
        static void Main(string[] args)
        {
        
            Console.WriteLine("JSON DB");
            foreach (var kvp in result)
            {
                a = kvp.Value.ToString();
                if (a.Length > 127)
                {
                    string aa = kvp.Value.ToString().Substring(0, 127);
                    string ab = aa.Replace('\"', ' ');
                    string ab1 = ab.Replace('\'', '_');
                    string ab2 = ab1.Replace('{', '\"');
                    string ab3 = ab2.Replace('}', '\"');
                   

                    result[kvp.Key] = ab3;
                }
                else 
                {
                    string aa = a;
                    string ab = aa.Replace('\"', ' ');
                    string ab1 = ab.Replace('`', '_');
                    string ab2 = ab1.Replace('{', ' ');
                    string ab3 = ab2.Replace('}', ' ');
                   

                    result[kvp.Key] = ab3;
                }
               
                
            }
            
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



                    if (command.ToLower().Equals("json")) //CONSTRUCTOR
                    {

                        foreach (KeyValuePair<string, object> kvp in result)

                        {


                            int leng = kvp.Value.ToString().Length;
                            if (leng > 127)
                            {
                                string cos = kvp.Value.ToString();
                                
                                string query = $"INSERT INTO hero_set_ (hero,sets) VALUES ({kvp.Key.ToString()},{cos})";
                                SqlCommand command_json = new SqlCommand(query, sqlConnection);
                                Console.WriteLine($"Added {command_json.ExecuteNonQuery()} row(s)");
                            }
                            else
                            {
                                string query1 = $"INSERT INTO hero_set_ (hero,sets) VALUES ('{kvp.Key.ToString()}','{kvp.Value.ToString().Substring(0, leng) }')";
                                SqlCommand command_json = new SqlCommand(query1, sqlConnection);
                                Console.WriteLine($"Added {command_json.ExecuteNonQuery()} row(s)");
                            }



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
                                Console.WriteLine($"{sqlDataReader["Id"]} + {sqlDataReader["hero"]} + {sqlDataReader["sets"]}");
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
