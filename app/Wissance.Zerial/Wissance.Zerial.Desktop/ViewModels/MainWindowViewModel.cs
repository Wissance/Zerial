﻿using System.Collections.Generic;
using System.Linq;
using Avalonia.Interactivity;
using DynamicData.Binding;
using ReactiveUI;
using Wissance.Zerial.Common.Rs232;

namespace Wissance.Zerial.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            SelectedBaudRate = _baudRates.First(b => b.Value == Rs232BaudRate.BaudMode9600).Key;
            SelectedByteLength = _byteLengthBits.First(bl => bl.Value == 8).Key;
            SelectedStopBits = _stopBits.First(sb => sb.Value == Rs232StopBits.One).Key;
            SelectedFlowControl = _flowControls.First(fc => fc.Value == Rs232FlowControl.NoControl).Key;
            SelectedParity = _parities.First(p => p.Value == Rs232Parity.Even).Key;
            XonSymbol = "0x11";
            XoffSymbol = "0x13";
            _isXonXoffEnabled = _flowControls[SelectedFlowControl] == Rs232FlowControl.XonXoff;
        }

        public void ExecuteConnectAction()
        {
            int a = 1;
        }

        #region RS232ConnOptions
        public IList<string> BaudRates => _baudRates.Keys.ToList();
        public string SelectedBaudRate { get; set; }

        public IList<string> StopBits => _stopBits.Keys.ToList();
        
        public string SelectedStopBits { get; set; }

        public IList<string> ByteLength => _byteLengthBits.Keys.ToList();
        
        public string SelectedByteLength { get; set; }

        public IList<string> Parities => _parities.Keys.ToList();
        
        public string SelectedParity { get; set; }

        public IList<string> FlowControls => _flowControls.Keys.ToList();

        public string SelectedFlowControl
        {
            get { return _selectedFlowControl;}
            set
            {
                _selectedFlowControl = value;
                IsProgrammableFlowControl = _flowControls[_selectedFlowControl] == Rs232FlowControl.XonXoff;
                // this DO TRICK with props changes apply on View (2Way Binding)
                this.RaisePropertyChanged("IsProgrammableFlowControl");
            }
        }

        public bool IsProgrammableFlowControl { get; set; }
        
        public bool IsXonXoffEnabled()
        {
            if (string.IsNullOrEmpty(SelectedFlowControl))
                    return false;
            return _flowControls[SelectedFlowControl] == Rs232FlowControl.XonXoff;
        }
        
        public string XonSymbol { get; set; }
        
        public string XoffSymbol { get; set; }
        #endregion

        private string _selectedFlowControl;
        private bool _isXonXoffEnabled;
        
        private readonly IDictionary<string, Rs232BaudRate> _baudRates = new Dictionary<string, Rs232BaudRate>()
        {
            {"9600", Rs232BaudRate.BaudMode9600},
            {"19200", Rs232BaudRate.BaudMode19200},
            {"38400", Rs232BaudRate.BaudMode38400},
            {"57600", Rs232BaudRate.BaudMode57600},
            {"115200", Rs232BaudRate.BaudMode115200}
        };

        private readonly IDictionary<string, Rs232StopBits> _stopBits = new Dictionary<string, Rs232StopBits>()
        {
            {"One", Rs232StopBits.One},
            {"One and half", Rs232StopBits.OneAndHalf},
            {"Two", Rs232StopBits.Two}
        };
        
        private readonly IDictionary<string, int> _byteLengthBits = new Dictionary<string, int>()
        {
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8}
        };

        private readonly IDictionary<string, Rs232Parity> _parities = new Dictionary<string, Rs232Parity>()
        {
            {"No parity", Rs232Parity.NoParity},
            {"Mark", Rs232Parity.Mark},
            {"Space", Rs232Parity.Space},
            {"Even", Rs232Parity.Even},
            {"Odd", Rs232Parity.Odd}
        };

        private readonly IDictionary<string, Rs232FlowControl> _flowControls = new Dictionary<string, Rs232FlowControl>()
        {
            {"No", Rs232FlowControl.NoControl},
            {"RTS/CTS", Rs232FlowControl.RtsCts},
            {"Xon/Xoff", Rs232FlowControl.XonXoff}
        };
    }
}