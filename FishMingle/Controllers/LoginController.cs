using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FishMingle.Models;

namespace FishMingle.Controllers
{

  public class LoginController : Controller
  {

    [HttpGet("/login")]
    public ActionResult Index()
    {
      return View();
    }
  }
}
