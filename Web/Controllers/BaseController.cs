using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
  public class BaseController : Controller
  {
    protected string ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
  }
}