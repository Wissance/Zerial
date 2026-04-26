using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Jeek.Avalonia.Localization;
using Wissance.Zerial.Common.Rs232;
using Wissance.Zerial.Common.Rs232.Settings;

namespace Wissance.Zerial.Desktop.Models
{
    public class SerialDeviceModel
    {
        public SerialDeviceModel()
        {
            Messages = new List<SerialDeviceMessageModel>();
            Settings = new Rs232Settings();
        }

        public SerialDeviceModel(bool connected, Rs232Settings settings, IList<SerialDeviceMessageModel> messages)
        {
            Connected = connected;
            Settings = settings;
            Messages = messages;
        }

        public SerialDeviceModel(string deviceConfiguration)
        {
            // parse back from string to device
            Settings = new Rs232Settings();            
            Connected = false;
            Messages = new List<SerialDeviceMessageModel>();
            // device configuration has format - "COM3, 115200 b/s, 8bit, 1 Sb, Even, No FC" (EN Localization)
            string[] parts = deviceConfiguration.Split(",");
            string portStr = parts.Any() ? parts[PortIndex] : string.Empty;
            Settings.DeviceName = parts[PortIndex];
            if (string.IsNullOrEmpty(portStr))
                return;
            
            Settings.DeviceName = portStr;
            Settings.BaudRate = (Rs232BaudRate)GetSettingsValueFromParsedString<int>(parts, BaudRateIndex);
            Settings.ByteLength = GetSettingsValueFromParsedString<int>(parts, ByteLengthIndex);
            Settings.StopBits = (Rs232StopBits)GetSettingsValueFromParsedString<int>(parts, StopBitIndex);
            Settings.Parity = (Rs232Parity)GetSettingsValueFromParsedString<int>(parts, ParityIndex);
            Settings.FlowControl = (Rs232FlowControl)GetSettingsValueFromParsedString<int>(parts, FlowControlIndex);
            
            Configuration = deviceConfiguration;
        }
        
        public string GetDisplayInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.Append($"{Settings.DeviceName}, ");
            infoBuilder.Append($"{(int)Settings.BaudRate} {Localizer.Get(BitPerSecondUnitKey)}, ");
            infoBuilder.Append($"{Settings.ByteLength}{Localizer.Get(BitUnitKey)}, ");
            
            infoBuilder.Append(_stopBitsOptions[Settings.StopBits]);
            infoBuilder.Append(", ");

            infoBuilder.Append(_parityOptions[Settings.Parity]);
            infoBuilder.Append(", ");

            infoBuilder.Append(_flowControlOptions[Settings.FlowControl]);

            // SerialPortShortInfoModel info = new SerialPortShortInfoModel(Connected, Settings.DeviceName, Configuration,
            //    infoBuilder.ToString());
            return infoBuilder.ToString();
        }
        
        public string BytesSend
        {
            get
            {
                return GetNumberOfBytesMessage(MessageType.Write, Localizer.Get(BytesSentStatsTemplateKey));
            }
        }

        public string BytesReceived
        {
            get
            {
                return GetNumberOfBytesMessage(MessageType.Read, Localizer.Get(BytesReceivedStatsTemplateKey));
            }
        }

        private string GetNumberOfBytesMessage(MessageType type, string template)
        {
            long totalBytesReceived = 0;
            IList<SerialDeviceMessageModel> sentMessages = Messages.Where(m => m.MessageType == type).ToList();
            totalBytesReceived = sentMessages.Aggregate(0, (t, m) => t + m.RawData.Length);
            return string.Format(template, totalBytesReceived);
        }

        private T GetSettingsValueFromParsedString<T>(string[] parsedStr, int settingsPropertyIndex)
        {
            try
            {
                if (parsedStr.Length <= settingsPropertyIndex)
                    return default(T);
                string stopBitStr = parsedStr[settingsPropertyIndex];
                string[] stopBitParts =  stopBitStr.Trim().Split(' ');
                string rawValue = stopBitParts[0].Trim();
                return (T)Convert.ChangeType(rawValue, typeof(T));
            }
            catch (Exception e)
            {
                return default(T);
            }
            
        }
        
        public bool Connected { get; set; }
        public Rs232Settings Settings { get; set; }
        public IList<SerialDeviceMessageModel> Messages { get; set; }
        
        public string Configuration { get; private set; }

        private readonly IDictionary<Rs232StopBits, string> _stopBitsOptions = new Dictionary<Rs232StopBits, string>()
        {
            {Rs232StopBits.None, $"No {Localizer.Get(StopBitDesignationKey)}"},
            {Rs232StopBits.One, $"1 {Localizer.Get(StopBitDesignationKey)}"},
            {Rs232StopBits.OneAndHalf, $"1.5 {Localizer.Get(StopBitDesignationKey)}"},
            {Rs232StopBits.Two, $"2 {Localizer.Get(StopBitDesignationKey)}"}
        };

        private readonly IDictionary<Rs232Parity, string> _parityOptions = new Dictionary<Rs232Parity, string>()
        {
            {Rs232Parity.NoParity, Localizer.Get(NoParityKey)},
            {Rs232Parity.Mark, Localizer.Get(MarkParityKey)},
            {Rs232Parity.Space, Localizer.Get(SpaceParityKey)},
            {Rs232Parity.Even, Localizer.Get(EvenParityKey)},
            {Rs232Parity.Odd, Localizer.Get(OddParityKey)}
        };

        private readonly IDictionary<Rs232FlowControl, string> _flowControlOptions = new Dictionary<Rs232FlowControl, string>()
        {
            {Rs232FlowControl.NoControl, Localizer.Get(NoFlowControlKey)},
            {Rs232FlowControl.XonXoff, Localizer.Get(XonXoffFlowControlKey)},
            {Rs232FlowControl.RtsCts, Localizer.Get(CtsRtsFlowControlKey)}
        };

        // Configuration storing as follows - "COM3, 115200 b/s, 8 bit, 1 Sb, Even, No FC" (En Localization)
        private const int PortIndex = 0;
        private const int BaudRateIndex = 1;
        private const int ByteLengthIndex = 2;
        private const int StopBitIndex = 3;
        private const int ParityIndex = 4;
        private const int FlowControlIndex = 5;

        private const string BitUnitKey = "Zerial_Bit_Unit";
        private const string BitPerSecondUnitKey = "Zerial_Bit_Per_Sec_Unit";
        private const string StopBitDesignationKey = "Zerial_Stop_Bit_Designation";
        
        private const string NoParityKey = "Zerial_No_Parity_Designation";
        private const string MarkParityKey = "Zerial_Mark_Parity";
        private const string SpaceParityKey = "Zerial_Space_Parity";
        private const string EvenParityKey = "Zerial_Even_Parity";
        private const string OddParityKey = "Zerial_Odd_Parity";
        
        private const string NoFlowControlKey = "Zerial_No_Flow_Control_Designation";
        private const string CtsRtsFlowControlKey = "Zerial_RTS_CTS_Flow_Control";
        private const string XonXoffFlowControlKey = "Zerial_Xon_Xoff_Flow_Control";
        
        private const string BytesSentStatsTemplateKey = "Zerial_Bytes_Sent_Stats_Template";
        private const string BytesReceivedStatsTemplateKey = "Zerial_Bytes_Received_Stats_Template";
    }
}