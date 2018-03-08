using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FishMingle.Models;

namespace FishMingle.Controllers
{
  public class FishController : Controller
  {

    [HttpGet("/fish/{sessionId}/browse")]
    public ActionResult FishBrowse(int sessionId)
    {
      Dictionary<string, object> profileData = new Dictionary<string, object>();
      List<Fish> fishList = Fish.GetAll();
      profileData.Add("fishList", fishList);
      Fish newFish = Fish.Find(sessionId);
      Console.WriteLine("Name: " + newFish.GetName());
      profileData.Add("newFish", newFish);
      profileData.Add("sessionId", sessionId);

      return View(profileData);
    }

    [HttpGet("/fish/{sessionId}")]
    public ActionResult ViewProfile(int sessionId)
    {
      Dictionary<string, object> profileData = new Dictionary<string, object>();
      List<Fish> fishList = Fish.GetAll();
      profileData.Add("fishList", fishList);
      Fish newFish = Fish.Find(sessionId);
      Console.WriteLine("Name: " + newFish.GetName());
      profileData.Add("newFish", newFish);
      profileData.Add("sessionId", sessionId);
      return View(profileData);
    }

    //FORM FOR CREATING USER
    [HttpGet("/fish/create")]
    public ActionResult CreateFishForm()
    {
      List<Species> speciesList = Species.GetAllSpecies();
      return View("NewFishForm", speciesList);
    }

    [HttpPost("/fish/create")]
    public ActionResult CreateFish()
    {
      string userNameActual = Request.Form["userName"];
      int speciesId = Int32.Parse(Request.Form["speciesId"]);
      string userNameProfile = Request.Form["userNameProfile"];
      string userPassword = Request.Form["userPassword"];
      Fish newFish = new Fish( userNameProfile, speciesId, userNameActual, userPassword );
      newFish.Save();
      int newSessionId = Fish.Login(Request.Form["userName"], Request.Form["userPassword"]);

      string selectedSpecies = Request.Form["chosenSpecies"];
      if (selectedSpecies != null)
      {
        String[] speciesIds = selectedSpecies.Split(',');
        foreach(var species in speciesIds)
        {
          int speciesIdInt = int.Parse(species);
          Species newSpecies = Species.Find(speciesIdInt);
          newFish.AddSpecies(newSpecies);
        }
      }
      return RedirectToAction("ViewProfile", new { sessionId = newSessionId});
    }

    [HttpGet("/fish/{sessionId}/update/password")]
    public ActionResult UpdatePassword(int sessionId)
    {
      Dictionary<string, object> profileData = new Dictionary<string, object>();
      Fish newFish = Fish.Find(sessionId);
      profileData.Add("sessionId", sessionId);
      profileData.Add("newFish", newFish);
      return View (profileData);
    }

    [HttpPost("/fish/{sessionId}/update/password")]
    public ActionResult UpdateUserPassword(int sessionId)
    {
      Fish thisFish = Fish.Find(sessionId);
      thisFish.UpdatePassword(Request.Form["updatePassword"]);
      return RedirectToAction ("SuccessChanges", "Home");;
    }

    [HttpGet("/fish/{sessionId}/update/username")]
    public ActionResult UpdateName(int sessionId)
    {
      Dictionary<string, object> profileData = new Dictionary<string, object>();
      Fish newFish = Fish.Find(sessionId);
      profileData.Add("sessionId", sessionId);
      profileData.Add("newFish", newFish);

      return View (profileData);
    }

    [HttpPost("/fish/{sessionId}/update/username")]
    public ActionResult UpdateUsername(int sessionId)
    {
      Fish thisFish = Fish.Find(sessionId);
      thisFish.UpdateFish(Request.Form["updateName"]);
      return RedirectToAction ("SuccessChanges", "Home");
    }

    //CONTROLLER ROUTES FOR VIEWING MATCHES AND WHEN ANOTHER USER LIKES A USER BUT ISN'T MATCHED

    [HttpGet("/fish/{sessionId}/match")]
    public ActionResult Match(int sessionId)
    {
      Dictionary<string, object> profileData = new Dictionary<string, object>();
      List<Fish> fishList = Fish.GetAll();
      profileData.Add("fishList", fishList);
      Fish newFish = Fish.Find(sessionId);
      Console.WriteLine("Name: " + newFish.GetName());
      profileData.Add("newFish", newFish);
      profileData.Add("sessionId", sessionId);

      return View(profileData);
    }

    [HttpGet("/fish/login")]
    public ActionResult FishLoginForm()
    {
      return View ("Login");
    }

    [HttpGet("/fish/login/error/{sessionId}")]
    public ActionResult LoginError()
    {
      return View ("LoginError");
    }

    [HttpPost("/fish/login/error/{sessionId}")]
    public ActionResult UserLoginError()
    {
      int sessionId = Fish.Login(Request.Form["userName"], Request.Form["userPassword"]);

      if(sessionId == 0){

        return RedirectToAction("LoginError");
      }
      return RedirectToAction("ViewProfile", new {sessionId= sessionId});
    }

    [HttpPost("/fish/login")]
    public ActionResult UserLogin()
    {
      int sessionId = Fish.Login(Request.Form["userName"], Request.Form["userPassword"]);

      if(sessionId != 0)
      {
        return RedirectToAction("ViewProfile", new {sessionId= sessionId});
      }
      return RedirectToAction("LoginError", new {sessionId = sessionId});
    }


    [HttpGet("/fish/{sessionId}/logout")]
    public ActionResult UserLogout(int sessionId)
    {
      Fish newFish = Fish.Find(sessionId);
      int newId = newFish.GetId();
      newFish.Logout(newId);
      return RedirectToAction("SuccessLogout", "Home");
    }
    [HttpGet("/fish/{sessionId}/confirmation/delete")]
    public ActionResult DeleteAccount(int sessionId)
    {
      Dictionary<string, object> profileData = new Dictionary<string, object>();
      Fish newFish = Fish.Find(sessionId);
      profileData.Add("sessionId", sessionId);
      profileData.Add("newFish", newFish);

      return View(profileData);
    }
    [HttpGet("/fish/{sessionId}/delete")]
    public ActionResult DeleteAccountConfirm(int sessionId)
    {
      Fish newFish = Fish.Find(sessionId);
      int newId = newFish.GetId();
      newFish.Delete();
      return RedirectToAction("Index", "Home");
    }
  }
}
