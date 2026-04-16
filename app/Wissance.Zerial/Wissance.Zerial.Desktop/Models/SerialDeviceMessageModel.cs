using System;
using System.Linq;
using System.Text;
using Jeek.Avalonia.Localization;

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

        public string ToString(string deviceName)
        {
            if (MessageType == MessageType.Special)
            {
                return string.Format(SpecialMessageTemplate, Time.ToString(Localizer.Get(DateTimeFormatKey)), deviceName, PreFormedMessage);
            }

            if (MessageType == MessageType.Connect || MessageType == MessageType.Disconnect)
            {
                return string.Format(StatusMessageTemplate, Time.ToString(Localizer.Get(DateTimeFormatKey)), deviceName,
                    MessageType == MessageType.Connect ? Localizer.Get(ConnectedDeviceStateKey) : Localizer.Get(DisconnectedDeviceStateKey));
            }

            StringBuilder dataAsStr = new StringBuilder();
            if (RawData != null && RawData.Any())
            {
                foreach (byte b in RawData)
                {
                    if (dataAsStr.Length > 0)
                        dataAsStr.Append(" ");
                    dataAsStr.Append($"0x{b:X}");
                }
            }

            return string.Format(IoMessageTemplate, Time.ToString(Localizer.Get(DateTimeFormatKey)), deviceName,
                MessageType == MessageType.Read ? Localizer.Get(DataReadMessagePrefixKey) : Localizer.Get(DataWriteMessagePrefixKey), dataAsStr);
        }

        public string PreFormedMessage { get; }
        public DateTime Time { get; }
        public MessageType MessageType { get; }
        public byte[] RawData { get; }

        private const string SpecialMessageTemplate = "[{0}] : {1} : {2}";
        private const string StatusMessageTemplate = "[{0}] : {1} : {2}";
        private const string IoMessageTemplate = "[{0}] : {1} : {2} : {3}";
        
        private const string ConnectedDeviceStateKey = "Zerial_Device_Connected_State";
        private const string DisconnectedDeviceStateKey = "Zerial_Device_Disconnected_State";
        private const string DateTimeFormatKey = "Zerial_Date_Time_Format";
        private const string DataReadMessagePrefixKey = "Zerial_Data_Read_Message_Prefix";
        private const string DataWriteMessagePrefixKey = "Zerial_Data_Write_Message_Prefix";
    }
}