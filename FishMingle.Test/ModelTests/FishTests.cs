using Microsoft.VisualStudio.TestTools.UnitTesting;
using FishMingle;
using System.Collections.Generic;
using FishMingle.Models;
using System;
using MySql.Data.MySqlClient;

namespace FishMingle.Tests
{
    [TestClass]
    public class FishTests : IDisposable
    {
        public FishTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=fish_mingle_test;";
        }
        public void Dispose()
        {
            Fish.DeleteAll();
            Interest.DeleteAll();
            Login.DeleteAll();
        }

        [TestMethod]
        public void GetAllFish_DataBaseAtFirst_0()
        {
            //Arrange, Act
            int result = Fish.GetAllFish().Count;
            Console.WriteLine("This is the number of Fish in the result list: " + result);

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void SaveFish_AssignsIdToObject_Id()
        {
            //Arrange
            Fish testFish = new Fish("Nemo", "Clownfish", 0 );

            //Act
            testFish.SaveFish();
            Fish savedFish = Fish.GetAllFish()[0];
            int result = savedFish.GetFishId();
            int testId = testFish.GetFishId();
            Console.WriteLine(result);
            Console.WriteLine(testId);

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindFishInDatabase_true()
        {
            //Arrange
            Fish testFish = new Fish("Nemo", "Clownfish", 0);
            testFish.SaveFish();
            //Act
            Fish foundFish = Fish.Find(testFish.GetFishId());
            //Assert
            Assert.AreEqual(testFish, foundFish);

        }
    
    }   
    
}


