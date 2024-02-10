using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            Settings = new Rs232Settings();
            Connected = false;
            Messages = new List<SerialDeviceMessageModel>();
            // device configuration has format - "COM3, 115200 b/s, 8bit, 1 Sb, Even, No FC"
            string[] parts = deviceConfiguration.Split(",");
            string portStr = parts.FirstOrDefault(p => p.Trim().StartsWith("COM"));
            if (portStr != null)
            {
                string number = portStr.Remove(0, 3);
                int portNumber;
                if (int.TryParse(number, out portNumber))
                {
                    Settings.PortNumber = portNumber;
                }
            }
            
            // baud rate
            string baudRateStr = parts.FirstOrDefault(p => p.Contains("b/s"));
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
            string byteLengthStr = parts.FirstOrDefault(p => p.Contains("bit"));
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
            string stopBitsStr = parts.FirstOrDefault(p => p.Contains("Sb"));
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
            infoBuilder.Append($"COM{Settings.PortNumber}, ");
            infoBuilder.Append($"{(int)Settings.BaudRate} b/s, ");
            infoBuilder.Append($"{Settings.ByteLength}bit, ");

            // todo(UMV): use dictionary
            switch (Settings.StopBits)
            {
                case Rs232StopBits.None:
                    infoBuilder.Append("No Sb");
                    break;
                case Rs232StopBits.One:
                    infoBuilder.Append("1 Sb");
                    break;
                case Rs232StopBits.OneAndHalf:
                    infoBuilder.Append("1.5 Sb");
                    break;
                case Rs232StopBits.Two:
                    infoBuilder.Append("2 Sb");
                    break;
            }
            infoBuilder.Append(", ");

            // todo(UMV): use dictionary
            switch (Settings.Parity)
            {
                case Rs232Parity.NoParity:
                    infoBuilder.Append("No P");
                    break;
                case Rs232Parity.Mark:
                    infoBuilder.Append("Mark");
                    break;
                case Rs232Parity.Space:
                    infoBuilder.Append("Space");
                    break;
                case Rs232Parity.Even:
                    infoBuilder.Append("Even");
                    break;
                case Rs232Parity.Odd:
                    infoBuilder.Append("Odd");
                    break;
            }
            infoBuilder.Append(", ");

            // todo(UMV): use dictionary
            switch (Settings.FlowControl)
            {
                case Rs232FlowControl.NoControl:
                    infoBuilder.Append("No FC");
                    break;
                case Rs232FlowControl.XonXoff:
                    infoBuilder.Append("Xon/Xoff");
                    break;
                case Rs232FlowControl.RtsCts:
                    infoBuilder.Append("Rts/Cts");
                    break;
            }

            SerialPortShortInfoModel info = new SerialPortShortInfoModel(Connected, Settings.PortNumber, infoBuilder.ToString());
            return info;
        }

        public bool Connected { get; set; }
        public Rs232Settings Settings { get; set; }
        public IList<SerialDeviceMessageModel> Messages { get; set; }

        public string BytesSend
        {
            get
            {
                return GetNumberOfBytesMessage(MessageType.Write, BytesSentTemplate);
            }
        }

        public string BytesReceived
        {
            get
            {
                return GetNumberOfBytesMessage(MessageType.Read, BytesReceivedTemplate);
            }
        }

        private string GetNumberOfBytesMessage(MessageType type, string template)
        {
            long totalBytesReceived = 0;
            IList<SerialDeviceMessageModel> sentMessages = Messages.Where(m => m.MessageType == type).ToList();
            totalBytesReceived = sentMessages.Aggregate(0, (t, m) => t + m.RawData.Length);
            return string.Format(template, totalBytesReceived);
        }

        public const string BytesSentTemplate = "Bytes sent: {0}";
        public const string BytesReceivedTemplate = "Bytes received: {0}";

        private readonly IDictionary<string, Rs232StopBits> _stopBitsOptions = new Dictionary<string, Rs232StopBits>()
        {
            {"No Sb", Rs232StopBits.None},
            {"1 Sb", Rs232StopBits.One},
            {"1.5 Sb", Rs232StopBits.OneAndHalf},
            {"2 Sb", Rs232StopBits.Two}
        };

        private readonly IDictionary<string, Rs232Parity> _parityOptions = new Dictionary<string, Rs232Parity>()
        {
            {"No P", Rs232Parity.NoParity},
            {"Mark", Rs232Parity.Mark},
            {"Space", Rs232Parity.Space},
            {"Even", Rs232Parity.Even},
            {"Odd", Rs232Parity.Odd}
        };

        private readonly IDictionary<string, Rs232FlowControl> _flowControlOptions = new Dictionary<string, Rs232FlowControl>()
        {
            {"No FC", Rs232FlowControl.NoControl},
            {"Xon/Xoff", Rs232FlowControl.XonXoff},
            {"Rts/Cts", Rs232FlowControl.RtsCts}
        };
    }
}