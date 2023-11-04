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
            infoBuilder.Append($"COM{Settings.PortNumber}, ");
            infoBuilder.Append($"{(int)Settings.BaudRate} b/s, ");
            infoBuilder.Append($"{Settings.ByteLength}b, ");

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

            SerialPortShortInfoModel info = new SerialPortShortInfoModel(Settings.PortNumber, infoBuilder.ToString());
            return info;
        }

        public bool Connected { get; set; }
        public Rs232Settings Settings { get; set; }
        public IList<SerialDeviceMessageModel> Messages { get; set; }
    }
}