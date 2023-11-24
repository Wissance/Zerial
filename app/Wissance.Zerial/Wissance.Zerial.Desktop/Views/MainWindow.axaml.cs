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
        
        private void OnInputSymbolKeyDown(object? sender, KeyEventArgs e)
        {
            // e.Key only 0-9 && A-F
            // int keyCode = (int)e.Key;
            if ((e.Key >= Key.A && e.Key <= Key.F) || (e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                if (_inputNibbleCounter % 2 == 1 /*&&  _inputNibbleCounter > 1*/)
                {
                    // get last and replace last with string 0x{Prev}{Current}
                    string prevNibble = _inputMessage[^1].ToString();
                    string currentNibble = _keyStrings[e.Key];
                    string byteRepresentation = $" 0x{prevNibble}{currentNibble}";
                    _inputMessage = _inputMessage.Remove(_inputMessage.Length - 1, 1);
                    _inputMessage.Append(byteRepresentation);
                }
                else
                {
                    _inputMessage.Append(_keyStrings[e.Key]);
                }

                _inputNibbleCounter++;
            }
            // we should replace content in OnKeyUp or OnTextChanged ...
        }
        
        
        private void OnInputSymbolKeyUp(object? sender, KeyEventArgs e)
        {
            const int symbolsPerByte = 5;
            if (e.Key == Key.Back)
            {
                if (_inputNibbleCounter >= 1)
                {
                    if (_inputNibbleCounter % 2 == 0)
                    {
                        string byteRepresentation = _inputMessage.ToString().Substring(_inputMessage.Length - symbolsPerByte, symbolsPerByte);
                        _inputMessage = _inputMessage.Remove(_inputMessage.Length - symbolsPerByte, symbolsPerByte);
                        _inputMessage.Append(byteRepresentation[symbolsPerByte - 2]);
                        _inputNibbleCounter--;
                    }
                    else
                    {
                        _inputMessage = _inputMessage.Remove(_inputMessage.Length - 1, 1);
                        _inputNibbleCounter--;
                    }
                }
            }
            UpdateInputMessageText();
        }
        
        
        private void OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            UpdateInputMessageText();
        }

        private void UpdateInputMessageText()
        {
            // update text if there are differences
            if (!string.Equals(SerialDeviceSendMessageTextBox.Text, _inputMessage.ToString()))
            {
                SerialDeviceSendMessageTextBox.Text = _inputMessage.ToString();
                SerialDeviceSendMessageTextBox.CaretIndex = _inputMessage.Length;
            }
        }
        
        private void OnSendMessageClick(object? sender, RoutedEventArgs e)
        {
            _inputMessage.Clear();
            _inputNibbleCounter = 0;
            // UpdateInputMessageText();
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

        private readonly IDictionary<Key, string> _keyStrings = new Dictionary<Key, string>()
        {
            {Key.D0, "0"},
            {Key.D1, "1"},
            {Key.D2, "2"},
            {Key.D3, "3"},
            {Key.D4, "4"},
            {Key.D5, "5"},
            {Key.D6, "6"},
            {Key.D7, "7"},
            {Key.D8, "8"},
            {Key.D9, "9"},
            
            {Key.A, "A"},
            {Key.B, "B"},
            {Key.C, "C"},
            {Key.D, "D"},
            {Key.E, "E"},
            {Key.F, "F"}
        };
        
        private readonly MainWindowViewModel _context;
        private TextEditor _textEditor; 
        private int _inputNibbleCounter = 0;
        private StringBuilder _inputMessage = new StringBuilder();
    }
}