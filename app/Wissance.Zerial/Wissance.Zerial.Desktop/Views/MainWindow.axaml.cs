using System;
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

        private void OnPortNumberListOpened(object? sender, EventArgs e)
        {
            _context.ReEnumeratePorts();
        }
        
        private readonly MainWindowViewModel _context;
    }
}