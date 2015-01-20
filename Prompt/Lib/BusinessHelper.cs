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

      File.WriteAllLines(Path.Combine(FolderPath, "_fix", "business.list"), BusinessWithTitle(), Encoding.Default);
    }

    private static IEnumerable<string> BusinessWithTitle()
    {
      var movie = string.Empty;
      IEnumerable<string> lines = File.ReadLines(Path.Combine(FolderPath, "business.list"), Encoding.Default);
      Regex moviePattern = new Regex(@"^MV:\s(?<title>.*?)$");
      Regex grossPattern = new Regex(@"^GR:\sUSD\s(?<sum>[0-9,]+)\s\((?<type>worldwide|usa|non\-usa)\)\s*$", RegexOptions.IgnoreCase);
      Regex budgetPattern = new Regex(@"^BT:\sUSD\s(?<sum>[0-9,]+)\s*$", RegexOptions.IgnoreCase);

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
        else if (budgetPattern.IsMatch(line) && !string.IsNullOrEmpty(movie))
        {
          var match = budgetPattern.Match(line);
          yield return movie + "\t\t\t" + match.Groups["sum"].Value + "\t\t\tBT";
        }
      }
    }
  }
}
