using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using Wissance.Zerial.Desktop.Models;
using Wissance.Zerial.Desktop.Views;

namespace Wissance.Zerial.Desktop.ViewModels
{

    public class SplashScreenViewModel : ViewModelBase
    {
        public SplashScreenViewModel(SplashScreenWindow window, AppVersionModel model)
        {
            _window = window;
            Model = model;
            SecondsToStart = DefaultWait;
            // TODO(UMV): ADD 1sec Timer
            TimerCallback tm = new TimerCallback(CountToStart);
            _timer = new Timer(tm, null, 0, 1000);
        }

        public void CountToStart(object obj)
        {
            SecondsToStart--;
            //Dispatcher.UIThread.Post(() =>
            //{
                this.RaisePropertyChanged(nameof(SecondsToStart));
            //});
            
            if (SecondsToStart == 0)
            { 
                //Close this and start MainWindow
                _timer.Change(-1, -1);
                _timer.Dispose();
                Dispatcher.UIThread.Post(() =>
                {
                    _window.Hide();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    mainWindow.Closed += (sender, args) =>
                    {
                        _window.Close();
                    };
                });
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
        
        public AppVersionModel Model { get; set; }
        
        public int SecondsToStart { get; set; }
        
        private const int YearOfWorksStarted = 2023;
        private const int DefaultWait = 6; // this time start parallel to Window drawing

        private readonly Timer _timer;
        private readonly SplashScreenWindow _window;
        
    }
}