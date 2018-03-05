using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Models
{
  public class Interest
  {
    private List<Species> selectedSpecies = new List <Species> {};
    private int _id;

    public Interest(string selectedSpecies, int id = 0)
    {
      _id = id;
    }
  }
}
