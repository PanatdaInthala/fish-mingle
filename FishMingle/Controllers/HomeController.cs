using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FishMingle.Models;

namespace FishMingle.Controllers
{

  public class HomeController : Controller
  {

    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/success/changes")]
    public ActionResult SuccessChanges(int sessionId)
    {
      Dictionary<string, object> profileData = new Dictionary<string, object>();
      Fish newFish = Fish.Find(sessionId);
      profileData.Add("sessionId", sessionId);
      profileData.Add("newFish", newFish);
      return View(profileData);
    }

    [HttpGet("/fish/logout")]
    public ActionResult SuccessLogout()
    {
      return View();
    }
  }
}
