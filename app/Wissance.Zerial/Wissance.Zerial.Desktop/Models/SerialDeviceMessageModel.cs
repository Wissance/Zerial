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

        public DateTime Time { get; set; }
        public MessageType MessageType { get; set; }
        public byte[] RawData { get; set; }
    }
}