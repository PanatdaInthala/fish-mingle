using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Models
{
  public class Fish
  {
    private int _id;
    private string _firstName;
    private string _lastName;
    private int _speciesId;
    private string _userName;
    private string _password;
    private string _bio;

    public Fish(string firstName, string lastName, int speciesId, string userName, string password, int id = 0)
    {
      _id = id;
      _firstName = firstName;
      _lastName = lastName;
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

    public string GetFirstName()
    {
      return _firstName;
    }

    public string GetLastName()
    {
      return _lastName;
    }

    public void SetFirstName(string firstName)
    {
      _firstName = firstName;
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

    public string GetBio()
    {
      return _bio;
    }

    public void SetBio(string bio)
    {
      _bio = bio;
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
        string firstName = rdr.GetString(1);
        string lastName = rdr.GetString(2);
        int speciesId = rdr.GetInt32(3);
        string userName = rdr.GetString(4);
        string password = rdr.GetString(5);
        string bio = rdr.GetString(6);

        Fish newFish = new Fish(firstName, lastName, speciesId, userName, password, userId);
        newFish.SetBio(bio);
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
        tempFishName = rdr.GetString(4);
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
        cmd.CommandText = @"INSERT INTO users (first_name, last_name, species_id, user_name, password) VALUES (@firstName, @lastName, @speciesId, @userName, @userPassword);";

        MySqlParameter firstName = new MySqlParameter("@firstName", _firstName);
        MySqlParameter lastName = new MySqlParameter("@lastName", _lastName);
        MySqlParameter speciesId = new MySqlParameter("@speciesId", _speciesId);
        MySqlParameter userName = new MySqlParameter("@userName", _userName);
        MySqlParameter password = new MySqlParameter("@userPassword", _password);

        cmd.Parameters.Add(firstName);
        cmd.Parameters.Add(lastName);
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
    public void AddBio(string newBio)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE users SET bio = @FishBio WHERE id = @SearchId;";

      MySqlParameter bioParameter = new MySqlParameter();
      bioParameter.ParameterName = "@FishBio";
      bioParameter.Value = newBio;
      cmd.Parameters.Add(bioParameter);

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@SearchId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static Fish Find(int sessionId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText =
      @"SELECT * FROM users
         JOIN sessions ON (sessions.user_id) = (users.id)
         WHERE sessions.session_id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = sessionId;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int userId = 0;
      string firstName = "";
      string lastName = "";
      int speciesId = 0;
      string userName = "";
      string password = "";
      string bio = "";

      while (rdr.Read())
      {
        userId = rdr.GetInt32(0);
        firstName = rdr.GetString(1);
        lastName = rdr.GetString(2);
        speciesId = rdr.GetInt32(3);
        userName = rdr.GetString(4);
        password = rdr.GetString(5);
        bio = rdr.GetString(6);
      }

      Fish foundFish= new Fish(firstName, lastName, speciesId, userName, password, userId);
      foundFish.SetBio(bio);
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
      string databaseUserName = "";

      while (rdr.Read())
      {
        id = rdr.GetInt32(0);
        databaseUserName = rdr.GetString(4);
        databasePassword = rdr.GetString(5);
      }

      rdr.Dispose();
      if (password == databasePassword && userName == databaseUserName)
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
      else
      {

      }
      conn.Close();
      if (!(conn == null))
      {
        conn.Dispose();
      }

      return sessionId;
    }

    public void Logout(int userId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM sessions WHERE sessions.user_id = @userId;";

      MySqlParameter thisUserId = new MySqlParameter("@userId", userId);
      cmd.Parameters.Add(thisUserId);

      cmd.ExecuteNonQuery();

      conn.Dispose();
    }
    public void UpdateFish(string newFirstName, string newLastName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE users SET first_name = @NewFirstName, last_name= @NewLastName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter firstName = new MySqlParameter();
      firstName.ParameterName = "@NewFirstName";
      firstName.Value = newFirstName;
      cmd.Parameters.Add(firstName);

      MySqlParameter lastName = new MySqlParameter();
      lastName.ParameterName = "@NewLastName";
      lastName.Value = newLastName;
      cmd.Parameters.Add(lastName);


      cmd.ExecuteNonQuery();
      _firstName = newFirstName;
      _lastName = newLastName;

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
    public Species GetFishSpecies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT species.* FROM users JOIN users_species ON (users.id = users_species.user_id) JOIN species ON (users_species.species_id = species.species_id) WHERE users.species_id = @SpeciesId;";

      MySqlParameter stylistIdParameter = new MySqlParameter();
      stylistIdParameter.ParameterName = "@SpeciesId";
      stylistIdParameter.Value = _speciesId;
      cmd.Parameters.Add(stylistIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      string name = "";
      int id = 0;

      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }
      Species newSpecies = new Species(name, id);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newSpecies;
    }
    //ADD A PREFERRED SPECIES TO CURRENT USER'S LIST
    public void AddSpecies(Species newSpecies)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO users_species (user_id, species_id) VALUES (@userId, @SpeciesId);";

      MySqlParameter user_id = new MySqlParameter();
      user_id.ParameterName = "@UserId";
      user_id.Value = _id;
      cmd.Parameters.Add(user_id);

      MySqlParameter species_id = new MySqlParameter();
      species_id.ParameterName = "@SpeciesId";
      species_id.Value = newSpecies.GetSpeciesId();
      cmd.Parameters.Add(species_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    //GET SPECIES THAT CURRENT FISH USER PREFERS
    public List<Species> GetPreferredSpecies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT species.* FROM users JOIN users_species ON (users.id = users_species.user_id) JOIN species ON (species.species_id = users_species.species_id) WHERE users.id = @FishId";

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
    //ADDS A FISH TO USER'S LIST OF PREFERRED FISH
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
    //GETS ALL FISH THAT PREFER FISH USER'S SPECIES
    public List<Fish> GetFishThatPreferMySpecies()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT users.* FROM users_species JOIN users ON users.id = users_species.user_id WHERE users_species.species_id = @SpeciesId;";

      MySqlParameter userIdParameter = new MySqlParameter();
      userIdParameter.ParameterName = "@SpeciesId";
      userIdParameter.Value = _speciesId;
      cmd.Parameters.Add(userIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Fish> users = new List<Fish>{};

      while(rdr.Read())
      {
        int userId = rdr.GetInt32(0);
        string firstName = rdr.GetString(1);
        string lastName = rdr.GetString(2);
        int speciesId = rdr.GetInt32(3);
        string userName = rdr.GetString(4);
        string password = rdr.GetString(5);
        Fish newFish = new Fish(firstName, lastName, speciesId, userName, password, userId);
        users.Add(newFish);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return users;
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
        string firstName = rdr.GetString(1);
        string lastName = rdr.GetString(2);
        int speciesId = rdr.GetInt32(3);
        string userName = rdr.GetString(4);
        string password = rdr.GetString(5);
        Fish newFish = new Fish(firstName, lastName, speciesId, userName, password, userId);
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
        string firstName = rdr.GetString(1);
        string lastName = rdr.GetString(2);
        int speciesId = rdr.GetInt32(3);
        string userName = rdr.GetString(4);
        string password = rdr.GetString(5);
        string bio = rdr.GetString(6);
        Fish newFish = new Fish(firstName, lastName, speciesId, userName, password, userId);
        newFish.SetBio(bio);
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
