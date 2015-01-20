using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Repository;
using Domain.Entities;
using Domain.Contracts;
using System.Data.Entity;

namespace Web.Controllers
{
  public class BaseController : Controller
  {
    protected string _connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
    protected DbContext _context;
    protected IRepository<FactRating> _ratings;
    protected IRepository<FactSale> _sales;
    protected IRepository<DimCountry> _countries;
    protected IRepository<DimGenre> _genres;

    public BaseController()
    {
      this._context = new MPContext(_connectionString);
      this._ratings = new BaseRepository<FactRating>(_context);
      this._sales = new BaseRepository<FactSale>(_context);
      this._countries = new BaseRepository<DimCountry>(_context);
      this._genres = new BaseRepository<DimGenre>(_context);
    }

  }
}