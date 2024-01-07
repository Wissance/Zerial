using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Interactivity;
using Avalonia.Threading;
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
            _deviceManager = new MultiDeviceRs232Manager(OnSerialDeviceDataReceived);
            _serialDevices = new List<SerialDeviceModel>();
            
            SelectedBaudRate = SerialOptions.BaudRates.First(b => b.Value == Rs232BaudRate.BaudMode9600).Key;
            SelectedByteLength = SerialOptions.ByteLength.First(bl => bl.Value == 8).Key;
            SelectedStopBits = SerialOptions.StopBits.First(sb => sb.Value == Rs232StopBits.One).Key;
            SelectedFlowControl = SerialOptions.FlowControls.First(fc => fc.Value == Rs232FlowControl.NoControl).Key;
            SelectedParity = SerialOptions.Parities.First(p => p.Value == Rs232Parity.Even).Key;
            XonSymbol = SerialDefaultsModel.DefaultXon;
            XoffSymbol = SerialDefaultsModel.DefaultXoff;

            ConnectButtonText = Globals.ConnectButtonConnectText;
            DevicesConfigs = new ObservableCollection<SerialPortShortInfoModel>()
            {
                 new SerialPortShortInfoModel(false, 5, "COM5, 115200 b/s, Even, 1 SB, No FC"),
                 new SerialPortShortInfoModel(true, 3, "COM3, 9600 b/s, No, 1 SB, No FC")
            };

            SerialDeviceMessages = new ObservableCollection<string>();
            // StatusBar
            Rs232SelectedDeviceStatus = string.Format(Rs232SelectedDeviceStatusStatusBarMessageTemplate, "Application started");
            Rs232SelectedDevicePort = string.Format(Rs232SelectedPortStatusBarMessageTemplate, string.Empty);
            Rs232SelectedDeviceBytesReceived = string.Format(SerialDeviceModel.BytesReceivedTemplate, 0);
            Rs232SelectedDeviceBytesSent = string.Format(SerialDeviceModel.BytesSentTemplate, 0);
        }

        #region HardwareOperationsWithSerialDevices

        public async Task ExecuteConnectActionAsync()
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
                    // todo(UMV): pass Xon + Xoff bytes as bytes
                };

                SerialDeviceModel serialDevice = _serialDevices.FirstOrDefault(s => s.Settings.PortNumber == deviceSetting.PortNumber);
                bool isNewDevice = serialDevice == null;
                if (serialDevice == null)
                {
                    serialDevice = new SerialDeviceModel(false, deviceSetting, new List<SerialDeviceMessageModel>() { });
                }
                else
                {
                    serialDevice.Settings = deviceSetting;
                }

                if (!serialDevice.Connected)
                {
                    bool openResult = await _deviceManager.OpenAsync(deviceSetting);
                    serialDevice.Connected = openResult;
                    if (openResult)
                    {
                        ConnectButtonText = Globals.ConnectButtonDisconnectText;
                        SerialDeviceMessageModel msg = new SerialDeviceMessageModel(MessageType.Connect, DateTime.Now, null);
                        serialDevice.Messages.Add(msg);
                        SerialDeviceMessages.Add(msg.ToString(serialDevice.Settings.PortNumber));
                    }
                }
                else
                {
                    await _deviceManager.CloseAsync(deviceSetting.PortNumber);
                    serialDevice.Connected = false;
                    ConnectButtonText = Globals.ConnectButtonConnectText;
                    SerialDeviceMessageModel msg =  new SerialDeviceMessageModel(MessageType.Disconnect, DateTime.Now, null);
                    serialDevice.Messages.Add(msg);
                    SerialDeviceMessages.Add(msg.ToString(serialDevice.Settings.PortNumber));
                }
                
                if (isNewDevice)
                    _serialDevices.Add(serialDevice);
                this.RaisePropertyChanged(nameof(ConnectButtonText));
                // todo(UMV): tree add
                if (DevicesConfigs.All(d => d.PortNumber != deviceSetting.PortNumber))
                {
                    DevicesConfigs.Add(serialDevice.ToShortInfo());
                    this.RaisePropertyChanged(nameof(DevicesConfigs));
                }
                else
                {
                    SerialPortShortInfoModel info = DevicesConfigs.FirstOrDefault(d => d.PortNumber == deviceSetting.PortNumber);
                    if (info != null)
                    {
                        int index = DevicesConfigs.IndexOf(info);
                        DevicesConfigs.RemoveAt(index);
                        info.Connected = serialDevice.Connected;
                        info.Configuration = serialDevice.ToShortInfo().Configuration;
                        DevicesConfigs.Insert(index, info);
                        this.RaisePropertyChanged(nameof(DevicesConfigs));
                    }
                }
                
                UpdateStatusbar(serialDevice);
            }
        }

        public async Task ExecuteMessageSendAsync()
        {
            int portNumber = 0;
            bool portParseResult = int.TryParse(SelectedPortNumber.Substring("COM".Length), out portNumber);
            if (portParseResult)
            {
                // 1. Get actual serial device
                SerialDeviceModel serialDevice = _serialDevices.FirstOrDefault(s => s.Settings.PortNumber == portNumber);
                if (serialDevice == null)
                    return;
                // 2. Get text
                string[] bytesStr = SerialDeviceMessageToSend.Split(" ").Where(p => !string.IsNullOrEmpty(p)).ToArray();
                IList<byte> bytes = new List<byte>();
                // 3. Split it
                IFormatProvider provider = new NumberFormatInfo();
                foreach (string byteStr in bytesStr)
                {
                    //byteStr is a 0x{Upper}{Lower}
                    string rawByte = byteStr.Substring(2);
                    byte raw;
                    bool res = byte.TryParse(rawByte, NumberStyles.HexNumber, provider, out raw);
                    if (!res)
                    {
                        // log here
                        SerialDeviceMessageModel msg = new SerialDeviceMessageModel(MessageType.Special, DateTime.Now, null,
                            "Unable to send data to COM device, due to it can't be converted into bytes");
                        serialDevice.Messages.Add(msg);
                        SerialDeviceMessages.Add(msg.ToString(serialDevice.Settings.PortNumber));
                        return;
                    }

                    bytes.Add(raw);
                }
                // 4 Send
                bool sendResult = await _deviceManager.WriteAsync(portNumber, bytes.ToArray());
                if (sendResult)
                {
                    SerialDeviceMessageModel msg = new SerialDeviceMessageModel(MessageType.Write, DateTime.Now, bytes.ToArray());
                    serialDevice.Messages.Add(msg);
                    SerialDeviceMessages.Add(msg.ToString(serialDevice.Settings.PortNumber));
                }
                else
                {
                    SerialDeviceMessageModel msg = new SerialDeviceMessageModel(MessageType.Special, DateTime.Now, null,
                        "Data wasn't send to device");
                    serialDevice.Messages.Add(msg);
                    SerialDeviceMessages.Add(msg.ToString(serialDevice.Settings.PortNumber));
                    return;
                }

                // todo(umv): append logs either to serial device and to TextEditor
                SerialDeviceMessageToSend = "";
                this.RaisePropertyChanged(nameof(SerialDeviceMessageToSend));
                UpdateStatusbar(serialDevice);
            }
        }

        private void OnSerialDeviceDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                // find a proper COM port number, probably convert sender to SerialPort
                SerialPort p = sender as SerialPort;
                if (p != null)
                {
                    int portNumber;
                    bool portParseResult = int.TryParse(p.PortName.Substring("COM".Length), out portNumber);
                    if (portParseResult)
                    {
                        Task<byte[]> readResTask = _deviceManager.ReadAsync(portNumber);
                        readResTask.Wait();
                        byte[] receivedData = readResTask.Result;
                        SerialDeviceModel serialDevice = _serialDevices.FirstOrDefault(s => s.Settings.PortNumber == portNumber);
                        SerialDeviceMessageModel msg = new SerialDeviceMessageModel(MessageType.Read, DateTime.Now, receivedData);
                        serialDevice.Messages.Add(msg);
                        string boxMsg = msg.ToString(serialDevice.Settings.PortNumber);
                        _ = Task.Run(() =>
                        {
                            UpdateMessagesFromAnotherThread(boxMsg);
                            UpdateStatusbar(serialDevice);
                        });
                    }
                }
            }
        }

        private async void UpdateMessagesFromAnotherThread(string msg)
        {
            Dispatcher.UIThread.Post(() =>SerialDeviceMessages.Add(msg));
        }

        public IList<string> ReEnumeratePorts()
        {
            Ports.Clear();
            IList<string> newPorts = Rs232PortsEnumerator.GetAvailablePorts();
            Ports = newPorts;
            return newPorts;
        }
        
        #endregion
        
        public void ShowSelectedSerialDeviceSetting(int portNumber)
        {
            if (portNumber < 0)
                return;
            SerialDeviceModel device = _serialDevices.FirstOrDefault(d => d.Settings.PortNumber == portNumber);
            if (device != null)
            {
                SelectedPortNumber = $"COM{portNumber}";
                this.RaisePropertyChanged(nameof(SelectedPortNumber));
                SelectedBaudRate = SerialOptions.BaudRates.FirstOrDefault(b => b.Value == device.Settings.BaudRate).Key;
                this.RaisePropertyChanged(nameof(SelectedBaudRate));
                SelectedByteLength = SerialOptions.ByteLength.FirstOrDefault(b => b.Value == device.Settings.ByteLength).Key;
                this.RaisePropertyChanged(nameof(SelectedByteLength));
                SelectedParity = SerialOptions.Parities.FirstOrDefault(b => b.Value == device.Settings.Parity).Key;
                this.RaisePropertyChanged(nameof(SelectedParity));
                SelectedStopBits = SerialOptions.StopBits.FirstOrDefault(b => b.Value == device.Settings.StopBits).Key;
                this.RaisePropertyChanged(nameof(SelectedStopBits));
                SelectedFlowControl = SerialOptions.FlowControls.FirstOrDefault(b => b.Value == device.Settings.FlowControl).Key;
                this.RaisePropertyChanged(nameof(SelectedFlowControl));
                // todo(UMV): add Xon+Xoff restore
                UpdateStatusbar(device);
            }
        }

        public void ResourcesCleanUp()
        {
            _deviceManager.Dispose();
        }

        #region RS232TreeConfiguration
        public ObservableCollection<SerialPortShortInfoModel> DevicesConfigs { get; set; }
        
        #endregion

        #region RS232DeviceConfiguration
        
        public IList<string> Ports
        {
            get { return _ports; }
            set
            {
                _ports = value;
                this.RaisePropertyChanged();
            }
        }

        public string ConnectButtonText { get; set; }

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
        
        #endregion

        #region RS232Messages
        public ObservableCollection<string> SerialDeviceMessages { get; set; }
        public string SerialDeviceMessageToSend { get; set; }

        #endregion

        #region Rs232StatusBar

        private void UpdateStatusbar(SerialDeviceModel device)
        {
            Rs232SelectedDevicePort = string.Format(Rs232SelectedPortStatusBarMessageTemplate, SelectedPortNumber);
            this.RaisePropertyChanged(nameof(Rs232SelectedDevicePort));
            string strStatus = device.Connected ? "Connected" : "Disconnected";
            Rs232SelectedDeviceStatus = Rs232SelectedDeviceStatus = string.Format(Rs232SelectedDeviceStatusStatusBarMessageTemplate, strStatus);
            this.RaisePropertyChanged(nameof(Rs232SelectedDeviceStatus));
            Rs232SelectedDeviceBytesReceived = device.BytesReceived;
            this.RaisePropertyChanged(nameof(Rs232SelectedDeviceBytesReceived));
            Rs232SelectedDeviceBytesSent = device.BytesSend;
            this.RaisePropertyChanged(nameof(Rs232SelectedDeviceBytesSent));
        }

        public string Rs232SelectedDeviceStatus { get; set; }
        public string Rs232SelectedDevicePort { get; set; }
        public string Rs232SelectedDeviceBytesSent { get; set; }
        public string Rs232SelectedDeviceBytesReceived { get; set; }
        #endregion

        private const string Rs232SelectedPortStatusBarMessageTemplate = "Selected Port: {0}";
        private const string Rs232SelectedDeviceStatusStatusBarMessageTemplate = "Status: {0}";
        
        private IList<string> _ports;
        private readonly IList<SerialDeviceModel> _serialDevices;
        private readonly IRs232DeviceManager _deviceManager;
        private string _selectedFlowControl;
        private string _selectedPortName;
    }
}