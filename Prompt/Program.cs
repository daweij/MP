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
  //public struct Tuple<T1, T2>
  //{
  //  public readonly T1 Item1;
  //  public readonly T2 Item2;
  //  public Tuple(T1 item1, T2 item2) { Item1 = item1; Item2 = item2; }
  //}

  class Program
  {
    private static string ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
    private static string FolderPath = ConfigurationManager.AppSettings["folder"].ToString();

    private static Dictionary<string, DimCountry> countries;
    private static Dictionary<Tuple<ActorGender, string>, DimActor> actors;
    private static Dictionary<string, DimGenre> genres;
    private static Dictionary<string, DimDirector> directors;
    private static Dictionary<string, DimMovie> movies;
    private static Dictionary<string, List<DimGenre>> genresByMovie;
    private static Dictionary<string, List<DimCountry>> countriesByMovie;
    private static Dictionary<string, List<DimDirector>> directorsByMovie;

    static void Main(string[] args)
    {
      //Console.WriteLine("Sort actors: {0}.", DateTime.Now);
      //ActorHelper.Save();

      //Console.WriteLine("Sort directors: {0}.", DateTime.Now);
      //DirectorHelper.Save();

      //Console.WriteLine("Sort business: {0}.", DateTime.Now);
      //BusinessHelper.Save();

      //Console.WriteLine("Sort ratings: {0}.", DateTime.Now);
      //RatingHelper.Save();

      //Console.WriteLine("\n---------------------------------------------");
      //Console.WriteLine("SORT COMPLETE: {0}", DateTime.Now);
      //Console.WriteLine("---------------------------------------------\n");


      //Console.WriteLine("Loading actors: {0}.", DateTime.Now);
      //actors = GetActors();

      //Console.WriteLine("Loading genres: {0}.", DateTime.Now);
      //genres = GetGenres();
      //Console.WriteLine("– by movie: {0}.", DateTime.Now);
      //genresByMovie = GetGenresByMovie();

      //Console.WriteLine("Loading countries: {0}.", DateTime.Now);
      //countries = GetCountries();
      //Console.WriteLine("– by movie: {0}.", DateTime.Now);
      //countriesByMovie = GetCountriesByMovie();

      //Console.WriteLine("Loading directors: {0}.", DateTime.Now);
      //directors = GetDirectors();
      //Console.WriteLine("– by movie: {0}.", DateTime.Now);
      //directorsByMovie = GetDirectorsByMovie();

      //Console.WriteLine("Loading movies: {0}.", DateTime.Now);
      //movies = GetMovies();

      Console.WriteLine("LOAD COMPLETE: {0}", DateTime.Now);
      Console.WriteLine("\n---------------------------------------------");
      //Console.WriteLine("Actors: {0}", actors.Count);
      //Console.WriteLine("Genres: {0}/{1}", genresByMovie.Count, genres.Count);
      //Console.WriteLine("Countries: {0}/{1}", countriesByMovie.Count, countries.Count);
      //Console.WriteLine("Directors: {0}", directors.Count);
      //Console.WriteLine("Movies: {0}", movies.Count);
      Console.WriteLine("---------------------------------------------\n");

      //File.WriteAllLines(Path.Combine(FolderPath, "countries.sql"), countries.Select(country => country.Value.ToSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} country to file: {1}.", countries.Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "genres.sql"), genres.Select(genre => genre.Value.ToSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} genres to file: {1}.", genres.Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "actors.sql"), actors.Select(actor => actor.Value.ToSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} actors to DB: {1}.", actors.Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "directors.sql"), directors.Select(director => director.Value.ToSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} directors to DB: {1}.", directors.Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "movies.sql"), movies.Select(movie => movie.Value.ToSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} movies to DB: {1}.", movies.Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "x_movie_country.sql"), movies.Where(movie => movie.Value.Countries != null && movie.Value.Countries.Any()).Select(movie => movie.Value.ToCountrySqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} movie-country relations to DB: {1}.", movies.SelectMany(x => x.Value.Countries).Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "x_movie_genres.sql"), movies.Where(movie => movie.Value.Genres != null && movie.Value.Genres.Any()).Select(movie => movie.Value.ToGenreSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} movie-genre relations to DB: {1}.", movies.SelectMany(x => x.Value.Genres).Count(), DateTime.Now);

      //File.WriteAllLines(Path.Combine(FolderPath, "x_movie_directors.sql"), movies.Where(movie => movie.Value.Directors != null && movie.Value.Directors.Any()).Select(movie => movie.Value.ToDirectorSqlInsert()), Encoding.UTF8);
      //Console.WriteLine("Saved {0} movie-director relations to DB: {1}.", movies.SelectMany(x => x.Value.Directors).Count(), DateTime.Now);

      //Console.WriteLine("Save complete: {0}.", DateTime.Now);


      //SaveDirectorMovieConnections();
      //SaveBussinessFacts();
      //SaveRatingFacts();




      Console.WriteLine("\n---------------------------------------------");
      Console.WriteLine("SAVE TO DATABASE: {0}", DateTime.Now);
      using (var conn = new SqlConnection(ConnectionString))
      {
        foreach (var file in Directory.EnumerateFiles(FolderPath, "*.sql", SearchOption.AllDirectories))
        {
          Console.WriteLine("Saving {0}: {1}", Path.GetFileName(file), DateTime.Now);
          foreach (var line in File.ReadLines(file, Encoding.UTF8))
          {
            try
            {
              conn.Execute(line);
            }
            catch (Exception)
            {
              throw;
            }
          }
        }
      }

      Console.WriteLine("---------------------------------------------\n");
      Console.WriteLine("ALLES DONE!");


      //var context = new MPContext(ConnectionString);
      //IRepository<FactSale> sales = new BaseRepository<FactSale>(context);
      //IRepository<FactRating> ratings = new BaseRepository<FactRating>(context);
      //IRepository<DimMovie> movies = new BaseRepository<DimMovie>(context);

      //Console.WriteLine("Amount of sales: {0}.", sales.Count());
      //Console.WriteLine("Amount of ratings: {0}.", ratings.Count());
      //Console.WriteLine("Amount of movies: {0}.", movies.Count());

      Console.ReadKey();
    }

    private static void SaveDirectorMovieConnections()
    {
      var directorMovies = File.ReadLines(Path.Combine(FolderPath, "_fix", "directors.list"), Encoding.Default).Select(line =>
      {
        var pattern = new Regex(@"^(?<movie>.*?)(  \(.*?\))?\t{3}(?<director>.*?)\s*$");
        var match = pattern.Match(line);
        var movieId = 0;
        var directorId = 0;

        if (movies.ContainsKey(match.Groups["movie"].Value))
          movieId = movies[match.Groups["movie"].Value].Id;

        if (directors.ContainsKey(match.Groups["director"].Value))
          directorId = directors[match.Groups["director"].Value].Id;

        if (movieId > 0 && directorId > 0)
          return string.Format(@"INSERT INTO [DirectorMovies] ([DirectorId], [MovieId]) VALUES ({0}, {1});", directorId, movieId);
        return string.Format(@"--INSERT INTO [DirectorMovies] ([DirectorId], [MovieId]) VALUES ('{0}', '{1}');", match.Groups["movie"].Value, match.Groups["director"].Value);
      });
      File.WriteAllLines(Path.Combine(FolderPath, "_fix", "x_directors.sql"), directorMovies, Encoding.UTF8);
      Console.WriteLine("Saved {0} directorConnections to DB: {1}.", directorMovies.Count(), DateTime.Now);
    }


    private static void SaveRatingFacts()
    {
      var ratingFacts = File.ReadLines(Path.Combine(FolderPath, "_fix", "ratings.list"), Encoding.Default).Select((line, i) =>
      {
        var pattern = new Regex(@"^(?<movie>.*?)\t+(?<rating>.*?)\t+(?<votes>.*?)\s*$");
        var match = pattern.Match(line);
        DimMovie movie = null;
        var rating = decimal.Parse(match.Groups["rating"].Value.Replace('.', ','));
        var votes = long.Parse(match.Groups["votes"].Value);

        if (movies.ContainsKey(match.Groups["movie"].Value))
          movie = movies[match.Groups["movie"].Value];

        if (movie != null)
        {
          var strings = new List<string>();
          strings.Add(string.Format(@"INSERT INTO [FactRatings] ([Id], [MovieId], [Rating], [Votes]) VALUES ({0}, {1}, {2}, {3});", i + 1, movie.Id, rating.ToString().Replace(',', '.'), votes));
          
          foreach (var genre in movie.Genres)
            strings.Add(string.Format("INSERT INTO [FactRatingGenres] ([FactRatingId], [GenreId]) VALUES ({0}, {1})", i + 1, genre.Id));
          
          foreach (var country in movie.Countries)
            strings.Add(string.Format("INSERT INTO [FactRatingCountries] ([FactRatingId], [CountryId]) VALUES ({0}, {1})", i + 1, country.Id));
          
          return string.Join("", strings.ToArray());
        }
        return string.Format(@"--INSERT INTO [FactRatings] ([Id], [MovieId], [Year], [Revenue]) VALUES ({0});", line);
      });

      File.WriteAllLines(Path.Combine(FolderPath, "_fix", "ratings.sql"), ratingFacts, Encoding.UTF8);
      Console.WriteLine("Saved {0} FactRatings to DB: {1}.", ratingFacts.Count(), DateTime.Now);
    }

    private static void SaveBussinessFacts()
    {
      int id = 1;
      var b = new Dictionary<int, FactSale>();
      var businessFacts = File.ReadLines(Path.Combine(FolderPath, "_fix", "business.list"), Encoding.Default).Select(line =>
      {
        var pattern = new Regex(@"^(?<movie>.*?)\t+(?<amount>.*?)\t+(?<type>.*?)\s*$");
        var match = pattern.Match(line);
        DimMovie movie = null;
        long amount = 0;
        long.TryParse(match.Groups["amount"].Value.Replace(",", ""), out amount);
        var type = match.Groups["type"].Value.ToLower();

        if (movies.ContainsKey(match.Groups["movie"].Value))
        {
          movie = movies[match.Groups["movie"].Value];
          
          if (b.ContainsKey(movie.Id))
          {
            if (type == "bt")
              b[movie.Id].Budget = amount;
            else if (type == "worldwide")
              b[movie.Id].Revenue = amount;
            else if (type == "usa")
              b[movie.Id].RevenueUSA = amount;
            else if (type == "non-usa")
              b[movie.Id].RevenueNonUSA = amount;
          }
          else
          {
            var fact = new FactSale { Id = id++ };
            fact.Movie = movie;
            if (type == "bt")
              fact.Budget = amount;
            else if (type == "worldwide")
              fact.Revenue = amount;
            else if (type == "usa")
              fact.RevenueUSA = amount;
            else if (type == "non-usa")
              fact.RevenueNonUSA = amount;

            b.Add(movie.Id, fact);
          }

        }

        
        //if (movie != null)
        //  return string.Format(@"INSERT INTO [FactSales] ([Id], [MovieId], [Year], [Revenue]) VALUES ({0}, {1}, {2}, {3}, N'{4}');", id++, movie.Id, movie.Year, amount, type);
        return string.Format(@"--INSERT INTO [FactSales] ([Id], [MovieId], [Year], [Revenue]) VALUES ({0}, {1}, {2}, {3}, N'{4}');", 0, match.Groups["movie"].Value, 0, amount, type);
      }).ToList();

      File.WriteAllLines(Path.Combine(FolderPath, "_fix", "business.sql"), b.Select(MovieFactsToSql).ToList(), Encoding.UTF8);
      Console.WriteLine("Saved {0} BusinessFacts to DB: {1}.", b.Count(), DateTime.Now);
    }

    private static string MovieFactsToSql(KeyValuePair<int, FactSale> x)
    {
      var strings = new List<string>();
      strings.Add(string.Format(@"INSERT INTO [FactSales] ([Id], [MovieId], [Budget], [Revenue], [RevenueUSA], [RevenueNonUSA]) VALUES ({0}, {1}, {2}, {3}, {4}, {5});", 
        x.Value.Id, 
        x.Value.Movie.Id, 
        x.Value.Budget.HasValue ? x.Value.Budget.ToString() : "NULL", 
        x.Value.Revenue.HasValue ? x.Value.Revenue.ToString() : "NULL", 
        x.Value.RevenueUSA.HasValue ? x.Value.RevenueUSA.ToString() : "NULL", 
        x.Value.RevenueNonUSA.HasValue ? x.Value.RevenueNonUSA.ToString() : "NULL"));

      foreach (var genre in x.Value.Movie.Genres)
        strings.Add(string.Format("INSERT INTO [FactSaleGenres] ([FactSaleId], [GenreId]) VALUES ({0}, {1});", x.Value.Id, genre.Id));

      foreach (var country in x.Value.Movie.Countries)
        strings.Add(string.Format("INSERT INTO [FactSaleCountries] ([FactSaleId], [CountryId]) VALUES ({0}, {1});", x.Value.Id, country.Id));

      return string.Join("", strings.ToArray());
    }

    #region private parts

    // Genres
    private static Regex GenrePattern = new Regex(@"^(?<title>[^\t]+)\t+(?<genre>[a-zA-Z-]*?)\s*$");
    private static Func<string, bool> IsGenre = (line) => GenrePattern.IsMatch(line);
    private static Func<string, string> LineToSingleGenre = (line) =>
    {
      string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
      return parts[parts.Length - 1];
    };
    private static Func<string, Tuple<string, string>> LineToGenre = (line) =>
    {
      string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
      return new Tuple<string, string>(parts[0], parts[parts.Length - 1]);
    };


    private static Dictionary<string, DimGenre> GetGenres()
    {
      int id = 1;
      var genres = new Dictionary<string, DimGenre>();
      IEnumerable<string> DistinctGenres = File.ReadLines(Path.Combine(FolderPath, "genres.list"), Encoding.Default)
        .Where(IsGenre)
        .Select(LineToSingleGenre)
        .Distinct();

      foreach (var genre in DistinctGenres)
      {
        genres.Add(genre, new DimGenre { Id = id++, Name = genre });
      }

      return genres;
    }

    private static Dictionary<string, List<DimGenre>> GetGenresByMovie()
    {
      var g = new Dictionary<string, List<DimGenre>>();
      IEnumerable<Tuple<string, string>> DistinctGenres = File.ReadLines(Path.Combine(FolderPath, "genres.list"), Encoding.Default)
        .Where(IsGenre)
        .Select(LineToGenre);

      foreach (var genre in DistinctGenres)
      {
        if (genres.ContainsKey(genre.Item2))
        {
          if (g.ContainsKey(genre.Item1))
          {
            g[genre.Item1].Add(genres[genre.Item2]);
          }
          else
          {
            g.Add(genre.Item1, new List<DimGenre> { genres[genre.Item2] });
          }
        }
      }

      return g;
    }

    // Countries
    private static Regex CountryPattern = new Regex(@"^(?<title>[^\t]+)\t+(?<country>.*?)\s*$");
    private static Func<string, bool> IsCountry = (line) => CountryPattern.IsMatch(line);
    private static Func<string, string> LineToSingleCountry = (line) =>
    {
      string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
      return parts[parts.Length - 1];
    };
    private static Func<string, Tuple<string, string>> LineToCountry = (line) =>
    {
      string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
      return new Tuple<string, string>(parts[0], parts[parts.Length - 1]);
    };

    private static Dictionary<string, DimCountry> GetCountries()
    {
      int id = 1;
      var countries = new Dictionary<string, DimCountry>();
      IEnumerable<string> DistinctCountries = File.ReadLines(Path.Combine(FolderPath, "countries.list"), Encoding.Default)
        .Where(IsCountry)
        .Select(LineToSingleCountry)
        .Distinct();

      foreach (var country in DistinctCountries)
      {
        countries.Add(country, new DimCountry { Id = id++, Name = country });
      }

      return countries;
    }

    private static Dictionary<string, List<DimCountry>> GetCountriesByMovie()
    {
      var c = new Dictionary<string, List<DimCountry>>();
      IEnumerable<Tuple<string, string>> DistinctCountries = File.ReadLines(Path.Combine(FolderPath, "countries.list"), Encoding.Default)
        .Where(IsCountry)
        .Select(LineToCountry);

      foreach (var country in DistinctCountries)
      {
        if (countries.ContainsKey(country.Item2))
        {
          if (c.ContainsKey(country.Item1))
          {
            c[country.Item1].Add(countries[country.Item2]);
          }
          else
          {
            c.Add(country.Item1, new List<DimCountry> { countries[country.Item2] });
          }
        }
      }

      return c;
    }


    // Actors
    private static Regex ActorPattern = new Regex(@"^(?<actor>[^\t]+)\t+(?<title>.*?\s\([\d\?]{4}.*?\)).*?$");
    private static Func<string, bool> IsActor = (line) => ActorPattern.IsMatch(line);
    private static Func<string, string> LineToActor = (line) => line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries)[0];

    private static Dictionary<Tuple<ActorGender, string>, DimActor> GetActors()
    {
      int id = 1;
      var actors = new Dictionary<Tuple<ActorGender, string>, DimActor>();
      IEnumerable<string> DistinctActors = File.ReadLines(Path.Combine(FolderPath, "actors.list"), Encoding.Default)
        .Where(IsActor)
        .Select(LineToActor)
        .Distinct();

      IEnumerable<string> DistinctActresses = File.ReadLines(Path.Combine(FolderPath, "actresses.list"), Encoding.Default)
        .Where(IsActor)
        .Select(LineToActor)
        .Distinct();

      foreach (var actor in DistinctActors)
      {
        actors.Add(new Tuple<ActorGender, string>(ActorGender.Male, actor), new DimActor { Id = id++, Name = actor, Gender = ActorGender.Male });
      }

      foreach (var actor in DistinctActresses)
      {
        actors.Add(new Tuple<ActorGender, string>(ActorGender.Female, actor), new DimActor { Id = id++, Name = actor, Gender = ActorGender.Female });
      }

      return actors;
    }


    // Directors
    private static Regex DirectorPattern = new Regex(@"^(?<director>[^\t]+)\t+(?<title>.*?\s\([\d\?]{4}.*?\)).*?$");
    private static Func<string, bool> IsDirector = (line) => DirectorPattern.IsMatch(line);
    private static Func<string, string> LineToSingleDirector = (line) => line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries)[0];


    private static Regex MovieDirectorPattern = new Regex(@"^(?<title>[^\t]+)\t+(?<director>.*?)\s*$");
    private static Func<string, bool> IsMovieDirector = (line) => MovieDirectorPattern.IsMatch(line);
    private static Func<string, Tuple<string, string>> LineToDirector = (line) =>
    {
      string[] parts = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
      return new Tuple<string, string>(parts[0], parts[parts.Length - 1]);
    };

    private static Dictionary<string, DimDirector> GetDirectors()
    {
      int id = 1;
      var directors = new Dictionary<string, DimDirector>();
      IEnumerable<string> DistinctDirectors = File.ReadLines(Path.Combine(FolderPath, "directors.list"), Encoding.Default)
        .Where(IsDirector)
        .Select(LineToSingleDirector)
        .Distinct();

      foreach (var director in DistinctDirectors)
      {
        directors.Add(director, new DimDirector { Id = id++, Name = director });
      }

      return directors;
    }

    public static Dictionary<string, List<DimDirector>> GetDirectorsByMovie()
    {
      var d = new Dictionary<string, List<DimDirector>>();
      IEnumerable<Tuple<string, string>> DistinctDirectors = File.ReadLines(Path.Combine(FolderPath, "_fix", "directors.list"), Encoding.Default)
        .Where(IsMovieDirector)
        .Select(LineToDirector);

      foreach (var director in DistinctDirectors)
      {
        if (directors.ContainsKey(director.Item2))
        {
          if (d.ContainsKey(director.Item1))
          {
            d[director.Item1].Add(directors[director.Item2]);
          }
          else
          {
            d.Add(director.Item1, new List<DimDirector> { directors[director.Item2] });
          }
        }
      }

      return d;
    }


    // Movies
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

    private static Dictionary<string, DimMovie> GetMovies()
    {
      int id = 1;
      var movies = new Dictionary<string, DimMovie>();
      IEnumerable<DimMovie> Movies = File.ReadLines(Path.Combine(FolderPath, "movies.list"), Encoding.Default)
        .Where(IsMovie)
        .Select(LineToMovie);

      foreach (var movie in Movies)
      {
        var g = new List<DimGenre>();
        if (genresByMovie.ContainsKey(movie.Title))
          g = genresByMovie[movie.Title];
        
        var c = new List<DimCountry>();
        if (countriesByMovie.ContainsKey(movie.Title))
          c = countriesByMovie[movie.Title];

        var d = new List<DimDirector>();
        if (directorsByMovie.ContainsKey(movie.Title))
          d = directorsByMovie[movie.Title];

        movies.Add(movie.Title, new DimMovie { Id = id++, Title = movie.Title, Year = movie.Year, Countries = c, Genres = g, Directors = d });
      }

      return movies;
    }

    #endregion
  }
}
