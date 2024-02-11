using System;

namespace Wissance.Zerial.Desktop.ViewModels
{

    public class SplashScreenViewModel : ViewModelBase
    {
        public string Years
        {
            get
            {
                int currentYear = DateTime.Now.Year;
                return currentYear > YearOfWorksStarted ? $"2023-{currentYear}" : "2023-2024";
            }
        }

        private const int YearOfWorksStarted = 2023;
    }
}