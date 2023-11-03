using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public SerialPortShortInfoModel ToShortInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.Append($"COM{Settings.PortNumber} ,");
            infoBuilder.Append($"{(int)Settings.BaudRate} b/s,");
            infoBuilder.Append($"{Settings.ByteLength}b");
            // todo(UMV): add something too ...
            SerialPortShortInfoModel info = new SerialPortShortInfoModel(Settings.PortNumber, infoBuilder.ToString());
            return info;
        }

        public bool Connected { get; set; }
        public Rs232Settings Settings { get; set; }
        public IList<SerialDeviceMessageModel> Messages { get; set; }
    }
}