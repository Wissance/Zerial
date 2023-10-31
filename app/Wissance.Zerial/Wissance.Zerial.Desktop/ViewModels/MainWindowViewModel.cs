using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Interactivity;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Wissance.Zerial.Common.Rs232;
using Wissance.Zerial.Common.Rs232.Managers;
using Wissance.Zerial.Common.Rs232.Settings;
using Wissance.Zerial.Common.Rs232.Tools;
using Wissance.Zerial.Desktop.Models;

namespace Wissance.Zerial.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            SerialOptions = new SerialDefaultsModel();
            _ports = new List<string>(Rs232PortsEnumerator.GetAvailablePorts().ToList());
            SelectedPortNumber = Ports.Any() ? Ports.First() : null;
            _deviceManager = new MultiDeviceRs232Manager();
            
            SelectedBaudRate = SerialOptions.BaudRates.First(b => b.Value == Rs232BaudRate.BaudMode9600).Key;
            SelectedByteLength = SerialOptions.ByteLength.First(bl => bl.Value == 8).Key;
            SelectedStopBits = SerialOptions.StopBits.First(sb => sb.Value == Rs232StopBits.One).Key;
            SelectedFlowControl = SerialOptions.FlowControls.First(fc => fc.Value == Rs232FlowControl.NoControl).Key;
            SelectedParity = SerialOptions.Parities.First(p => p.Value == Rs232Parity.Even).Key;
            XonSymbol = SerialDefaultsModel.DefaultXon;
            XoffSymbol = SerialDefaultsModel.DefaultXoff;
            // example ....
            DevicesConfigs = new ObservableCollection<SerialPortNodeModel>()
            {
                 new SerialPortNodeModel(5, "COM5, 115200 b/s, Even, 1 Stop Bit, No Flow Control"),
                 new SerialPortNodeModel(3, "COM3, 9600 b/s, No, 1 Stop Bit, No Flow Control")
            };
        }

        // todo(UMV): this should be a common handler 4 Connect/Disconnect
        public async Task ExecuteConnectAction()
        {
            int portNumber = 0;
            bool portParseResult = int.TryParse(SelectedPortNumber.Substring("COM".Length), out portNumber);
            if (portParseResult)
            {
                Rs232Settings deviceSetting = new Rs232Settings()
                {
                    PortNumber = portNumber,
                    BaudRate = SerialOptions.BaudRates[SelectedBaudRate],
                    ByteLength = SerialOptions.ByteLength[SelectedByteLength],
                    Parity = SerialOptions.Parities[SelectedParity],
                    StopBits = SerialOptions.StopBits[SelectedStopBits],
                    FlowControl = SerialOptions.FlowControls[SelectedFlowControl]
                    // todo(UMV): pass Xon + Xoff bytes
                };
                bool openResult = await _deviceManager.OpenAsync(deviceSetting);
                if (openResult)
                {
                    // todo(UMV): add to tree
                }
            }
        }

        public IList<string> ReEnumeratePorts()
        {
            Ports.Clear();
            IList<string> newPorts = Rs232PortsEnumerator.GetAvailablePorts();
            Ports = newPorts;
            return newPorts;
        }

        public void ResourcesCleanUp()
        {
            _deviceManager.Dispose();
        }

        #region RS232TreeConfiguration
        public ObservableCollection<SerialPortNodeModel> DevicesConfigs { get; set; }
        
        #endregion

        #region SerialDeviceSettings
        
        public IList<string> Ports
        {
            get { return _ports; }
            set
            {
                _ports = value;
                this.RaisePropertyChanged();
            }
        }
        
        public SerialDefaultsModel SerialOptions { get; }

        public string SelectedPortNumber
        {
            get { return _selectedPortName;}
            set
            {
                _selectedPortName = value;
                IsPortSelected = !string.IsNullOrEmpty(_selectedPortName);
                this.RaisePropertyChanged(nameof(IsPortSelected));
            }
        }
        public string SelectedBaudRate { get; set; }
        public string SelectedStopBits { get; set; }
        public string SelectedByteLength { get; set; }
        public string SelectedParity { get; set; }

        public string SelectedFlowControl
        {
            get { return _selectedFlowControl;}
            set
            {
                _selectedFlowControl = value;
                IsProgrammableFlowControl = SerialOptions.FlowControls[_selectedFlowControl] == Rs232FlowControl.XonXoff;
                // this DO TRICK with props changes apply on View (2Way Binding)
                this.RaisePropertyChanged(nameof(IsProgrammableFlowControl));
            }
        }

        public bool IsProgrammableFlowControl { get; set; }
        public bool IsPortSelected { get; set; }

        public string XonSymbol { get; set; }
        public string XoffSymbol { get; set; }
        
        private string _selectedFlowControl;
        private string _selectedPortName;
        #endregion
        
        private IList<string> _ports;
        private readonly IRs232DeviceManager _deviceManager;
    }
}