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
  public static class DirectorHelper
  {
    private static string FolderPath = ConfigurationManager.AppSettings["folder"].ToString();

    public static void Save()
    {
      if (File.Exists(Path.Combine(FolderPath, "_fix", "directors.list")))
      {
        File.Delete(Path.Combine(FolderPath, "_fix", "directors.list"));
      }

      File.WriteAllLines(Path.Combine(FolderPath, "_fix", "directors.list"), DirectorsWithTitle(), Encoding.Default);
    }

    private static IEnumerable<string> DirectorsWithTitle()
    {
      var director = string.Empty;
      IEnumerable<string> lines = File.ReadLines(Path.Combine(FolderPath, "directors.list"), Encoding.Default);
      Regex directorName = new Regex(@"^(?<director>[^\t]+)\t+(?<title>.*?\s\([\d\?]{4}.*?\)).*?$");
      Regex noDirectorName = new Regex(@"^\t+(?<title>.*?\s\(([\d\?]{4}).*?\)).*?$");

      foreach (var line in lines)
      {
        if (directorName.IsMatch(line))
        {
          var parts = line.Split('\t');
          director = parts[0];
          yield return parts[parts.Length - 1] + "\t\t\t" + director;
        }
        else if (noDirectorName.IsMatch(line))
        {
          yield return line.Trim() + "\t\t\t" + director;
        }
      }
    }
  }
}
