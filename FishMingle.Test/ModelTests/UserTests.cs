using Microsoft.VisualStudio.TestTools.UnitTesting;
using FishMingle.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Tests
{
  [TestClass]
  public class FishTest: IDisposable
  {
    public FishTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=fish_mingle_test;";
    }
    public void Dispose()
    {
      Fish.DeleteAll();
    }
    [TestMethod]
    public void Equals_TrueForSameFish_Fish()
    {
      //Arrange, Act
      Fish firstFish = new Fish("Jim", 1, "Jim45", "Password1");
      Fish secondFish = new Fish("Jim", 1, "Jim45", "Password1");

      //Assert
      Assert.AreEqual(firstFish, secondFish);
    }
    [TestMethod]
    public void Save_FishSavesToDatabase_FishList()
    {
      //Arrange
      Fish testFish = new Fish("Jim", 1, "Jim45", "Password1");
      testFish.Save();

      //Act
      List<Fish> result = Fish.GetAll();
      List<Fish> testList = new List<Fish>{testFish};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Fish testFish = new Fish("Jim", 1, "Jim45", "Password1");
      testFish.Save();

      //Act
      Fish savedFish = Fish.GetAll()[0];

      int result = savedFish.GetId();
      int testId = testFish.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindFishInDatabase_true()
    {
        //Arrange
        Fish testFish = new Fish("Jim", 1, "Jim45", "Password1");
        testFish.Save();
        //Act
        Fish foundFish = Fish.Find(testFish.GetId());
        //Assert
        Assert.AreEqual(testFish, foundFish);
    }
    [TestMethod]
    public void Login_TrueForSamePassword_Fish()
    {
      //Arrange, Act
      Fish user = new Fish("Jim", 1, "Jim45", "Password1");
      user.Save();
      Fish databaseInfo = Fish.Find(user.GetId()) ;

      //Assert
      Assert.AreEqual(user, databaseInfo);
    }
    [TestMethod]
    public void Delete_DeletesFishInDatabase_Void()
    {
      //Arrange
      Fish firstFish = new Fish("Jim", 1, "Jim45", "Password1");
      firstFish.Save();
      Fish secondFish = new Fish ("James", 1, "James45", "Password2");
      secondFish.Save();
      //Act
      firstFish.Delete();
      List<Fish> expected = new List<Fish> {secondFish};
      List<Fish> result = Fish.GetAll();

      //Assert
      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void Logout_LogoutsFish_Void()
    {
      //Arrange
      Fish firstFish = new Fish("Jim", 1, "Jim45", "Password1");
      firstFish.Save();
      Fish secondFish = new Fish ("James", 1, "James45", "Password2");
      secondFish.Save();

      //Act
      Fish.Logout(firstFish.GetId());
      List<Fish> expected = new List<Fish> {firstFish, secondFish};
      List<Fish> result = Fish.GetAll();

      //Assert
      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void UpdateFish_UpdatesFishInDatabase_String()
    {
      //Arrange
      Fish testFish = new Fish("Jim", 1, "Jim45", "Password1");
      testFish.Save();

      string updatedFish = "Nick";
      //Act
      testFish.UpdateFish(updatedFish);

      string result = Fish.Find(testFish.GetId()).GetName();

      //Assert
      Assert.AreEqual(updatedFish, result);
    }
    [TestMethod]
    public void UpdatePassword_UpdatesPasswordInDatabase_String()
    {
      //Arrange
      Fish testFish = new Fish("Jim", 1, "Jim45", "Password1");
      testFish.Save();

      string updatedPassword = "Password2";
      //Act
      testFish.UpdatePassword(updatedPassword);

      string result = Fish.Find(testFish.GetId()).GetPassword();

      //Assert
      Assert.AreEqual(updatedPassword, result);
    }
    [TestMethod]
    public void Test_AddSpeciesPreference_AddsSpecies_SpeciesList()
    {
      //Arrange
      Fish testFish = new Fish("Jim", 1, "Jim45", "Password1");
      testFish.Save();

      Species testSpecies1 = new Species("Tom");
      testSpecies1.Save();

      //Act
      testFish.AddSpecies(testSpecies1);
      List<Species> result = testFish.GetPreferredSpecies();
      int testId = result[0].GetSpeciesId();
      int testId2 = testSpecies1.GetSpeciesId();

      //Assert
      Assert.AreEqual(testId, testId2);
    }
    [TestMethod]
    public void Test_AddPreference_AddsFishToPreferences()
    {
      //Arrange
      Fish testFish = new Fish("Jim", 1, "Jim45", "Password1");
      testFish.Save();

      Fish testFish1 = new Fish("Tom", 1, "Tom45", "Password1");
      testFish1.Save();

      Fish testFish2 = new Fish("Sam", 1, "Sam45", "Password1");
      testFish2.Save();

      //Act
      testFish.AddPreference(testFish1);
      testFish.AddPreference(testFish2);

      List<Fish> result = testFish.GetPreferences();
      List<Fish> testList = new List<Fish>{testFish1, testFish2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    //GET ALL FISH THAT PREFER CURRENT FISH USER'S SPECIES
    [TestMethod]
    public void GetFishThatPreferMySpecies_ReturnsAllFishMatches_FishList()
    {
      //Arrange
      Species newSpecies = new Species("Angler");
      newSpecies.Save();
      Species newSpecies2 = new Species("Shark");
      newSpecies2.Save();
      int id = newSpecies.GetSpeciesId();
      int id2 = newSpecies2.GetSpeciesId();

      Fish testFish = new Fish("Jim", id, "Jim45", "Password1");
      testFish.Save();
      Fish testFish2 = new Fish("Tom", id2, "Tom45", "Password1");
      testFish2.Save();

      testFish.AddSpecies(newSpecies2);
      testFish2.AddSpecies(newSpecies);

      //Act
      List<Fish> testFishThatPreferMe = testFish.GetFishThatPreferMySpecies();
      List<Fish> savedFish = new List<Fish> {testFish2};

      //Assert
      CollectionAssert.AreEqual(testFishThatPreferMe, savedFish);
    }
    [TestMethod]
    public void GetPreferences_ReturnsAllFishPreferences_FishList()
    {
      //Arrange
      Fish testFish = new Fish("Jim", 1, "Jim45", "Password1");
      testFish.Save();

      Fish testFish1 = new Fish("Tom", 1, "Tom45", "Password1");
      testFish1.Save();

      //Act
      testFish.AddPreference(testFish1);
      List<Fish> savedFishs = testFish.GetPreferences();
      List<Fish> testList = new List<Fish> {testFish1};

      //Assert
      CollectionAssert.AreEqual(testList, savedFishs);
    }
    [TestMethod]
    public void GetMatches_ReturnsAllFishMatches_FishList()
    {
      //Arrange
      Fish testFish = new Fish("Jim", 1, "Jim45", "Password1");
      testFish.Save();

      Fish testFish2 = new Fish("Tom", 1, "Tom45", "Password1");
      testFish2.Save();

      testFish.AddPreference(testFish2);
      testFish2.AddPreference(testFish);

      //Act
      List<Fish> testFishMatches = testFish.GetMatches();
      List<Fish> savedFishMatches = new List<Fish> {testFish2};

      //Assert
      CollectionAssert.AreEqual(testFishMatches, savedFishMatches);
    }
  }
}
