using Avalonia.Controls;
using Avalonia.Interactivity;
using Wissance.Zerial.Desktop.ViewModels;

namespace Wissance.Zerial.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        
        public void OnConnectClick(object sender, RoutedEventArgs e)
        {
            // todo(UMV): think what should we programmatically do with other controls
        }
    }
}