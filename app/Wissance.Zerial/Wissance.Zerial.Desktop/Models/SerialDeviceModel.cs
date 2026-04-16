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
            Settings = new Rs232Settings();            Connected = false;
            Messages = new List<SerialDeviceMessageModel>();
            // device configuration has format - "COM3, 115200 b/s, 8bit, 1 Sb, Even, No FC"
            string[] parts = deviceConfiguration.Split(",");
            string portStr = parts.Any() ? parts[0] : null;
            Settings.DeviceName = parts[0];
            if (portStr != null)
            {
                Settings.DeviceName = portStr;
            }
            
            // baud rate
            string baudRateStr = parts.FirstOrDefault(p => p.Contains(Localizer.Get(BitPerSecondUnitKey)));
            if (baudRateStr != null)
            {
                string[] baudRateParts = baudRateStr.Trim().Split(' ');
                int baudRate;
                if (int.TryParse(baudRateParts[0].Trim(), out baudRate))
                {
                    Settings.BaudRate = (Rs232BaudRate)baudRate;
                }
            }
            // byte len
            string byteLengthStr = parts.FirstOrDefault(p => p.Contains(Localizer.Get(BitUnitKey)));
            if (byteLengthStr != null)
            {
                string byteLengthValueStr = byteLengthStr.Trim().Substring(0, 1);
                int byteLength;
                if (int.TryParse(byteLengthValueStr, out byteLength))
                {
                    Settings.ByteLength = byteLength;
                }
            }
            // stop bits
            string stopBitsStr = parts.FirstOrDefault(p => p.Contains($"{StopBitDesignationKey}"));
            if (stopBitsStr != null)
            {
                stopBitsStr = stopBitsStr.Trim();
                if (_stopBitsOptions.ContainsKey(stopBitsStr))
                    Settings.StopBits = _stopBitsOptions[stopBitsStr];
            }
            // parity
            string parityStr = parts.FirstOrDefault(p => _parityOptions.Keys.Contains(p.Trim()));
            if (parityStr != null)
            {
                Settings.Parity = _parityOptions[parityStr.Trim()];
            }
            // flow control
            string flowControlStr = parts.FirstOrDefault(p => _flowControlOptions.Keys.Contains(p.Trim()));
            if (flowControlStr != null)
            {
                Settings.FlowControl = _flowControlOptions[flowControlStr.Trim()];
            }
        }
        
        public SerialPortShortInfoModel ToShortInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.Append($"{Settings.DeviceName}, ");
            infoBuilder.Append($"{(int)Settings.BaudRate} {Localizer.Get(BitPerSecondUnitKey)}, ");
            infoBuilder.Append($"{Settings.ByteLength}{Localizer.Get(BitUnitKey)}, ");

            // todo(UMV): use dictionary
            switch (Settings.StopBits)
            {
                case Rs232StopBits.None:
                    infoBuilder.Append($"No {StopBitDesignationKey}");
                    break;
                case Rs232StopBits.One:
                    infoBuilder.Append($"1 {StopBitDesignationKey}");
                    break;
                case Rs232StopBits.OneAndHalf:
                    infoBuilder.Append($"1.5 {StopBitDesignationKey}");
                    break;
                case Rs232StopBits.Two:
                    infoBuilder.Append($"2 {StopBitDesignationKey}");
                    break;
            }
            infoBuilder.Append(", ");

            // todo(UMV): use dictionary
            switch (Settings.Parity)
            {
                case Rs232Parity.NoParity:
                    infoBuilder.Append(Localizer.Get(NoParityKey));
                    break;
                case Rs232Parity.Mark:
                    infoBuilder.Append(Localizer.Get(MarkParityKey));
                    break;
                case Rs232Parity.Space:
                    infoBuilder.Append(Localizer.Get(SpaceParityKey));
                    break;
                case Rs232Parity.Even:
                    infoBuilder.Append(Localizer.Get(EvenParityKey));
                    break;
                case Rs232Parity.Odd:
                    infoBuilder.Append(Localizer.Get(OddParityKey));
                    break;
            }
            infoBuilder.Append(", ");

            // todo(UMV): use dictionary
            switch (Settings.FlowControl)
            {
                case Rs232FlowControl.NoControl:
                    infoBuilder.Append(Localizer.Get(NoFlowControlKey));
                    break;
                case Rs232FlowControl.XonXoff:
                    infoBuilder.Append(Localizer.Get(XonXoffFlowControlKey));
                    break;
                case Rs232FlowControl.RtsCts:
                    infoBuilder.Append(Localizer.Get(CtsRtsFlowControlKey));
                    break;
            }

            SerialPortShortInfoModel info = new SerialPortShortInfoModel(Connected, Settings.DeviceName, infoBuilder.ToString());
            return info;
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
        
        public bool Connected { get; set; }
        public Rs232Settings Settings { get; set; }
        public IList<SerialDeviceMessageModel> Messages { get; set; }

        //public const string BytesSentTemplate = "Bytes sent: {0}";
        //public const string BytesReceivedTemplate = "Bytes received: {0}";

        private readonly IDictionary<string, Rs232StopBits> _stopBitsOptions = new Dictionary<string, Rs232StopBits>()
        {
            {$"No {StopBitDesignationKey}", Rs232StopBits.None},
            {$"1 {StopBitDesignationKey}", Rs232StopBits.One},
            {$"1.5 {StopBitDesignationKey}", Rs232StopBits.OneAndHalf},
            {$"2 {StopBitDesignationKey}", Rs232StopBits.Two}
        };

        private readonly IDictionary<string, Rs232Parity> _parityOptions = new Dictionary<string, Rs232Parity>()
        {
            {Localizer.Get(NoParityKey), Rs232Parity.NoParity},
            {Localizer.Get(MarkParityKey), Rs232Parity.Mark},
            {Localizer.Get(SpaceParityKey), Rs232Parity.Space},
            {Localizer.Get(EvenParityKey), Rs232Parity.Even},
            {Localizer.Get(OddParityKey), Rs232Parity.Odd}
        };

        private readonly IDictionary<string, Rs232FlowControl> _flowControlOptions = new Dictionary<string, Rs232FlowControl>()
        {
            {Localizer.Get(NoFlowControlKey), Rs232FlowControl.NoControl},
            {Localizer.Get(XonXoffFlowControlKey), Rs232FlowControl.XonXoff},
            {Localizer.Get(CtsRtsFlowControlKey), Rs232FlowControl.RtsCts}
        };

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