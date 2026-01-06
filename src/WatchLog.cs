namespace hyprwatch.Logger
{
  using System;
  using System.IO;
  using System.Threading;
  using System.Diagnostics;
  using System.Collections.Generic;
  using Newtonsoft.Json;
  using hyprwatch.Window;
  using hyprwatch.Time;

  public class WatchLog
  {
    public static string GetTime()
    {
      string? t = null;

      string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

      t = DateTime.Now.ToString("HH:mm:ss");

      return t ?? string.Empty;
    }

    public static string GetDate()
    {
      string? d = null;

      string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

      d = DateTime.Now.ToString("dd-MM-yyyy");

      return d ?? string.Empty;
    }

    static void UpdateCSV(string date, Dictionary<string, string> data)
    {
      string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      string filePath = Path.Combine(homeDir, ".cache", "hyprwatch", "daily_data", $"{date}.csv");

      string? dirPath = Path.GetDirectoryName(filePath);
      if(dirPath is not null)
      {
        Directory.CreateDirectory(dirPath);
      }
      else
      {
        throw new InvalidOperationException("Invalid file path.");
      }

      var overwriteData = new List<string[]>();
      foreach(var kvp in data)
      {
        overwriteData.Add(new[] { kvp.Value, kvp.Key });
      }

      using (var writer = new StreamWriter(filePath))
      {
        foreach (var row in overwriteData)
        {
          writer.WriteLine(string.Join("\t", row));
        }
      }
    }

    static Dictionary<string, string> ImportData(string file)
    {
      var data = new Dictionary<string, string>();

      var rawData = File.ReadAllLines(file);

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

      return data;
    }

    public static void LogCreation()
    {
      string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      string currentDate = GetDate();
      string filename = Path.Combine($"{homeDir}", ".cache", "hyprwatch", "daily_data", $"{currentDate}.csv");
      if(!File.Exists(filename))
      {
        string directoryPath = Path.Combine($"{homeDir}", ".cache", "hyprwatch", "daily_data");

        Directory.CreateDirectory(directoryPath);

        using (var fp = File.Create(filename))
        {
          // The using block ensures the file is created and closed properly
        }
      }

        var data = ImportData(filename);
        data ??= new Dictionary<string, string>();
        
        while(true)
        {
          string newDate = GetDate();
          if(newDate != currentDate)
          {
            currentDate = newDate;
            filename = Path.Combine($"{homeDir}", ".cache", "hyprwatch", "daily_data", $"{currentDate}.csv");
            data.Clear();

            using (var fp = File.Create(filename))
            {
              // The using block ensures the file is created and closed properly
            }
          }

          //Console.WriteLine(data);

          string activeWindow = GetWindows.ActiveWindow();
          Console.WriteLine(activeWindow);
          string usage = data.TryGetValue(activeWindow, out string? value) ? value : "00:00:00";

          Thread.Sleep(1000);

          usage = TimeOperations.TimeAddition("00:00:01", usage);
          data[activeWindow] = usage;

          /*if(File.Exists(filename))
          {
            UpdateCSV(GetDate(), data);
          }
          else if(!File.Exists(filename))
          {
            string newFilename = Path.Combine($"{homeDir}", ".cache", "hyprwatch", "daily_data", $"{GetDate()}.csv");
            using (var fp = File.Create(newFilename))
            {
                // The using block ensures the file is created and closed properly
            }

            data.Clear();
          

          }*/
          
          UpdateCSV(currentDate, data);
      }
    }
  }
}
