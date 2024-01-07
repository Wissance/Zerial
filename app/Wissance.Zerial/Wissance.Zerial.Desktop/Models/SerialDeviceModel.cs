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
            sentMessages.Aggregate(totalBytesReceived, (t, m) =>
            {
                return t += m.RawData.Length;
            });
            return string.Format(template, totalBytesReceived);
        }

        public const string BytesSentTemplate = "Bytes sent: {0}";
        public const string BytesReceivedTemplate = "Bytes received: {0}";
    }
}