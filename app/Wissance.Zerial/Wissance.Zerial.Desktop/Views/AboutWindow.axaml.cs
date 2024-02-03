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
        DataContext = new AboutWindowViewModel(model);
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

    private void OnClick(object? sender, KeyEventArgs e)
    {
        System.Diagnostics.Process.Start(WissanceWebSiteTextBlock.Text);
    }
}