using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Models
{
  public class User
  {
    private int _id;
    private string _name;
    private int _speciesId;
    private string _userName;
    private string _password;

    public User(string name, int speciesId, string userName, string password, int id = 0)
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
        int speciesId = rdr.GetInt32(2);
        string userName = rdr.GetString(3);
        string password = rdr.GetString(4);

        User newUser = new User(name, speciesId, userName, password, userId);
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

      User foundUser= new User(name, speciesId, userName, password, userId);

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
    public void UpdateUser(string newUserName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE users SET name = @NewUserName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter userName = new MySqlParameter();
      userName.ParameterName = "@newUserName";
      userName.Value = newUserName;
      cmd.Parameters.Add(userName);

      cmd.ExecuteNonQuery();
      _userName = newUserName;

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
    public void AddPreference(User newUser)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO users_users (fish1_id, fish2_id) VALUES (@UserId, @NewFishId);";

      MySqlParameter userId = new MySqlParameter();
      userId.ParameterName = "@UserId";
      userId.Value = _id;
      cmd.Parameters.Add(userId);

      MySqlParameter newFishId = new MySqlParameter();
      newFishId.ParameterName = "@NewFishId";
      newFishId.Value = newUser.GetId();
      cmd.Parameters.Add(newFishId);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<User> GetPreferences()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT B.* FROM users AS A JOIN users_users ON (A.id = users_users.fish1_id) JOIN users AS B ON (users_users.fish2_id = B.id) WHERE A.id = @UserId;";

      MySqlParameter userIdParameter = new MySqlParameter();
      userIdParameter.ParameterName = "@UserId";
      userIdParameter.Value = _id;
      cmd.Parameters.Add(userIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<User> users = new List<User>{};

      while(rdr.Read())
      {
        int userId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int speciesId = rdr.GetInt32(2);
        string userName = rdr.GetString(3);
        string password = rdr.GetString(4);
        User newUser = new User(name, speciesId, userName, password, userId);
        users.Add(newUser);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return users;
    }
  }
}
