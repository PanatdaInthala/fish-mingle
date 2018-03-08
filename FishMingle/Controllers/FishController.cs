using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FishMingle.Models;

namespace FishMingle.Controllers
{
  public class FishController : Controller
  {

    [HttpGet("/fish/browse")]
    public ActionResult FishBrowse()
    {
      List<Fish> fishList = Fish.GetAll();
      return View("FishIndex", fishList);
    }

    [HttpGet("/fish/{sessionId}")]
    public ActionResult ViewProfile(int sessionId)
    {
      Console.WriteLine("SESSION ID: " + sessionId);
      Dictionary<string, object> profileData = new Dictionary<string, object>();
      List<Fish> fishList = Fish.GetAll();
      profileData.Add("fishList", fishList);
      Fish newFish = Fish.Find(sessionId);
      Console.WriteLine("Name: " + newFish.GetName());
      profileData.Add("newFish", newFish);
      List<Fish> matchList = newFish.GetMatches();
      profileData.Add("matchList", matchList);
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
      Console.WriteLine("Session ID at Create:" + newSessionId);
      return RedirectToAction("ViewProfile", new { sessionId = newSessionId});
    }

    [HttpGet("/fish/{sessionId}/update/password")]
    public ActionResult UpdatePasswordForm(int sessionId)
    {
      Fish thisFish = Fish.Find(sessionId);
      return View ("UpdatePassword", thisFish);
    }

    [HttpPost("/fish/{sessionId}/update/password")]
    public ActionResult UpdatePassword(int sessionId)
    {
      Fish thisFish = Fish.Find(sessionId);
      thisFish.UpdatePassword(Request.Form["updatePassword"]);
      return View ("UpdatePasswordConfirmation");
    }

    [HttpGet("/fish/{sessionId}/update/username")]
    public ActionResult UpdateName(int sessionId)
    {
      Fish thisFish = Fish.Find(sessionId);
      return View ("UpdateName", thisFish);
    }

    [HttpPost("/fish/{sessionId}/update/username")]
    public ActionResult UpdateUsername(int sessionId)
    {
      Fish thisFish = Fish.Find(sessionId);
      thisFish.UpdateFish(Request.Form["UpdateUsername"]);
      return View ("UpdateUsernameConfirmation");
    }

    //CONTROLLER ROUTES FOR VIEWING MATCHES AND WHEN ANOTHER USER LIKES A USER BUT ISN'T MATCHED

    [HttpGet("/fish/{sessionId}/matchmaking")]
    public ActionResult ViewMatchMaking(string name, int speciesId, string userName, string password)
    {
      var Fish = new Fish(name, speciesId, userName, password);
      List<Fish> userList = Fish.GetMatches();
      return View ();
    }

    [HttpGet("/fish/login")]
    public ActionResult FishLoginForm()
    {
      return View ("Login");
    }

    [HttpPost("/fish/login")]
    public ActionResult UserLogin()
    {
      int sessionId = Fish.Login(Request.Form["userName"], Request.Form["userPassword"]);
      return RedirectToAction("ViewProfile", new {sessionId= sessionId});
    }

  }
}
