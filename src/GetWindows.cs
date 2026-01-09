namespace hyprwatch.Window
{
  using System;
  using System.Text;
  using System.Runtime.InteropServices;
  using System.Diagnostics;

  public partial class GetWindows
  {
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    public static string ActiveWindow()
    {
      string? activeWindow;
      IntPtr hwnd = GetForegroundWindow();

      var title = new StringBuilder(256);
      GetWindowText(hwnd, title, title.Capacity);

      GetWindowThreadProcessId(hwnd, out uint pid);
      var process = Process.GetProcessById((int)pid);

      activeWindow = title.ToString();

      if(activeWindow == null)
      {
        activeWindow = "Home-Screen";
      }

      return activeWindow ?? string.Empty;
    }
  }
}
