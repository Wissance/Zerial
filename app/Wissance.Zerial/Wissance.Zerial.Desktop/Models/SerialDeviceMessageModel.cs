using System;
using System.Linq;
using System.Text;

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
                return string.Format(StatusMessageTemplate, Time.ToString("yyyy-MM-dd HH:mm:ss"), portNumber,
                    MessageType == MessageType.Connect ? "Connected" : "Disconnected");
            }

            StringBuilder dataAsStr = new StringBuilder();
            if (RawData != null && RawData.Any())
            {
                foreach (byte b in RawData)
                {
                    if (dataAsStr.Length > 0)
                        dataAsStr.Append(" ");
                    dataAsStr.Append(string.Format("0x:{0:b}", b));
                }
            }

            return string.Format(IoMessageTemplate, Time.ToString("yyyy-MM-dd HH:mm:ss"), portNumber,
                MessageType == MessageType.Read ? "Read" : "Write", dataAsStr);
        }

        public DateTime Time { get; set; }
        public MessageType MessageType { get; set; }
        public byte[] RawData { get; set; }

        private const string StatusMessageTemplate = "[{0}] : COM{1} : {2}";
        private const string IoMessageTemplate = "[{0}] : COM{1} : {2} : {3}";
    }
}