using Avalonia.Controls;
using Avalonia.Interactivity;
using DynamicData.Binding;
using Wissance.Zerial.Desktop.ViewModels;

namespace Wissance.Zerial.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _context = new MainWindowViewModel();
            DataContext = _context;
        }
        
        public void OnConnectClick(object sender, RoutedEventArgs e)
        {
            // todo(UMV): think what should we programmatically do with other controls
        }

        private void OnFlowControlSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            bool isProgrammable = _context.IsXonXoffEnabled();
            XonTextBox.IsEnabled = isProgrammable;
            XoffTextBox.IsEnabled = isProgrammable;
        }

        private readonly MainWindowViewModel _context;
    }
}