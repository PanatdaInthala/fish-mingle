using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Models
{
  public class Fish
  {
    private string _name;
    private int _id;
    private string _species;

    public Fish(string name, string species, int id = 0)
    {
      _name = name;
      _id = id;
      _species = species;
    }
  }
}  
