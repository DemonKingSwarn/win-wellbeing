using hyprwatch.Logger;
using hyprwatch.Report.Weekly;

class Program
{
  static void Main(string[] args)
  {
    string version = "0.0.8";
    string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    string hyprwatchDirectory = Path.Combine(homeDirectory, ".cache", "hyprwatch");
    string dailyDataDirectory = Path.Combine(hyprwatchDirectory, "daily_data");
    Directory.CreateDirectory(dailyDataDirectory);

    if (args.Length == 0 || args[0] != "-d" && args[0] != "--show" && args[0] != "-v")
    {
      Console.WriteLine("Usage: -d || -v || --show");
      return;
    }

    if(args[0] == "-v")
    {
      Console.WriteLine($"win-wellbeing v{version}");
    }

    if(args[0] == "-d")
    {
        while(true)
        {
          WatchLog.LogCreation();
          System.Threading.Thread.Sleep(5000);
        }
    }

    if(args[0] == "--show")
    {
      Dictionary<string, string> data = new Dictionary<string, string>();

      var date = WatchLog.GetDate(); 
      string filePath = $"{homeDirectory}/.cache/hyprwatch/daily_data/{date}.csv";

      var rawData = File.ReadAllLines(filePath);

      foreach(var line in rawData)
      {
        var parts = line.Split('\t');
        if(parts.Length >= 2)
        {
          string key = parts[1].TrimEnd();
          string value = parts[0];
          data[key] = value;
        }
      }
      
      TimeSpan totalTime = TimeSpan.Zero;

      foreach (var entry in data)
      {
        if (TimeSpan.TryParse(entry.Value, out TimeSpan time))
        {
          totalTime += time;
        }
      }
      
      Console.WriteLine(" ");
      Console.WriteLine($"Today's Screen Usage\t{totalTime}");
      WriteLineColored(new string('-', 60), ConsoleColor.Red);
      WriteColored(string.Format("{0,-30}{1,30}", "App", "Time"), ConsoleColor.Yellow);
      Console.WriteLine();
      WriteLineColored(new string('-', 60), ConsoleColor.Red);
      
      foreach (var entry in data)
      {
        string key = entry.Key.Length > 30 ? entry.Key.Substring(0, 27) + "..." : entry.Key;
        WriteColored($"{key, -30}", ConsoleColor.Blue);
        WriteLineColored($"{entry.Value, 30}", ConsoleColor.Green);
      }
      
    }
    /*if(args[0] == "--weekly")
    {
      WeeklyReport.DisplayWeeklyReport();
    }*/
  }

  static void WriteColored(string text, ConsoleColor color)
  {
      var prev = Console.ForegroundColor;
      Console.ForegroundColor = color;
      Console.Write(text);
      Console.ForegroundColor = prev;
  }

  static void WriteLineColored(string text, ConsoleColor color)
  {
    WriteColored(text + Environment.NewLine, color);
  }

}
