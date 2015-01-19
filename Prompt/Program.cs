using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;
using Domain.Contracts;
using Repository;
using Prompt.Lib;
using System.Text.RegularExpressions;
using System.IO;
using Dapper;
using System.Data.SqlClient;

namespace Prompt
{
  class Program
  {
    private static string ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
    private static string FolderPath = ConfigurationManager.AppSettings["folder"].ToString();

    #region movie methods
    private static Regex MoviePattern = new Regex(@"^(?<title>.*?)\t+(?<created>[\d\?]{4}).*?$");
    private static Func<string, bool> IsMovie = (line) => MoviePattern.IsMatch(line);
    private static Func<string, DimMovie> LineToMovie = (line) =>
    {
      var match = MoviePattern.Match(line);
      int year = 0;
      int.TryParse(match.Groups["created"].Value, out year);
      return new DimMovie
      {
        Title = match.Groups["title"].Value,
        Year = year
      };
    };
    #endregion

    #region actor methods
    private static Regex ActorPattern = new Regex(@"^(?<actor>[^\t]+)\t+(?<title>.*?\s\([\d\?]{4}.*?\)).*?$");
    private static Func<string, bool> IsActor = (line) => ActorPattern.IsMatch(line);
    private static Func<string, string> LineToActor = (line) => line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries)[0];
    #endregion

    #region director methods
    private static Regex DirectorPattern = new Regex(@"^(?<director>[^\t]+)\t+(?<title>.*?\s\([\d\?]{4}.*?\)).*?$");
    private static Func<string, bool> IsDirector = (line) => DirectorPattern.IsMatch(line);
    private static Func<string, string> LineToDirector = (line) => line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries)[0];
    #endregion

    #region genre methods
    private static Regex GenrePattern = new Regex(@"^(?<title>[^\t]+)\t+(?<genre>[a-zA-Z-]*?)\s*$");
    private static Func<string, bool> IsGenre = (line) => GenrePattern.IsMatch(line);
    private static Func<string, string> LineToGenre = (line) =>
    {
      string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
      return parts[parts.Length - 1];
    };
    #endregion

    #region country methods
    private static Regex CountryPattern = new Regex(@"^(?<title>[^\t]+)\t+(?<country>.*?)\s*$");
    private static Func<string, bool> IsCountry = (line) => CountryPattern.IsMatch(line);
    private static Func<string, string> LineToCountry = (line) =>
    {
      string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
      return parts[parts.Length - 1];
    };
    #endregion

    static void Main(string[] args)
    {
      Console.WriteLine("Sort actors: {0}.", DateTime.Now);
      ActorHelper.Save();
      Console.WriteLine("Sort complete: {0}.", DateTime.Now);

      Console.WriteLine("Sort directors: {0}.", DateTime.Now);
      DirectorHelper.Save();
      Console.WriteLine("Sort complete: {0}.", DateTime.Now);

      Console.WriteLine("Sort business: {0}.", DateTime.Now);
      BusinessHelper.Save();
      Console.WriteLine("Sort complete: {0}.", DateTime.Now);

      Console.WriteLine("Sort genres: {0}.", DateTime.Now);
      GenreHelper.Save();
      Console.WriteLine("Sort complete: {0}.", DateTime.Now);

      Console.WriteLine("Sort countries: {0}.", DateTime.Now);
      CountryHelper.Save();
      Console.WriteLine("Sort complete: {0}.", DateTime.Now);

      Console.WriteLine("Sort ratings: {0}.", DateTime.Now);
      RatingHelper.Save();
      Console.WriteLine("Sort complete: {0}.", DateTime.Now);


      var movies = new List<string>();
      movies.Add("Avatar (2009)");

      
      Console.WriteLine("Load started: {0}.", DateTime.Now);
      var genres = GetGenres();
      var countries = GetCountries();
      var actors = GetActors();
      var directors = GetDirectors();

      //var movies = GetMovies();



      //Console.WriteLine("Loaded complete: {0}.", DateTime.Now);
      //Console.WriteLine("Save started: {0}.", DateTime.Now);
      
      //File.WriteAllLines(Path.Combine(FolderPath, "genres.sql"), genres.Select(genre => genre.Value.ToSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} genres to file: {1}.", genres.Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "countries.sql"), countries.Select(country => country.Value.ToSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} countries to DB: {1}.", countries.Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "actors.sql"), actors.Select(actor => actor.Value.ToSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} actors to DB: {1}.", actors.Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "directors.sql"), directors.Select(director => director.Value.ToSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} directors to DB: {1}.", directors.Count(), DateTime.Now);

      //Console.WriteLine("Save complete: {0}.", DateTime.Now);

      

      
      //var context = new MPContext(ConnectionString);
      //IRepository<FactSale> sales = new BaseRepository<FactSale>(context);
      //IRepository<FactRating> ratings = new BaseRepository<FactRating>(context);
      //IRepository<DimMovie> movies = new BaseRepository<DimMovie>(context);

      //Console.WriteLine("Amount of sales: {0}.", sales.Count());
      //Console.WriteLine("Amount of ratings: {0}.", ratings.Count());
      //Console.WriteLine("Amount of movies: {0}.", movies.Count());

      Console.ReadKey();
    }

    private static Dictionary<string, DimGenre> GetGenres()
    {
      int id = 1;
      var genres = new Dictionary<string, DimGenre>();
      IEnumerable<string> DistinctGenres = File.ReadLines(Path.Combine(FolderPath, "genres.list"), Encoding.Default)
        .Where(IsGenre)
        .Select(LineToGenre)
        .Distinct();

      foreach (var genre in DistinctGenres)
      {
        genres.Add(genre, new DimGenre { Id = id++, Name = genre });
      }

      return genres;
    }

    private static Dictionary<string, DimCountry> GetCountries()
    {
      int id = 1;
      var countries = new Dictionary<string, DimCountry>();
      IEnumerable<string> DistinctCountries = File.ReadLines(Path.Combine(FolderPath, "countries.list"), Encoding.Default)
        .Where(IsCountry)
        .Select(LineToCountry)
        .Distinct();

      foreach (var country in DistinctCountries)
      {
        countries.Add(country, new DimCountry { Id = id++, Name = country });
      }

      return countries;
    }

    private static Dictionary<string, DimActor> GetActors()
    {
      int id = 1;
      var actors = new Dictionary<string, DimActor>();
      IEnumerable<string> DistinctActors = File.ReadLines(Path.Combine(FolderPath, "actors.list"), Encoding.Default)
        .Where(IsActor)
        .Select(LineToActor)
        .Distinct();

      foreach (var actor in DistinctActors)
      {
        actors.Add(actor, new DimActor { Id = id++, Name = actor });
      }

      return actors;
    }

    private static Dictionary<string, DimDirector> GetDirectors()
    {
      int id = 1;
      var directors = new Dictionary<string, DimDirector>();
      IEnumerable<string> DistinctDirectors = File.ReadLines(Path.Combine(FolderPath, "directors.list"), Encoding.Default)
        .Where(IsDirector)
        .Select(LineToDirector)
        .Distinct();

      foreach (var director in DistinctDirectors)
      {
        directors.Add(director, new DimDirector { Id = id++, Name = director });
      }

      return directors;
    }

    private static Dictionary<string, DimMovie> GetMovies()
    {


      return null;
    }
  }
}
