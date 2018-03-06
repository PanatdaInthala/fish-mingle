using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FishMingle.Models;

namespace FishMingle.Controllers
{
  public class UserController : Controller
  {
    //USER IS ABLE TO BROWSE OTHER PROFILES ON THIS PAGE
    [HttpGet("/fish")]
    public ActionResult Fish()
    {
      List<Fish> fishList = Fish.GetAll();
      return View(fishList);
    }

    [HttpGet("/fish/{id}")]
    public ActionResult ViewProfile(int id)
    {
      Fish newFish = Fish.Find(id);
      return View(newFish);
    }

    //FORM FOR CREATING USER
    [HttpGet("/fish/create")]
    public ActionResult CreateFishForm()
    {
      return View("NewFishForm");
    }

    [HttpPost("/fish")]
    public ActionResult CreateFish()
    {
      string userNameActual = Request.Form["userName"];
      //THE USERS SPECIES ID WILL BE INPUT HERE
      string userNameProfile = Request.Form["userNameProfile"]);
      string userPassword = Request.Form["userPassword"]);
      Fish newFish = new Fish( userNameActual, speciesId, userNameProfile, userPassword );
      newFish.Save();

      return View("UserConfirmation", newFish);
    }

    [HttpGet("fish/{id}/update/password")]
    public ActionResult UpdatePasswordForm()
    {
      return View("UpdatePasswordForm");
    }

    [HttpPost("fish/{id}/update/password")]
    public ActionResult UpdatePassword()
    {
      Fish thisFish = Fish.Find(id);
      thisFish.UpdatePassword(Request.Form["updatePassword"]);
      return View ("UpdatePasswordConfirmation");
    }

    [HttpGet("fish/{id}/update/username")]
    public ActionResult UpdateUsername()
    {
      return View ("UpdateUserNameForm");
    }

    [HttpPost("fish/{id}/update/userName")]
    public ActionResult UpdateUsername()
    {
      Fish thisFish = Fish.Find(id);
      thisFish.UpdateFish(Request.Form["UpdateUsername"]);
      return View ("UpdateUsernameConfirmation");
    }

    //CONTROLLER ROUTES FOR VIEWING MATCHES AND WHEN ANOTHER USER LIKES A USER BUT ISN'T MATCHED

    [HttpGet("fish/{id}/matchmaking")]
    public ActionResult ViewMatchMaking()
    {
      List<Fish> userList = Fish.GetMatches();
      return View ()
    }

  }
}
