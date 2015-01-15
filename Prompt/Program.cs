using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;
using Domain.Contracts;
using Repository;

namespace Prompt
{
  class Program
  {
    static void Main(string[] args)
    {
      var connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
      var context = new MPContext(connectionString);
      IRepository<FactSale> sales = new BaseRepository<FactSale>(context);
      IRepository<FactRating> ratings = new BaseRepository<FactRating>(context);
      IRepository<DimMovie> movies = new BaseRepository<DimMovie>(context);

      Console.WriteLine("Amount of sales: {0}.", sales.Count());
      Console.WriteLine("Amount of ratings: {0}.", ratings.Count());
      Console.WriteLine("Amount of movies: {0}.", movies.Count());

      Console.ReadKey();
    }
  }
}
