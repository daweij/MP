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
      model.AvailibleCountries = _countries.Select().OrderBy(x => x.Name);
      model.AvailibleGenres = _genres.Select().OrderBy(x => x.Name);
      return View(model);
    }

    [HttpPost]
    public ActionResult Filter(SalesFilterVM model)
    {
      model.AvailibleCountries = _countries.Select().OrderBy(x => x.Name);
      model.AvailibleGenres = _genres.Select().OrderBy(x => x.Name);

      if (ModelState.IsValid)
      {
        model.Sales = _sales.Select(x => (string.IsNullOrEmpty(model.SearchTerm) || x.Movie.Title.Contains(model.SearchTerm))
          && x.Genres.Select(g => g.Id).Intersect(model.SelectedGenres).Any()
          && x.Countries.Select(c => c.Id).Intersect(model.SelectedCountries).Any())
        .OrderByDescending(x => x.Revenue).ThenByDescending(x => x.Budget)
        .Take(MaximumTake);
      }
      return View(model);
    }

    public ActionResult Charts()
    {
      return View();
    }




    #region JSON
    public JsonResult BudgetByYear(bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), "BudgetByYear.json");
      var model = new List<ChartVM>();

      if (System.IO.File.Exists(filePath) && Update == false)
      {
        model = JsonConvert.DeserializeObject<List<ChartVM>>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
        return Json(model, JsonRequestBehavior.AllowGet);
      }
      else
      {
        var budgetGroup = _sales.Select(x => x.Budget.HasValue && x.Revenue.HasValue && x.Movie != null && x.Movie.Year > 0).ToList().GroupBy(fact => fact.Movie.Year);
        var budgetItem = new ChartVM();
        budgetItem.label = "Budget";
        foreach (var group in budgetGroup.OrderBy(g => g.Key))
        {
          budgetItem.data.Add(new double[] { group.Key, (group.Sum(g => g.Budget).Value / 1000) });
        }
        model.Add(budgetItem);

        var revenueGroup = _sales.Select(x => x.Revenue.HasValue && x.Budget.HasValue && x.Movie != null && x.Movie.Year > 0).ToList().GroupBy(fact => fact.Movie.Year);
        var revenueItem = new ChartVM();
        revenueItem.label = "Revenue";
        foreach (var group in revenueGroup.OrderBy(g => g.Key))
        {
          revenueItem.data.Add(new double[] { group.Key, (group.Sum(g => g.Revenue).Value / 1000) });
        }
        model.Add(revenueItem);

        // save it for later!!
        System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(model.ToArray(), Formatting.Indented), System.Text.Encoding.UTF8);

        return Json(model, JsonRequestBehavior.AllowGet);
      }
    }

    public JsonResult BudgetByGenre(bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), "BudgetByGenre.json");
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
          var byGenre = _sales.Select(x => x.Revenue.HasValue && x.Budget.HasValue && x.Movie != null && x.Movie.Year > 0).ToList().Where(x => x.Genres.Select(g => g.Id).Contains(genre.Id)).ToList();
          var data = byGenre.Sum(x => x.Budget.Value);
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

    public JsonResult RevenueByGenre(bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), "RevenueByGenre.json");
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
          var byGenre = _sales.Select(x => x.Revenue.HasValue && x.Budget.HasValue && x.Movie != null && x.Movie.Year > 0).ToList().Where(x => x.Genres.Select(g => g.Id).Contains(genre.Id)).ToList();
          var data = byGenre.Sum(x => x.Revenue.Value);
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


    public JsonResult BudgetByCountry(bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), "BudgetByCountry.json");
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
          var byCountry = _sales.Select(x => x.Revenue.HasValue && x.Budget.HasValue && x.Movie != null && x.Movie.Year > 0).ToList().Where(x => x.Countries.Select(c => c.Id).Contains(country.Id)).ToList();
          var data = byCountry.Sum(x => x.Budget.Value);
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

    public JsonResult RevenueByCountry(bool Update = false)
    {
      var filePath = Path.Combine(Server.MapPath("/App_Data"), "RevenueByCountry.json");
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
          var byCountry = _sales.Select(x => x.Revenue.HasValue && x.Budget.HasValue && x.Movie != null && x.Movie.Year > 0).ToList().Where(x => x.Countries.Select(c => c.Id).Contains(country.Id)).ToList();
          var data = byCountry.Sum(x => x.Budget.Value);
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

    //public JsonResult BudgetByDirector(bool Update = false)
    //{
    //  var filePath = Path.Combine(Server.MapPath("/App_Data"), "BudgetByDirector.json");
    //  var model = new List<SimpleChartVM>();

    //  if (System.IO.File.Exists(filePath) && Update == false)
    //  {
    //    model = JsonConvert.DeserializeObject<List<SimpleChartVM>>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
    //    return Json(model, JsonRequestBehavior.AllowGet);
    //  }
    //  else
    //  {
    //    foreach (var director in _directors.Select(x => x.Movies != null && x.Movies.Select(m => m.Sale).Any()))
    //    {
    //      var byDirector = _sales.Select(x => x.Revenue.HasValue && x.Budget.HasValue && x.Movie != null && x.Movie.Year > 0).ToList().Where(x => x.Movie.Directors.Select(c => c.Id).Contains(director.Id)).ToList();
    //      var data = byDirector.Sum(x => x.Budget.Value);
    //      if (data > 0)
    //      {
    //        model.Add(new SimpleChartVM
    //        {
    //          label = director.Name,
    //          data = (data / 1000)
    //        });
    //      }
    //    }

    //    // save it for later!!
    //    System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(model.ToArray(), Formatting.Indented), System.Text.Encoding.UTF8);

    //    return Json(model, JsonRequestBehavior.AllowGet);
    //  }
    //}

    //public JsonResult RevenueByDirector(bool Update = false)
    //{
    //  var filePath = Path.Combine(Server.MapPath("/App_Data"), "RevenueByDirector.json");
    //  var model = new List<SimpleChartVM>();

    //  if (System.IO.File.Exists(filePath) && Update == false)
    //  {
    //    model = JsonConvert.DeserializeObject<List<SimpleChartVM>>(System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8));
    //    return Json(model, JsonRequestBehavior.AllowGet);
    //  }
    //  else
    //  {
    //    //foreach (var director in _directors.Select(x => x.Movies != null).ToList())
    //    //{
    //    //  var byDirector = _sales.Select(x => x.Revenue.HasValue && x.Budget.HasValue && x.Movie != null && x.Movie.Year > 0).ToList().Where(x => x.Movie.Directors.Select(c => c.Id).Contains(director.Id)).ToList();
    //    //  var data = byDirector.Sum(x => x.Revenue.Value);
    //    //  if (data > 0)
    //    //  {
    //    //    model.Add(new SimpleChartVM
    //    //    {
    //    //      label = director.Name,
    //    //      data = (data / 1000)
    //    //    });
    //    //  }
    //    //}

    //    // save it for later!!
    //    System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(model.ToArray(), Formatting.Indented), System.Text.Encoding.UTF8);

    //    return Json(model, JsonRequestBehavior.AllowGet);
    //  }
    //}
    #endregion
  }
}