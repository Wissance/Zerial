using System;
using System.Diagnostics;

namespace Wissance.Zerial.Desktop.Utils
{
    public static class LinkNavigator
    {
        public static void Navigate(string url)
        {
            try
            { 
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.FileName = url;
                Process.Start(psi);
            }
            catch (Exception exception)
            {
            }
        }
    }
}