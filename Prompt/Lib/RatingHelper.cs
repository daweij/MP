using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prompt.Lib
{
  public static class RatingHelper
  {
    private static string FolderPath = ConfigurationManager.AppSettings["folder"].ToString();

    public static void Save()
    {
      if (File.Exists(Path.Combine(FolderPath, "_fix", "ratings.list")))
      {
        File.Delete(Path.Combine(FolderPath, "_fix", "ratings.list"));
      }

      using (var sw = File.AppendText(Path.Combine(FolderPath, "_fix", "ratings.list")))
      {
        foreach (var rating in RatingWithTitle())
        {
          sw.WriteLine(rating);
        }
      }
    }

    private static IEnumerable<string> RatingWithTitle()
    {
      IEnumerable<string> lines = File.ReadLines(Path.Combine(FolderPath, "ratings.list"), Encoding.Default);
      Regex pattern = new Regex(@"^\s+([\d\.]+)\s+(?<votes>[\d]+)\s+(?<rating>[\d\.]+)\s+(?<title>.*?)$");

      foreach (var line in lines)
      {
        if (pattern.IsMatch(line))
        {
          var match = pattern.Match(line);
          yield return match.Groups["title"].Value + "\t\t\t" + match.Groups["rating"].Value + "\t\t\t" + match.Groups["votes"].Value;
        }
      }
    }
  }
}
