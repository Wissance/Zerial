using System;

namespace Wissance.Zerial.Desktop.ViewModels
{

    public class SplashScreenViewModel : ViewModelBase
    {
        public SplashScreenViewModel()
        {
            SecondsToStart = DefaultWait;
            // TODO(UMV): ADD 1sec Timer
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
    }
}