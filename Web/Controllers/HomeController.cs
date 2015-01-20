using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Repository;
using Domain.Entities;
using Domain.Contracts;

namespace Web.Controllers
{
  public class HomeController : BaseController
  {
    public ActionResult Index()
    { 
      return View();
    }
  }
}