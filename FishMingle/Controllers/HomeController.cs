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
    public ActionResult SuccessChanges()
    {
      return View();
    }

    [HttpGet("/fish/logout")]
    public ActionResult SuccessLogout()
    {
      return View();
    }
  }
}
