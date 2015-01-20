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
  public static class ActorHelper
  {
    private static string FolderPath = ConfigurationManager.AppSettings["folder"].ToString();

    public static void Save()
    {
      if (File.Exists(Path.Combine(FolderPath, "_fix", "actors.list")))
      {
        File.Delete(Path.Combine(FolderPath, "_fix", "actors.list"));
      }

      File.WriteAllLines(Path.Combine(FolderPath, "_fix", "actors.list"), ActorsWithTitle(), Encoding.Default);
      File.WriteAllLines(Path.Combine(FolderPath, "_fix", "actresses.list"), ActressesWithTitle(), Encoding.Default);
    }

    private static IEnumerable<string> ActorsWithTitle()
    {
      var actor = string.Empty;
      IEnumerable<string> lines = File.ReadLines(Path.Combine(FolderPath, "actors.list"), Encoding.Default);
      Regex actorName = new Regex(@"^(?<actor>[^\t]+)\t+(?<title>.*?\s\([\d\?]{4}.*?\)).*?$");
      Regex noActorName = new Regex(@"^\t+(?<title>.*?\s\(([\d\?]{4}).*?\)).*?$");

      foreach (var line in lines)
      {
        if (actorName.IsMatch(line))
        {
          var parts = line.Split('\t');
          actor = parts[0];
          yield return parts[parts.Length - 1] + "\t\t\t" + actor;
        }
        else if (noActorName.IsMatch(line))
        {
          yield return line.Trim() + "\t\t\t" + actor;
        }
      }
    }

    private static IEnumerable<string> ActressesWithTitle()
    {
      var actor = string.Empty;
      IEnumerable<string> lines = File.ReadLines(Path.Combine(FolderPath, "actresses.list"), Encoding.Default);
      Regex actorName = new Regex(@"^(?<actor>[^\t]+)\t+(?<title>.*?\s\([\d\?]{4}.*?\)).*?$");
      Regex noActorName = new Regex(@"^\t+(?<title>.*?\s\(([\d\?]{4}).*?\)).*?$");

      foreach (var line in lines)
      {
        if (actorName.IsMatch(line))
        {
          var parts = line.Split('\t');
          actor = parts[0];
          yield return parts[parts.Length - 1] + "\t\t\t" + actor;
        }
        else if (noActorName.IsMatch(line))
        {
          yield return line.Trim() + "\t\t\t" + actor;
        }
      }
    }
  
  }
}
