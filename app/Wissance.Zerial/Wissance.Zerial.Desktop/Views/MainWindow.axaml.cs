using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
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
        
        private void OnPortsPointerControlMouseOver(object? sender, PointerEventArgs e)
        {
            IList<string> ports = _context.ReEnumeratePorts();
            PortsListSelect.ItemsSource = ports as IEnumerable;
            PortsListSelect.SelectedItem = ports.Any() ? ports [0]: null;
        }
        
        private void OnWindowClose(object? sender, WindowClosingEventArgs e)
        {
            _context.ResourcesCleanUp();
        }
        
        private readonly MainWindowViewModel _context;
    }
}