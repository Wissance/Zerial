using System.Collections.ObjectModel;
using Wissance.Zerial.Common.Rs232;

namespace Wissance.Zerial.Desktop.Models
{
    public class SerialDeviceModel
    {
        public bool Connected { get; set; }
        public Rs232Settings Settings { get; set; }
        public ObservableCollection<SerialDeviceMessageModel> Messages { get; set; }
    }
}