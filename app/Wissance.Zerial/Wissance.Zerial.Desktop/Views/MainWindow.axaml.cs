using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using Wissance.Zerial.Common.Utils;
using Wissance.Zerial.Desktop.ViewModels;

namespace Wissance.Zerial.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            _context = new MainWindowViewModel();
            DataContext = _context;
            _context.SerialDeviceMessages.CollectionChanged += (sender, args) =>
            {
                StringBuilder builder = new StringBuilder();
                foreach (string message in _context.SerialDeviceMessages)
                {
                    builder.Append(message);
                    builder.Append(Environment.NewLine);
                }
                _textEditor.Document = new TextDocument(builder.ToString());
            };
            InitializeComponent();
            InitializeTextEditor();
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
        private void OnTreeItemTapped(object? sender, TappedEventArgs e)
        {
            // sender is a TextBlock with Text like = COM4, 9600 b/s, 8b, 1 Sb, Even, No FC
            // todo(UMV): Load Setting from tapped item to the controls
            TextBlock treeLineText = sender as TextBlock;
            if (treeLineText != null)
            {
                int portNumber = GetPortNumberFromTreeItemText(treeLineText.Text);
                _context.ShowSelectedSerialDeviceSetting(portNumber);
            }
        }

        private int GetPortNumberFromTreeItemText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return -1;
            string[] items = text.Split(",");
            if (items == null || !items.Any())
                return -2;
            return SerialPortNumberExtractor.Extract(items[0]);
        }

        private void InitializeTextEditor()
        {
            _textEditor = this.FindControl<TextEditor>(TextEditorName);
            _textEditor.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
            _textEditor.Background = Brushes.Transparent;
            _textEditor.ShowLineNumbers = true;
            _textEditor.TextArea.Background = this.Background;
            //_textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;
            //_textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            _textEditor.Options.ShowBoxForControlCharacters = true;
            _textEditor.Options.ColumnRulerPositions = new List<int>() { 80, 100 };
            //_textEditor.TextArea.IndentationStrategy = new Indentation.CSharp.CSharpIndentationStrategy(_textEditor.Options);
            //_textEditor.TextArea.Caret.PositionChanged += Caret_PositionChanged;
            _textEditor.TextArea.RightClickMovesCaret = true;
            _textEditor.Document = new TextDocument("Application started!");
        }

        private const string TextEditorName = "SerialDevicesMessageViewer";

        private readonly MainWindowViewModel _context;
        private TextEditor _textEditor;
    }
}