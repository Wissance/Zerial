using System;

namespace Wissance.Zerial.Desktop.Models
{
    public enum MessageType
    {
        Connect,
        Disconnect,
        Read,  // from Device to PC
        Write  // from PC to Device
    }
    
    public class SerialDeviceMessageModel
    {
        public SerialDeviceMessageModel(MessageType messageType, DateTime timestamp, byte[] rawData)
        {
            Time = timestamp;
            MessageType = messageType;
            RawData = rawData;
        }

        public string ToString(int portNumber)
        {
            if (MessageType == MessageType.Connect || MessageType == MessageType.Disconnect)
            {
                return string.Format(StatusMessageTemplate, Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    MessageType == MessageType.Connect ? "Connected" : "Disconnected");
            }

            return "";
        }

        public DateTime Time { get; set; }
        public MessageType MessageType { get; set; }
        public byte[] RawData { get; set; }

        private const string StatusMessageTemplate = "[{0}] : {1}";
        private const string IoMessageTemplate = "[{0}] : {1} : {2}";
    }
}