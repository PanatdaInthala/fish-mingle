using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FishMingle.Models;

namespace FishMingle.Controllers
{

  public class UserController : Controller
  {

    [HttpGet("/users")]
    public ActionResult Index()
    {
      return View();
    }
  }
}
