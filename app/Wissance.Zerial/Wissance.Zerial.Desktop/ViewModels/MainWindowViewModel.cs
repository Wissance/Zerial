using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Collections;
using Avalonia.Interactivity;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Wissance.Zerial.Common.Rs232;
using Wissance.Zerial.Common.Tools;
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

        public void ExecuteConnectAction()
        {
            int a = 1;
        }

        public IList<string> ReEnumeratePorts()
        {
            Ports.Clear();
            IList<string> newPorts = Rs232PortsEnumerator.GetAvailablePorts();
            Ports = newPorts;
            return newPorts;
        }

        #region RS232TreeConfiguration
        public ObservableCollection<SerialPortNodeModel> DevicesConfigs { get; set; }
        
        #endregion

        #region SerialConnectSettingsOptions
        
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
                this.RaisePropertyChanged("SelectedPortNumber");
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
                this.RaisePropertyChanged("IsProgrammableFlowControl");
            }
        }

        public bool IsProgrammableFlowControl { get; set; }
        
        public string XonSymbol { get; set; }
        
        public string XoffSymbol { get; set; }
        #endregion

        private string _selectedFlowControl;
        private string _selectedPortName;
        
        private IList<string> _ports;
    }
}