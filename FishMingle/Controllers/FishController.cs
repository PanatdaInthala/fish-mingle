using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FishMingle.Models;

namespace FishMingle.Controllers
{

  public class FishController : Controller
  {

    [HttpGet("/fish")]
    public ActionResult Index()
    {
      return View();
    }
  }
}
