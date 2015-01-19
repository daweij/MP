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
  public static class BusinessHelper
  {
    private static string FolderPath = ConfigurationManager.AppSettings["folder"].ToString();

    public static void Save()
    {
      if (File.Exists(Path.Combine(FolderPath, "_fix", "business.list")))
      {
        File.Delete(Path.Combine(FolderPath, "_fix", "business.list"));
      }

      using (var sw = File.AppendText(Path.Combine(FolderPath, "_fix", "business.list")))
      {
        foreach (var business in BusinessWithTitle())
        {
          sw.WriteLine(business);
        }
      }
    }

    private static IEnumerable<string> BusinessWithTitle()
    {
      var movie = string.Empty;
      IEnumerable<string> lines = File.ReadLines(Path.Combine(FolderPath, "business.list"), Encoding.Default);
      Regex moviePattern = new Regex(@"^MV:\s(?<title>.*?)$");
      Regex grossPattern = new Regex(@"^GR:\sUSD\s(?<sum>[0-9,]+)\s(?<type>\(worldwide\))\s*$", RegexOptions.IgnoreCase);

      foreach (var line in lines)
      {
        if (moviePattern.IsMatch(line))
        {
          movie = line.Split('\t')[0].Replace("MV: ", "");
        }
        else if (grossPattern.IsMatch(line) && !string.IsNullOrEmpty(movie))
        {
          var match = grossPattern.Match(line);
          yield return movie + "\t\t\t" + match.Groups["sum"].Value + "\t\t\t" + match.Groups["type"].Value;
        }
      }
    }
  }
}
