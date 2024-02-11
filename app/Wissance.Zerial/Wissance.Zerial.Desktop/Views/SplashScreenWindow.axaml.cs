using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Wissance.Zerial.Desktop.Utils;
using Wissance.Zerial.Desktop.ViewModels;

namespace Wissance.Zerial.Desktop.Views
{
    public partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            _context = new SplashScreenViewModel(this, Globals.CurrentAppVersion);
            DataContext = _context;
            InitializeComponent();
        }

        private void OnWissanceClick(object? sender, PointerPressedEventArgs e)
        {
            LinkNavigator.Navigate(Globals.WissanceWebSiteUrl);
        }

        private void OnMVUClick(object? sender, PointerPressedEventArgs e)
        {
            LinkNavigator.Navigate(Globals.MVUGithubUrl);
        }

        private void OnSupportClick(object? sender, PointerPressedEventArgs e)
        {
            LinkNavigator.Navigate(Globals.SupportUrl);
        }

        private void OnMouseOver(object? sender, PointerEventArgs e)
        {
            Cursor = new Cursor(StandardCursorType.Hand);
        }

        private void OnMouseLeave(object? sender, PointerEventArgs e)
        {
            Cursor = new Cursor(StandardCursorType.Arrow);
        }

        private readonly SplashScreenViewModel _context;
    }
    
}