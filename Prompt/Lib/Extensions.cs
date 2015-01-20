using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;
using System.Text.RegularExpressions;

namespace Prompt.Lib
{
  public static class Extensions
  {
    public static string ToSqlInsert(this DimGenre genre)
    {
      return string.Format(@"INSERT INTO [DimGenres] ([Id], [Name]) VALUES ({0}, N'{1}');",
        genre.Id, genre.Name.ConvertToSqlString());
    }

    public static string ToSqlInsert(this DimCountry country)
    {
      return string.Format(@"INSERT INTO [DimCountries] ([Id], [Name]) VALUES ({0}, N'{1}');",
        country.Id, country.Name.ConvertToSqlString());
    }

    public static string ToSqlInsert(this DimActor actor)
    {
      return string.Format(@"INSERT INTO [DimActors] ([Id], [Name], [Gender]) VALUES ({0}, N'{1}', {2});",
        actor.Id, actor.Name.ConvertToSqlString(), (byte)actor.Gender);
    }

    public static string ToSqlInsert(this DimDirector director)
    {
      return string.Format(@"INSERT INTO [DimDirectors] ([Id], [Name]) VALUES ({0}, N'{1}');",
        director.Id, director.Name.ConvertToSqlString());
    }

    public static string ToSqlInsert(this DimMovie movie)
    {
      return string.Format(@"INSERT INTO [DimMovies] ([Id], [Title], [Year]) VALUES ({0}, N'{1}', {2});",
        movie.Id, movie.Title.ConvertToSqlString(), movie.Year);
    }

    public static string ToCountrySqlInsert(this DimMovie movie)
    {
      var sql = new List<string>();
      foreach (var country in movie.Countries)
      {
        sql.Add(string.Format(@"INSERT INTO [CountryMovies] ([CountryId], [MovieId]) VALUES ({0}, {1});", country.Id, movie.Id));
      }
      return string.Join("", sql.ToArray());
    }

    public static string ToGenreSqlInsert(this DimMovie movie)
    {
      var sql = new List<string>();
      foreach (var genre in movie.Genres)
      {
        sql.Add(string.Format(@"INSERT INTO [GenreMovies] ([GenreId], [MovieId]) VALUES ({0}, {1});", genre.Id, movie.Id));
      }
      return string.Join("", sql.ToArray());
    }

    public static string ToDirectorSqlInsert(this DimMovie movie)
    {
      var sql = new List<string>();
      foreach (var director in movie.Directors)
      {
        sql.Add(string.Format(@"INSERT INTO [DirectorMovies] ([DirectorId], [MovieId]) VALUES ({0}, {1});", director.Id, movie.Id));
      }
      return string.Join("", sql.ToArray());
    }
    
    


    public static string GenerateSlug(this string phrase)
    {
      string str = phrase.ConvertToASCII().ToLower();
      str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
      str = Regex.Replace(str, @"\s+", " ").Trim();
      str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
      str = Regex.Replace(str, @"\s", "-");
      return str;
    }

    public static string ConvertToASCII(this string txt)
    {
      byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
      return System.Text.Encoding.ASCII.GetString(bytes);
    }

    public static string ConvertToUTF8(this string txt)
    {
      byte[] bytes = Encoding.Default.GetBytes(txt);
      return Encoding.UTF8.GetString(bytes);
    }

    private static string ConvertToSqlString(this string txt)
    {
      return txt.Replace("'", "''");
    }
  }
}
