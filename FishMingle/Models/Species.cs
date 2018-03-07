using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Models
{
  public class Species
  {
      private int _speciesId;
      private string _speciesName;

      public Species(string speciesName, int speciesId = 0)
      {
          _speciesName = speciesName;
          _speciesId = speciesId;
      }

      //Getters and Setters
      public int GetSpeciesId()
      {
          return _speciesId;
      }
      public void SetSpeciesId(int speciesId)
      {
          _speciesId = speciesId;
      }
      public string GetSpeciesName()
      {
          return _speciesName;
      }
      public void SetSpeciesName(string speciesName)
      {
          _speciesName = speciesName;
      }
      public static List<Species> GetAll()
      {
        List<Species> allSpecies = new List<Species>{};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM species;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int userId = rdr.GetInt32(0);
          string name = rdr.GetString(1);
          Species newSpecies = new Species(name, userId);
          allSpecies.Add(newSpecies);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return allSpecies;
      }
      public static Species Find(int speciesId)
      {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM species WHERE speciesId = (@searchId);";

      MySqlParameter speciesIdParameter = new MySqlParameter();
      speciesIdParameter.ParameterName = "@searchId";
      speciesIdParameter.Value = speciesId;
      cmd.Parameters.Add(speciesId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int foundId = 0;
      string foundName = "";

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundName = rdr.GetString(1);
      }

      Species foundSpecies = new Species(foundName, foundId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundSpecies;
      }
  }
}
