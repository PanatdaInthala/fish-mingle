using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FishMingle.Models;

namespace FishMingle.Controllers
{

  public class InterestController : Controller
  {

    [HttpGet("/interest")]
    public ActionResult Index()
    {
      return View();
    }
  }
}
