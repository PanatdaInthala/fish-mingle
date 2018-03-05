using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Models
{
    public class Interest
    {
        // private List<Species> selectedSpecies = new List <Species> {};
        // private int _id;

        // public Interest(string selectedSpecies, int id = 0)
        // {
        //   _id = id;
        // }
        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM interest;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            };
        }
    }
}
