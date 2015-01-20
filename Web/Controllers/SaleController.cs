using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Repository;
using Domain.Entities;
using Domain.Contracts;
using Web.Models;

namespace Web.Controllers
{
  public class SaleController : BaseController
  {
    private const int MaximumTake = 50;

    public ActionResult Index(int id = MaximumTake)
    {
      return View(_sales.Select(x => x.Revenue.HasValue)
        .OrderByDescending(x => x.Revenue.Value)
        .Take(id));
    }

    public ActionResult ByGenre(int id)
    {
      return View(_sales.Select(x => x.Revenue.HasValue && x.Genres.Any(g => g.Id == id))
        .OrderByDescending(x => x.Revenue.Value)
        .Take(MaximumTake));
    }

    public ActionResult ByCountry(int id)
    {
      return View(_sales.Select(x => x.Revenue.HasValue && x.Countries.Any(g => g.Id == id))
        .OrderByDescending(x => x.Revenue.Value)
        .Take(MaximumTake));
    }

    public ActionResult Filter()
    {
      var model = new SalesFilterVM();
      model.AvailibleCountries = _countries.Select();
      model.AvailibleGenres = _genres.Select();
      return View(model);
    }

    [HttpPost]
    public ActionResult Filter(SalesFilterVM model)
    {
      model.AvailibleCountries = _countries.Select();
      model.AvailibleGenres = _genres.Select();

      if (ModelState.IsValid)
      {
        model.Sales = _sales.Select(x =>
          (string.IsNullOrEmpty(model.SearchTerm) || x.Movie.Title.Contains(model.SearchTerm))
          && x.Genres.Select(genre => genre.Id).Intersect(model.SelectedGenres).Any()
          && x.Countries.Select(country => country.Id).Intersect(model.SelectedCountries).Any())
        .OrderByDescending(x => x.Revenue)
        .Take(MaximumTake);
      }
      return View(model);
    }

    public ActionResult Charts()
    {
      return View();
    }
  }
}