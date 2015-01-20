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
  public class RatingController : BaseController
  {
    private IRepository<FactRating> ratings;
    private int MaximumTake = 100;
    private int MinimumVotes = 10000;

    public RatingController()
    {
      this.ratings = new BaseRepository<FactRating>(ConnectionString);
    }

    public ActionResult Top100()
    {
      return View(ratings.Select(x => x.Votes > MinimumVotes)
        .OrderByDescending(x => x.Rating)
        .Take(MaximumTake));
    }

    public ActionResult ByGenre(int id)
    {
      return View(ratings.Select(x => x.Votes > MinimumVotes && x.Genres.Any(g => g.Id == id))
        .OrderByDescending(x => x.Rating)
        .Take(MaximumTake));
    }

    public ActionResult ByCountry(int id)
    {
      return View(ratings.Select(x => x.Votes > MinimumVotes && x.Countries.Any(g => g.Id == id))
        .OrderByDescending(x => x.Rating)
        .Take(MaximumTake));
    }
  }
}