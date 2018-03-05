using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Models
{
  public class User
  {
    private string _fishName;
    private int _fishId;
    private string _fishSpecies;

    public Fish(string fishName, string fishSpecies, int fishId = 0)
    {
      _fishName = fishName;
      _fishId = fishId;
      _fishSpecies = fishSpecies;
    }

    //GETTERS AND SETTERS

    public void SetFishName(string fishName)
    {
      _fishName = fishName;
    }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM fish;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void SetFishId(int fishId)
    {
      _fishId = fishId;
    }

    public void SetFishSpecies(string fishSpecies)
    {
      _fishSpecies = fishSpecies;
    }

    public string GetFishName()
    {
      return _fishName;
    }

    public int GetFishId()
    {
      return _fishId;
    }

    public string GetFishSpecies()
    {
      return _fishSpecies;
    }

        //METHODS

        public override int GetHashCode()
        {
            return this.GetFishId().GetHashCode();
        }

        //METHODS

        public override bool Equals(System.Object otherFish)
        {
            if (!(otherFish is Fish))
            {
                return false;
            }
            else
            {
                Fish newFish = (Fish)otherFish;
                bool idEquality = (this.GetFishId() == newFish.GetFishId());
                bool fishEquality = (this.GetFishName() == newFish.GetFishName());
                return (idEquality && fishEquality);
            }
        }
        public void SaveFish()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"Insert INTO fish (name, species, id) VALUES (@fishName, @fishSpecies , @fishId);";
  
            MySqlParameter fishName = new MySqlParameter();
            fishName.ParameterName = "@fishName";
            fishName.Value = this._fishName;
            cmd.Parameters.Add(fishName);

            MySqlParameter fishSpecies = new MySqlParameter();
            fishSpecies.ParameterName = "@fishSpecies";
            fishSpecies.Value = this._fishSpecies;
            cmd.Parameters.Add(fishSpecies);

            MySqlParameter fishId = new MySqlParameter();
            fishId.ParameterName = "@fishId";
            fishId.Value = this._fishId;
            cmd.Parameters.Add(fishId);

            
            cmd.ExecuteNonQuery();
            _fishId = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        
        }

        public static Fish Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM fish WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int fishId = 0;
            string fishName = "";
            string fishSpecies = ""; 

            while (rdr.Read())
            {
                fishId = rdr.GetInt32(0);
                fishName = rdr.GetString(1);
                fishSpecies = rdr.GetString(2);
            }

            Fish foundFish= new Fish(fishName, fishSpecies, fishId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundFish;
        }

        public static List<Fish> GetAllFish()
        {
            List<Fish> allFish = new List<Fish> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM fish ORDER BY name ASC;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int fishId = rdr.GetInt32(0);
                string fishName = rdr.GetString(1);
                string fishSpecies= rdr.GetString(2);

                Fish newFish = new Fish(fishName, fishSpecies, fishId);
                allFish.Add(newFish);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allFish;
          
        } 

       
 
 
 
  }
}

