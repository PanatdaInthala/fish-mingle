using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Models
{
  public class User
  {
    private int _id;
    private string _name;
    private string _species;
    private string _userName;
    private string _password;

    public User(string name, string species, string userName, string password, int id = 0)
    {
      _id = id;
      _name = name;
      _species = species;
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

    public string GetSpecies()
    {
      return _species;
    }

    public void SetSpecies(string species)
    {
      _species = species;
    }

    public string GetUserName()
    {
      return _userName;
    }

    public void SetUserName(string userName)
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

    public static List<User> GetAll()
    {
      List<User> allUsers = new List<User>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int userId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string species = rdr.GetString(2);
        string userName = rdr.GetString(3);
        string password = rdr.GetString(4);

        User newUser = new User(name, species, userName, password, userId);
        allUsers.Add(newUser);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allUsers;
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

      string tempUserName = "";

      while(rdr.Read())
      {
        tempUserName = rdr.GetString(3);
      }

      conn.Close();
      if (!(conn == null))
      {
        conn.Dispose();
      }

      if (tempUserName == "")
      {
        conn = DB.Connection();
        conn.Open();
        cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO users (name, species, user_name, password) VALUES (@name, @species, @userName, @userPassword);";

        MySqlParameter name = new MySqlParameter("@name", _name);
        MySqlParameter species = new MySqlParameter("@species", _species);
        MySqlParameter userName = new MySqlParameter("@userName", _userName);
        MySqlParameter password = new MySqlParameter("@userPassword", _password);

        cmd.Parameters.Add(name);
        cmd.Parameters.Add(species);
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

    public static User Find(int id)
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
        string species = "";
        string userName = "";
        string password = "";

        while (rdr.Read())
        {
          int userId = rdr.GetInt32(0);
          string name = rdr.GetString(1);
          string species = rdr.GetString(2);
          string userName = rdr.GetString(3);
          string password = rdr.GetString(4);
        }

        User foundUser= new User(name, species, userName, password, userId);

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }

        return foundUser;
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

    public override bool Equals(System.Object otherUser)
    {
      if (!(otherUser is User))
      {
        return false;
      }
      else
      {
        User newUser = (User) otherUser;
        return this.GetId().Equals(newUser.GetId());
      }
    }
    
    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }
  }
}
