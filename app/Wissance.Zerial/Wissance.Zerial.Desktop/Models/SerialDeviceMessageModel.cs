using System;
using System.Linq;
using System.Text;

namespace Wissance.Zerial.Desktop.Models
{
    public enum MessageType
    {
        Special = 1,
        Connect,
        Disconnect,
        Read,  // from Device to PC
        Write  // from PC to Device
    }
    
    public class SerialDeviceMessageModel
    {
        public SerialDeviceMessageModel(MessageType messageType, DateTime timestamp, byte[] rawData = null, string preFormedMsg = "")
        {
            Time = timestamp;
            MessageType = messageType;
            RawData = rawData;
            PreFormedMessage = preFormedMsg;
        }

        public string ToString(int portNumber)
        {
            if (MessageType == MessageType.Special)
            {
                return string.Format(SpecialMessageTemplate, Time.ToString("yyyy-MM-dd HH:mm:ss"), portNumber,
                    MessageType == MessageType.Connect ? "Connected" : "Disconnected");
            }

            if (MessageType == MessageType.Connect || MessageType == MessageType.Disconnect)
            {
                return string.Format(StatusMessageTemplate, Time.ToString("yyyy-MM-dd HH:mm:ss"), portNumber,
                    PreFormedMessage);
            }

            StringBuilder dataAsStr = new StringBuilder();
            if (RawData != null && RawData.Any())
            {
                foreach (byte b in RawData)
                {
                    if (dataAsStr.Length > 0)
                        dataAsStr.Append(" ");
                    dataAsStr.Append(string.Format("0x{0}", b.ToString("X")));
                }
            }

            return string.Format(IoMessageTemplate, Time.ToString("yyyy-MM-dd HH:mm:ss"), portNumber,
                MessageType == MessageType.Read ? "Read" : "Write", dataAsStr);
        }

        public string PreFormedMessage { get; }
        public DateTime Time { get; }
        public MessageType MessageType { get; }
        public byte[] RawData { get; }

        private const string SpecialMessageTemplate = "[{0}] : COM{1} : {2}";
        private const string StatusMessageTemplate = "[{0}] : COM{1} : {2}";
        private const string IoMessageTemplate = "[{0}] : COM{1} : {2} : {3}";
    }
}