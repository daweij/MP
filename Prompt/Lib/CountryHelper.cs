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
  public static class CountryHelper
  {
    private static string FolderPath = ConfigurationManager.AppSettings["folder"].ToString();

    public static void Save()
    {
      if (File.Exists(Path.Combine(FolderPath, "_fix", "countries.list")))
      {
        File.Delete(Path.Combine(FolderPath, "_fix", "countries.list"));
      }

      using (var sw = File.AppendText(Path.Combine(FolderPath, "_fix", "countries.list")))
      {
        foreach (var genre in GenreWithTitle())
        {
          sw.WriteLine(genre);
        }
      }
    }

    private static IEnumerable<string> GenreWithTitle()
    {
      var movie = string.Empty;
      IEnumerable<string> lines = File.ReadLines(Path.Combine(FolderPath, "countries.list"), Encoding.Default);
      Regex pattern = new Regex(@"^([^\s].*?)\t+(.*?)\s*$");

      foreach (var line in lines)
      {
        if (pattern.IsMatch(line))
        {
          yield return line;
        }

      }
    }
  }
}
