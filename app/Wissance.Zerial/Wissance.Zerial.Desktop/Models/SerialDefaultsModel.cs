using System.Collections.Generic;
using System.Linq;
using Wissance.Zerial.Common.Rs232;
using Wissance.Zerial.Common.Rs232.Settings;

namespace Wissance.Zerial.Desktop.Models
{
    public class SerialDefaultsModel
    {
        public SerialDefaultsModel()
        {
        }

        public IDictionary<string, Rs232BaudRate> BaudRates => _baudRates;
        public IDictionary<string, Rs232StopBits> StopBits => _stopBits;
        public IDictionary<string, int> ByteLength => _byteLengthBits;
        public IDictionary<string, Rs232Parity> Parities => _parities;
        
        public IDictionary<string, Rs232FlowControl> FlowControls => _flowControls;
        public IList<string> BaudRatesOptions => _baudRates.Keys.ToList();
        public IList<string> StopBitsOptions => _stopBits.Keys.ToList();
        public IList<string> ByteLengthOptions => _byteLengthBits.Keys.ToList();
        public IList<string> ParitiesOptions => _parities.Keys.ToList();
        public IList<string> FlowControlsOptions => _flowControls.Keys.ToList();
        
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

        private readonly IDictionary<string, Rs232Parity> _parities = new Dictionary<string, Rs232Parity>()
        {
            {"No parity", Rs232Parity.NoParity},
            {"Mark", Rs232Parity.Mark},
            {"Space", Rs232Parity.Space},
            {"Even", Rs232Parity.Even},
            {"Odd", Rs232Parity.Odd}
        };

        private readonly IDictionary<string, Rs232FlowControl> _flowControls = new Dictionary<string, Rs232FlowControl>()
        {
            {"No", Rs232FlowControl.NoControl},
            {"RTS/CTS", Rs232FlowControl.RtsCts},
            {"Xon/Xoff", Rs232FlowControl.XonXoff}
        };

        public const string DefaultXon = "Ox11";
        public const string DefaultXoff = "Ox13";
    }
}