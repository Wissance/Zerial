using System;
using System.Threading;
using Avalonia.Controls;
using Wissance.Zerial.Desktop.Models;
using Wissance.Zerial.Desktop.Views;

namespace Wissance.Zerial.Desktop.ViewModels
{

    public class SplashScreenViewModel : ViewModelBase
    {
        public SplashScreenViewModel(SplashScreenWindow window, AppVersionModel model)
        {
            SecondsToStart = DefaultWait;
            // TODO(UMV): ADD 1sec Timer
            TimerCallback tm = new TimerCallback(CountToStart);
            _timer = new Timer(tm, null, 0, 1000);
        }

        public void CountToStart(object obj)
        {
            SecondsToStart--;
            if (SecondsToStart == 0)
            { 
                //Close this and start MainWindow
                _timer.Change(-1, -1);
                _timer.Dispose();
                _window.Close();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        public string Years
        {
            get
            {
                int currentYear = DateTime.Now.Year;
                return currentYear > YearOfWorksStarted ? $"2023-{currentYear}" : "2023-2024";
            }
        }
        
        
        public int SecondsToStart { get; set; }
            

        private const int YearOfWorksStarted = 2023;
        private const int DefaultWait = 3;

        private readonly Timer _timer;
        private readonly SplashScreenWindow _window;
        private readonly AppVersionModel _model;
    }
}