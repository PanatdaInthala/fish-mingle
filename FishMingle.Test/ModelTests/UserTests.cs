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
      User firstUser = new User("Jim", 1, "Jim45", "Password1");
      User secondUser = new User("Jim", 1, "Jim45", "Password1");

      //Assert
      Assert.AreEqual(firstUser, secondUser);
    }
    [TestMethod]
    public void Save_UserSavesToDatabase_UserList()
    {
      //Arrange
      User testUser = new User("Jim", 1, "Jim45", "Password1");
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
      User testUser = new User("Jim", 1, "Jim45", "Password1");
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
        User testUser = new User("Jim", 1, "Jim45", "Password1");
        testUser.Save();
        //Act
        User foundUser = User.Find(testUser.GetId());
        //Assert
        Assert.AreEqual(testUser, foundUser);
    }
    [TestMethod]
    public void Login_TrueForSamePassword_User()
    {
      //Arrange, Act
      User user = new User("Jim", 1, "Jim45", "Password1");
      user.Save();
      User databaseInfo = User.Find(user.GetId()) ;

      //Assert
      Assert.AreEqual(user, databaseInfo);
    }
    [TestMethod]
    public void Delete_DeletesUserInDatabase_Void()
    {
      //Arrange
      User firstUser = new User("Jim", 1, "Jim45", "Password1");
      firstUser.Save();
      User secondUser = new User ("James", 1, "James45", "Password2");
      secondUser.Save();
      //Act
      firstUser.Delete();
      List<User> expected = new List<User> {secondUser};
      List<User> result = User.GetAll();

      //Assert
      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void Logout_LogoutsUser_Void()
    {
      //Arrange
      User firstUser = new User("Jim", 1, "Jim45", "Password1");
      firstUser.Save();
      User secondUser = new User ("James", 1, "James45", "Password2");
      secondUser.Save();

      //Act
      User.Logout(firstUser.GetId());
      List<User> expected = new List<User> {firstUser, secondUser};
      List<User> result = User.GetAll();

      //Assert
      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void UpdateUser_UpdatesUserInDatabase_String()
    {
      //Arrange
      User testUser = new User("Jim", 1, "Jim45", "Password1");
      testUser.Save();

      string updatedUser = "Nick";
      //Act
      testUser.UpdateUser(updatedUser);

      string result = User.Find(testUser.GetId()).GetName();

      //Assert
      Assert.AreEqual(updatedUser, result);
    }
    [TestMethod]
    public void UpdatePassword_UpdatesPasswordInDatabase_String()
    {
      //Arrange
      User testUser = new User("Jim", 1, "Jim45", "Password1");
      testUser.Save();

      string updatedPassword = "Password2";
      //Act
      testUser.UpdatePassword(updatedPassword);

      string result = User.Find(testUser.GetId()).GetPassword();

      //Assert
      Assert.AreEqual(updatedPassword, result);
    }
    [TestMethod]
    public void Test_AddPreference_AddsUserToPreferences()
    {
      //Arrange
      User testUser = new User("Jim", 1, "Jim45", "Password1");
      testUser.Save();

      User testUser1 = new User("Tom", 1, "Tom45", "Password1");
      testUser1.Save();

      User testUser2 = new User("Sam", 1, "Sam45", "Password1");
      testUser2.Save();

      //Act
      testUser.AddPreference(testUser1);
      testUser.AddPreference(testUser2);

      List<User> result = testUser.GetPreferences();
      List<User> testList = new List<User>{testUser1, testUser2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void GetPreferences_ReturnsAllUserPreferences_UserList()
    {
      //Arrange
      User testUser = new User("Jim", 1, "Jim45", "Password1");
      testUser.Save();

      User testUser1 = new User("Tom", 1, "Tom45", "Password1");
      testUser1.Save();

      //Act
      testUser.AddPreference(testUser1);
      List<User> savedUsers = testUser.GetPreferences();
      List<User> testList = new List<User> {testUser1};

      //Assert
      CollectionAssert.AreEqual(testList, savedUsers);
    }
    [TestMethod]
    public void GetMatches_ReturnsAllUserMatches_UserList()
    {
      //Arrange
      User testUser = new User("Jim", 1, "Jim45", "Password1");
      testUser.Save();

      User testUser2 = new User("Tom", 1, "Tom45", "Password1");
      testUser2.Save();

      testUser.AddPreference(testUser2);
      testUser2.AddPreference(testUser);

      //Act
      List<User> testUserMatches = testUser.GetMatches();
      List<User> savedUserMatches = new List<User> {testUser2};

      //Assert
      CollectionAssert.AreEqual(testUserMatches, savedUserMatches);
    }
  }
}
