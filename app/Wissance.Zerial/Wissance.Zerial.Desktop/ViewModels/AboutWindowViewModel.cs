using Wissance.Zerial.Desktop.Models;

namespace Wissance.Zerial.Desktop.ViewModels
{
    public class AboutWindowViewModel : ViewModelBase
    {
        public AboutWindowViewModel(AppVersionModel model)
        {
            Model = model;
        }
        
        public AppVersionModel Model { get; set; }
    }
}