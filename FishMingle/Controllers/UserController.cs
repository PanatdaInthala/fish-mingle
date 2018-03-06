using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FishMingle.Models;

namespace FishMingle.Controllers
{

  public class UserController : Controller
  {
    //USER IS ABLE TO BROWSE OTHER PROFILES ON THIS PAGE
    [HttpGet("/users")]
    public ActionResult Users()
    {
      List<User> userList = User.GetAll();
      return View(userList);
    }

    //FORM FOR CREATING USER
    [HttpGet("/users/create")]
    public ActionResult CreateUserForm()
    {
      return View("NewUserForm");
    }

    [HttpPost("/users")]
    public ActionResult CreateUser()
    {
      string userNameActual = Request.Form["userName"];
      //THE USERS SPECIES ID WILL BE INPUT HERE
      string userNameProfile = Request.Form["userNameProfile"]);
      string userPassword = Request.Form["userPassword"]);
      User newUser = new User( userNameActual, speciesId, userNameProfile, userPassword );
      newUser.Save();

      return View("UserConfirmation", newUser);
    }

    [HttpGet("users/{id}/update/password")]
    public ActionResult UpdatePasswordForm()
    {
      return View("UpdatePasswordForm");
    }

    [HttpPost("users/{id}/update/password")]
    public ActionResult UpdatePassword()
    {
      User thisUser = User.Find(id);
      thisUser.UpdatePassword(Request.Form["updatePassword"]);
      return View ("UpdatePasswordConfirmation");
    }

    [HttpGet("users/{id}/update/username")]
    public ActionResult UpdateUsername()
    {
      return View ("UpdateUserNameForm");
    }

    [HttpPost("users/{id}/update/userName")]
    public ActionResult UpdateUsername()
    {
      User thisUser = User.Find(id);
      thisUser.UpdateUser(Request.Form["UpdateUsername"]);
      return View ("UpdateUsernameConfirmation");
    }
  }
}
