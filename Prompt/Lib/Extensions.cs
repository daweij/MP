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
      return string.Format(@"INSERT INTO [DimActors] ([Id], [Name]) VALUES ({0}, N'{1}');",
        actor.Id, actor.Name.ConvertToSqlString());
    }

    public static string ToSqlInsert(this DimDirector director)
    {
      return string.Format(@"INSERT INTO [DimDirectors] ([Id], [Name]) VALUES ({0}, N'{1}');",
        director.Id, director.Name.ConvertToSqlString());
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
