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
  public class SaleController : BaseController
  {
    private IRepository<FactSale> sales;
    private int MaximumTake = 100;

    public SaleController()
    {
      this.sales = new BaseRepository<FactSale>(ConnectionString);
    }

    // GET: Sale
    public ActionResult Top100()
    {
      return View(sales.Select(x => x.Revenue.HasValue)
        .OrderByDescending(x => x.Revenue.Value)
        .Take(MaximumTake));
    }
  }
}