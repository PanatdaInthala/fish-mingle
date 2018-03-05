using Microsoft.VisualStudio.TestTools.UnitTesting;
using FishMingle.Models;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FishMingle.Tests
{
  [TestClass]
  public class UserTest: IDisposable
  {
    public UserTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=fish_mingle_test;";
    }
    public void Dispose()
    {
      User.DeleteAll();
    }
    [TestMethod]
    public void Equals_TrueForSameUser_User()
    {
      //Arrange, Act
      User firstUser = new User("Jim", "Human", "Jim45", "Password1");
      User secondUser = new User("Jim", "Human", "Jim45", "Password1");

      //Assert
      Assert.AreEqual(firstUser, secondUser);
    }
    [TestMethod]
    public void Save_UserSavesToDatabase_UserList()
    {
      //Arrange
      User testUser = new User("Jim", "Human", "Jim45", "Password1");
      testUser.Save();

      //Act
      List<User> result = User.GetAll();
      List<User> testList = new List<User>{testUser};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      User testUser = new User("Jim", "Human", "Jim45", "Password1");
      testUser.Save();

      //Act
      User savedUser = User.GetAll()[0];

      int result = savedUser.GetId();
      int testId = testUser.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindUserInDatabase_true()
    {
        //Arrange
        User testUser = new User("Jim", "Human", "Jim45", "Password1");
        testUser.SaveUser();
        //Act
        User foundUser = User.Find(testUser.GetId());
        //Assert
        Assert.AreEqual(testUser, foundUser);

    }
  }
}
