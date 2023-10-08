using System.Collections.Generic;
using System.Linq;
using Wissance.Zerial.Common.Rs232;

namespace Wissance.Zerial.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IList<string> BaudRates => _baudRates.Keys.ToList();
        public string SelectedBaudRate { get; set; }

        public IList<string> StopBits => _stopBits.Keys.ToList();
        
        public string SelectedStopBits { get; set; }

        public IList<string> ByteLength => _byteLengthBits.Keys.ToList();
        
        public string SelectedByteLength { get; set; }

        public IList<string> FlowControls => _flowControls.Keys.ToList();
        
        public string SelectedFlowControl { get; set; }
        
        private readonly IDictionary<string, Rs232BaudRate> _baudRates = new Dictionary<string, Rs232BaudRate>()
        {
            {"9600", Rs232BaudRate.BaudMode9600},
            {"19200", Rs232BaudRate.BaudMode19200},
            {"38400", Rs232BaudRate.BaudMode38400},
            {"57600", Rs232BaudRate.BaudMode57600},
            {"115200", Rs232BaudRate.BaudMode115200}
        };

        private readonly IDictionary<string, Rs232StopBits> _stopBits = new Dictionary<string, Rs232StopBits>()
        {
            {"One", Rs232StopBits.One},
            {"One and half", Rs232StopBits.OneAndHalf},
            {"Two", Rs232StopBits.Two}
        };
        
        private readonly IDictionary<string, int> _byteLengthBits = new Dictionary<string, int>()
        {
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8}
        };

        private readonly IDictionary<string, Rs232FlowControl> _flowControls = new Dictionary<string, Rs232FlowControl>()
        {
            {"No", Rs232FlowControl.NoControl},
            {"RTS/CTS", Rs232FlowControl.RtsCts},
            {"Xon/Xoff", Rs232FlowControl.XonXoff}
        };
    }
}