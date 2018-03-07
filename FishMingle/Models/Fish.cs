using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Models
{
  public class Fish
  {
    private int _id;
    private string _name;
    private int _speciesId;
    private string _userName;
    private string _password;

    public Fish(string name, int speciesId, string userName, string password, int id = 0)
    {
      _id = id;
      _name = name;
      _speciesId = speciesId;
      _userName = userName;
      _password = password;
    }

    public int GetId()
    {
      return _id;
    }

    public void SetId(int id)
    {
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public void SetName(string name)
    {
      _name = name;
    }

    public int GetSpeciesId()
    {
      return _speciesId;
    }

    public void SetSpeciesId(int speciesId)
    {
      _speciesId = speciesId;
    }

    public string GetFishName()
    {
      return _userName;
    }

    public void SetFishName(string userName)
    {
      _userName = userName;
    }

    public string GetPassword()
    {
      return _password;
    }

    public void SetPassword(string password)
    {
      _password = password;
    }

    public static List<Fish> GetAll()
    {
      List<Fish> allFishs = new List<Fish>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int userId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int speciesId = rdr.GetInt32(2);
        string userName = rdr.GetString(3);
        string password = rdr.GetString(4);

        Fish newFish = new Fish(name, speciesId, userName, password, userId);
        allFishs.Add(newFish);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFishs;
    }

    public bool Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT user_name FROM users WHERE user_name = @userName;";
      MySqlParameter screenName = new MySqlParameter("@userName", _userName);
      cmd.Parameters.Add(screenName);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      string tempFishName = "";

      while(rdr.Read())
      {
        tempFishName = rdr.GetString(3);
      }

      conn.Close();
      if (!(conn == null))
      {
        conn.Dispose();
      }

      if (tempFishName == "")
      {
        conn = DB.Connection();
        conn.Open();
        cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO users (name, species_id, user_name, password) VALUES (@name, @speciesId, @userName, @userPassword);";

        MySqlParameter name = new MySqlParameter("@name", _name);
        MySqlParameter speciesId = new MySqlParameter("@speciesId", _speciesId);
        MySqlParameter userName = new MySqlParameter("@userName", _userName);
        MySqlParameter password = new MySqlParameter("@userPassword", _password);

        cmd.Parameters.Add(name);
        cmd.Parameters.Add(speciesId);
        cmd.Parameters.Add(userName);
        cmd.Parameters.Add(password);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;

        conn.Close();
        if (!(conn == null))
        {
          conn.Dispose();
        }
        return true;
      }
      return false;
    }

    public static Fish Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int userId = 0;
      string name = "";
      int speciesId = 0;
      string userName = "";
      string password = "";

      while (rdr.Read())
      {
        userId = rdr.GetInt32(0);
        name = rdr.GetString(1);
        speciesId = rdr.GetInt32(2);
        userName = rdr.GetString(3);
        password = rdr.GetString(4);
      }

      Fish foundFish= new Fish(name, speciesId, userName, password, userId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundFish;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM users;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
   {
     MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"DELETE FROM users WHERE id = @thisId";

     MySqlParameter deleteId = new MySqlParameter();
     deleteId.ParameterName = "@thisId";
     deleteId.Value = this.GetId();
     cmd.Parameters.Add(deleteId);

     cmd.ExecuteNonQuery();

     conn.Close();
     if (conn != null)
     {
       conn.Dispose();
     }
   }

    public override bool Equals(System.Object otherFish)
    {
      if (!(otherFish is Fish))
      {
        return false;
      }
      else
      {
        Fish newFish = (Fish) otherFish;
        return this.GetId().Equals(newFish.GetId());
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public static int Login(string userName, string password)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM users WHERE user_name = @userName;";

      MySqlParameter userNameParameter = new MySqlParameter();
      userNameParameter.ParameterName = "@userName";
      userNameParameter.Value = userName;
      cmd.Parameters.Add(userNameParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int id = 0;
      string databasePassword = "";
      int sessionId = 0;

      while (rdr.Read())
      {
        id = rdr.GetInt32(0);
        databasePassword = rdr.GetString(4);
      }

      rdr.Dispose();
      if (password == databasePassword)
      {
        cmd.CommandText = @"INSERT INTO sessions (user_id, session_id) VALUES (@userId, @sessionId);";

        MySqlParameter thisIdParameter = new MySqlParameter();
        thisIdParameter.ParameterName = "@userId";
        thisIdParameter.Value = id;
        cmd.Parameters.Add(thisIdParameter);

        MySqlParameter sessionIdParameter = new MySqlParameter();
        sessionIdParameter.ParameterName = "@sessionId";
        Random newRandom = new Random();
        int randomNumber = newRandom.Next(100000000);
        sessionIdParameter.Value = randomNumber;
        cmd.Parameters.Add(sessionIdParameter);

        cmd.ExecuteNonQuery();
        sessionId = randomNumber;
      }

      conn.Close();
      if (!(conn == null))
      {
        conn.Dispose();
      }

      return sessionId;
    }

    public static void Logout(int sessionId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM sessions WHERE session_id = @sessionId;";

      MySqlParameter thisSessionId = new MySqlParameter("@sessionId", sessionId);
      cmd.Parameters.Add(thisSessionId);

      cmd.ExecuteNonQuery();

      conn.Dispose();
    }
    public void UpdateFish(string newFishName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE users SET name = @NewFishName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter userName = new MySqlParameter();
      userName.ParameterName = "@newFishName";
      userName.Value = newFishName;
      cmd.Parameters.Add(userName);

      cmd.ExecuteNonQuery();
      _userName = newFishName;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void UpdatePassword(string newPassword)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE users SET password = @NewPassword WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter userPassword = new MySqlParameter();
      userPassword.ParameterName = "@newPassword";
      userPassword.Value = newPassword;
      cmd.Parameters.Add(userPassword);

      cmd.ExecuteNonQuery();
      _password = newPassword;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    // GET SPECIES OF CURRENT FISH USER
    public string GetFishSpecies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT species.name FROM users
          WHERE users.species_id = @SpeciesId;";

      MySqlParameter stylistIdParameter = new MySqlParameter();
      stylistIdParameter.ParameterName = "@SpeciesId";
      stylistIdParameter.Value = _speciesId;
      cmd.Parameters.Add(stylistIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      string speciesName = "";

      while(rdr.Read())
      {
        string name = rdr.GetString(0);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return speciesName;
    }
    //GET SPECIES (PLURAL) THAT CURRENT FISH USER PREFERS
    public List<Species> GetPreferredSpecies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT species.* FROM users JOIN users_species ON (users.id = users_species.user_id) JOIN species ON (users_species.species_id = users.id) WHERE users.id = @FishId;";

      MySqlParameter FishIdParameter = new MySqlParameter();
      FishIdParameter.ParameterName = "@FishId";
      FishIdParameter.Value = _id;
      cmd.Parameters.Add(FishIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Species> species = new List<Species>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Species newSpecies = new Species (name, id);
        species.Add(newSpecies);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return species;
    }
    //ADDS A USER TO CURRENT FISH USER'S LIST OF PREFERRED FISH
    public void AddPreference(Fish newFish)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO users_users (fish1_id, fish2_id) VALUES (@FishId, @NewFishId);";

      MySqlParameter userId = new MySqlParameter();
      userId.ParameterName = "@FishId";
      userId.Value = _id;
      cmd.Parameters.Add(userId);

      MySqlParameter newFishId = new MySqlParameter();
      newFishId.ParameterName = "@NewFishId";
      newFishId.Value = newFish.GetId();
      cmd.Parameters.Add(newFishId);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    //GETS CURRENT FISH USER'S LIST OF PREFERRED FISH
    public List<Fish> GetPreferences()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT B.* FROM users AS A JOIN users_users ON (A.id = users_users.fish1_id) JOIN users AS B ON (users_users.fish2_id = B.id) WHERE A.id = @FishId;";

      MySqlParameter userIdParameter = new MySqlParameter();
      userIdParameter.ParameterName = "@FishId";
      userIdParameter.Value = _id;
      cmd.Parameters.Add(userIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Fish> users = new List<Fish>{};

      while(rdr.Read())
      {
        int userId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int speciesId = rdr.GetInt32(2);
        string userName = rdr.GetString(3);
        string password = rdr.GetString(4);
        Fish newFish = new Fish(name, speciesId, userName, password, userId);
        users.Add(newFish);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return users;
    }
    //RETURNS LIST OF FISH USERS THAT CURRENT FISH MATCHES WITH
    public List<Fish> GetMatches()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT B.* FROM users AS A JOIN users_users ON (A.id = users_users.fish1_id) JOIN users AS B ON (users_users.fish2_id = B.id) WHERE A.id = @FishId;";

      MySqlParameter userIdParameter = new MySqlParameter();
      userIdParameter.ParameterName = "@FishId";
      userIdParameter.Value = _id;
      cmd.Parameters.Add(userIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Fish> users = new List<Fish>{};

      while(rdr.Read())
      {
        int userId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int speciesId = rdr.GetInt32(2);
        string userName = rdr.GetString(3);
        string password = rdr.GetString(4);
        Fish newFish = new Fish(name, speciesId, userName, password, userId);
        users.Add(newFish);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      List<Fish> matches = new List<Fish>{};
      foreach (var user in users)
      {
        List<Fish> newList = user.GetPreferences();
        foreach (var prefFish in newList)
        {
          if (prefFish.GetId() == _id)
          {
            matches.Add(user);
          }
        }
      }
      return matches;
    }
  }
}
