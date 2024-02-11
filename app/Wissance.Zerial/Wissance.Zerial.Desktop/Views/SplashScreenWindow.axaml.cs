using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Wissance.Zerial.Desktop.Utils;

namespace Wissance.Zerial.Desktop.Views;

public partial class SplashScreenWindow : Window
{
    public SplashScreenWindow()
    {
        InitializeComponent();
    }
    
    private void OnWissanceClick(object? sender, PointerPressedEventArgs e)
    {
        LinkNavigator.Navigate(WissanceWebSiteUrl);
    }
    
    private void OnMVUClick(object? sender, PointerPressedEventArgs e)
    {
        LinkNavigator.Navigate(MVUGithubUrl);
    }
    
    private void OnSupportClick(object? sender, PointerPressedEventArgs e)
    {
        LinkNavigator.Navigate(SupportUrl);
    }
    
    private void OnMouseOver(object? sender, PointerEventArgs e)
    {
        Cursor = new Cursor(StandardCursorType.Hand);
    }

    private void OnMouseLeave(object? sender, PointerEventArgs e)
    {
        Cursor = new Cursor(StandardCursorType.Arrow);
    }

    private const string WissanceWebSiteUrl = "https://wissance.com/en"; //todo(use from model)
    private const string MVUGithubUrl = "https://github.com/EvilLord666";
    private const string SupportUrl = "https://github.com/Wissance/Zerial/blob/master/Support.md";
}