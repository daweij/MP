using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Repository;
using Domain.Entities;
using Domain.Contracts;
using Web.Models;
using Newtonsoft.Json;
using System.IO;

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
      model.AvailibleCountries = _countries.Select().OrderBy(x => x.Name);
      model.AvailibleGenres = _genres.Select().OrderBy(x => x.Name);
      return View(model);
    }

    [HttpPost]
    public ActionResult Filter(RatingFilterVM model)
    {
      model.AvailibleCountries = _countries.Select().OrderBy(x => x.Name);
      model.AvailibleGenres = _genres.Select().OrderBy(x => x.Name);

      if (ModelState.IsValid)
      {
        model.Ratings = _ratings.Select(x => (string.IsNullOrEmpty(model.SearchTerm) || x.Movie.Title.Contains(model.SearchTerm)))
        .OrderByDescending(x => x.Rating)
        .Take(MaximumTake);

        if (model.SelectedGenres != null && model.SelectedGenres.Any())
          model.Ratings = model.Ratings.Where(x => model.SelectedGenres.All(genre => x.Genres.Select(y => y.Id).Contains(genre)));

        if (model.SelectedCountries != null && model.SelectedCountries.Any())
          model.Ratings = model.Ratings.Where(x => model.SelectedCountries.All(country => x.Countries.Select(y => y.Id).Contains(country)));

      }
      return View(model);
    }

    public ActionResult Charts()
    {
      return View();
    }


    #region JSON
    public JsonResult RatingByYear(int id = 100, bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), string.Format("RatingByYear{0}.json", id));
      var model = new List<ChartVM>();

      if (System.IO.File.Exists(filePath) && Update == false)
      {
        model = JsonConvert.DeserializeObject<List<ChartVM>>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
        return Json(model, JsonRequestBehavior.AllowGet);
      }
      else
      {
        var ratingGroup = _ratings.Select(x => x.Votes > id && x.Movie != null && x.Movie.Year > 0).ToList().GroupBy(fact => fact.Movie.Year);
        var ratingItem = new ChartVM();
        ratingItem.label = "Rating";
        foreach (var group in ratingGroup.OrderBy(g => g.Key))
        {
          ratingItem.data.Add(new double[] { group.Key, group.Average(g => g.Rating) });
        }
        model.Add(ratingItem);

        // save it for later!!
        System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(model.ToArray(), Formatting.Indented), System.Text.Encoding.UTF8);

        return Json(model, JsonRequestBehavior.AllowGet);
      }
    }



    public JsonResult RatingByGenre(bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), "RatingByGenre.json");
      var model = new List<SimpleChartVM>();

      if (System.IO.File.Exists(filePath) && Update == false)
      {
        model = JsonConvert.DeserializeObject<List<SimpleChartVM>>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
        return Json(model, JsonRequestBehavior.AllowGet);
      }
      else
      {
        foreach (var genre in _genres.Select().ToList())
        {
          var byGenre = _ratings.Select(x => x.Movie != null && x.Movie.Year > 0 && x.Genres.Select(g => g.Id).Contains(genre.Id)).ToList();
          if (byGenre.Any())
          {
            var data = byGenre.Average(x => x.Rating);
            if (data > 0)
            {
              model.Add(new SimpleChartVM
              {
                label = genre.Name,
                data = Math.Round(data, 2)
              });
            }
          }
        }
      }

      model = model.OrderByDescending(x => x.data).ToList();

      // save it for later!!
      System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(model.ToArray(), Formatting.Indented), System.Text.Encoding.UTF8);

      return Json(model, JsonRequestBehavior.AllowGet);
    }

    public JsonResult VotesByGenre(bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), "VotesByGenre.json");
      var model = new List<SimpleChartVM>();

      if (System.IO.File.Exists(filePath) && Update == false)
      {
        model = JsonConvert.DeserializeObject<List<SimpleChartVM>>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
        return Json(model, JsonRequestBehavior.AllowGet);
      }
      else
      {
        foreach (var genre in _genres.Select().ToList())
        {
          var byGenre = _ratings.Select(x => x.Movie != null && x.Movie.Year > 0 && x.Genres.Select(g => g.Id).Contains(genre.Id)).ToList();
          var data = byGenre.Sum(x => x.Votes);
          if (data > 0)
          {
            model.Add(new SimpleChartVM
            {
              label = genre.Name,
              data = (data / 1000)
            });
          }
        }
      }

      // save it for later!!
      System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(model.ToArray(), Formatting.Indented), System.Text.Encoding.UTF8);

      return Json(model, JsonRequestBehavior.AllowGet);
    }


    public JsonResult RatingByCountry(bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), "RatingByCountry.json");
      var model = new List<SimpleChartVM>();

      if (System.IO.File.Exists(filePath) && Update == false)
      {
        model = JsonConvert.DeserializeObject<List<SimpleChartVM>>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
        return Json(model, JsonRequestBehavior.AllowGet);
      }
      else
      {
        foreach (var country in _countries.Select().ToList())
        {
          //var byCountry = _ratings.Select(x => x.Movie != null && x.Movie.Year > 0).ToList().Where(x => x.Countries.Select(c => c.Id).Contains(country.Id)).ToList();
          var byCountry = _ratings.Select(x => x.Movie != null && x.Movie.Year > 0 && x.Countries.Select(c => c.Id).Contains(country.Id)).ToList();
          if (byCountry.Any())
          {
            var data = byCountry.Average(x => x.Rating);
            if (data > 0)
            {
              model.Add(new SimpleChartVM
              {
                label = country.Name,
                data = data
              });
            }
          }

        }
      }

      // save it for later!!
      System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(model.ToArray(), Formatting.Indented), System.Text.Encoding.UTF8);

      return Json(model, JsonRequestBehavior.AllowGet);
    }

    public JsonResult VotesByCountry(bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), "VotesByCountry.json");
      var model = new List<SimpleChartVM>();

      if (System.IO.File.Exists(filePath) && Update == false)
      {
        model = JsonConvert.DeserializeObject<List<SimpleChartVM>>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
        return Json(model, JsonRequestBehavior.AllowGet);
      }
      else
      {
        foreach (var country in _countries.Select().ToList())
        {
          var byCountry = _ratings.Select(x => x.Movie != null && x.Movie.Year > 0 && x.Countries.Select(c => c.Id).Contains(country.Id)).ToList();
          var data = byCountry.Sum(x => x.Votes);
          if (data > 0)
          {
            model.Add(new SimpleChartVM
            {
              label = country.Name,
              data = (data / 1000)
            });
          }
        }
      }

      // save it for later!!
      System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(model.ToArray(), Formatting.Indented), System.Text.Encoding.UTF8);

      return Json(model, JsonRequestBehavior.AllowGet);
    }

    #endregion
  }
}