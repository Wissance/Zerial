using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Wissance.Zerial.Desktop.Models;
using Wissance.Zerial.Desktop.ViewModels;

namespace Wissance.Zerial.Desktop.Views;

public partial class AboutWindow : Window
{
    public AboutWindow(AppVersionModel model)
        :this()
    {
        _context = new AboutWindowViewModel(model);
        DataContext = _context;
    }
    
    public AboutWindow()
    {
        InitializeComponent();
    }

    private void OnMouseOver(object? sender, PointerEventArgs e)
    {
        Cursor = new Cursor(StandardCursorType.Hand);
    }

    private void OnMouseLeave(object? sender, PointerEventArgs e)
    {
        Cursor = new Cursor(StandardCursorType.Arrow);
    }
    
    private void  OnClick(object? sender, PointerPressedEventArgs e)
    {
        try
        {
            string url = _context.Model.CompanyWebSite; // Unfortunately can't get text in Underline within a TextBlock
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = url;
            Process.Start(psi);
        }
        catch (Exception exception)
        {
        }
    }

    private AboutWindowViewModel _context;
}