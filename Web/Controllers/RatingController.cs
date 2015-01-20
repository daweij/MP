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
  public class RatingController : BaseController
  {
    private const int MaximumTake = 50;
    private int MinimumVotes = 10000;


    public ActionResult Index(int id = MaximumTake)
    {
      return View(_ratings.Select(x => x.Votes > MinimumVotes)
        .OrderByDescending(x => x.Rating)
        .Take(id));
    }

    public ActionResult ByGenre(int id)
    {
      return View(_ratings.Select(x => x.Votes > MinimumVotes && x.Genres.Any(g => g.Id == id))
        .OrderByDescending(x => x.Rating)
        .Take(MaximumTake));
    }

    public ActionResult ByCountry(int id)
    {
      return View(_ratings.Select(x => x.Votes > MinimumVotes && x.Countries.Any(g => g.Id == id))
        .OrderByDescending(x => x.Rating)
        .Take(MaximumTake));
    }

    public ActionResult Filter()
    {
      var model = new RatingFilterVM();
      model.AvailibleCountries = _countries.Select();
      model.AvailibleGenres = _genres.Select();
      return View(model);
    }

    [HttpPost]
    public ActionResult Filter(RatingFilterVM model)
    {
      model.AvailibleCountries = _countries.Select();
      model.AvailibleGenres = _genres.Select();

      if (ModelState.IsValid)
      {
        model.Ratings = _ratings.Select(x =>
          (string.IsNullOrEmpty(model.SearchTerm) || x.Movie.Title.Contains(model.SearchTerm))
          && x.Genres.Select(genre => genre.Id).Intersect(model.SelectedGenres).Any()
          && x.Countries.Select(country => country.Id).Intersect(model.SelectedCountries).Any())
        .OrderByDescending(x => x.Rating)
        .Take(MaximumTake);
      }
      return View(model);
    }
  }
}