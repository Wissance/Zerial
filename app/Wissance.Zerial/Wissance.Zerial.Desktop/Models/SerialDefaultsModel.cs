using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaEdit.Utils;
using Jeek.Avalonia.Localization;
using Wissance.Zerial.Common.Rs232;
using Wissance.Zerial.Common.Rs232.Settings;

namespace Wissance.Zerial.Desktop.Models
{
    /// <summary>
    ///   SerialDefaultsModel is a class that holds RS232 Connect Options to any Serial (RS232) Device / Port 
    /// </summary>
    public class SerialDefaultsModel
    {
        public SerialDefaultsModel()
        {
            FlowControlsOptions = new ObservableCollection<string>(_flowControls.Keys);
            StopBitsOptions = new ObservableCollection<string>(_stopBits.Keys);
            ParitiesOptions = new ObservableCollection<string>(_parities.Keys);
        }

        public void ReloadOptions()
        {
            _stopBits.Clear();
            _stopBits.AddRange(new List<KeyValuePair<string, Rs232StopBits>>()
                {
                    new KeyValuePair<string, Rs232StopBits>(Localizer.Get(NoStopBitsKey), Rs232StopBits.None),
                    new KeyValuePair<string, Rs232StopBits>(Localizer.Get(OneStopBitsKey), Rs232StopBits.One),
                    new KeyValuePair<string, Rs232StopBits>(Localizer.Get(OneAndHalfStopBitsKey), Rs232StopBits.OneAndHalf),
                    new KeyValuePair<string, Rs232StopBits>(Localizer.Get(TwoStopBitsKey), Rs232StopBits.Two)
                });
            StopBitsOptions.Clear();
            StopBitsOptions.AddRange(_stopBits.Keys);
            
            _parities.Clear();
            _parities.AddRange(new List<KeyValuePair<string, Rs232Parity>>()
            {
                new KeyValuePair<string, Rs232Parity>(Localizer.Get(NoParityKey), Rs232Parity.NoParity),
                new KeyValuePair<string, Rs232Parity>(Localizer.Get(MarkParityKey), Rs232Parity.Mark),
                new KeyValuePair<string, Rs232Parity>(Localizer.Get(SpaceParityKey), Rs232Parity.Space),
                new KeyValuePair<string, Rs232Parity>(Localizer.Get(EvenParityKey), Rs232Parity.Even),
                new KeyValuePair<string, Rs232Parity>(Localizer.Get(OddParityKey), Rs232Parity.Odd)
            });
            
            ParitiesOptions.Clear();
            ParitiesOptions.AddRange(_parities.Keys);
            
            _flowControls.Clear();
            _flowControls.AddRange(new List<KeyValuePair<string, Rs232FlowControl>>()
                {
                    new KeyValuePair<string, Rs232FlowControl>(Localizer.Get(NoFlowControlKey),
                        Rs232FlowControl.NoControl),
                    new KeyValuePair<string, Rs232FlowControl>(Localizer.Get(CtsRtsFlowControlKey),
                        Rs232FlowControl.RtsCts),
                    new KeyValuePair<string, Rs232FlowControl>(Localizer.Get(XonXoffFlowControlKey),
                        Rs232FlowControl.XonXoff)
                }
            );
            
            FlowControlsOptions.Clear();
            FlowControlsOptions.AddRange(_flowControls.Keys);
        }

        public IDictionary<string, Rs232BaudRate> BaudRates => _baudRates;
        public IDictionary<string, Rs232StopBits> StopBits => _stopBits;
        public IDictionary<string, int> ByteLength => _byteLengthBits;
        public IDictionary<string, Rs232Parity> Parities => _parities;
        
        public IDictionary<string, Rs232FlowControl> FlowControls => _flowControls;
        public IList<string> BaudRatesOptions => _baudRates.Keys.ToList();
        public IList<string> StopBitsOptions { get; set; }
        public IList<string> ByteLengthOptions => _byteLengthBits.Keys.ToList();
        public IList<string> ParitiesOptions { get; set; }
        public IList<string> FlowControlsOptions { get; set; }
        
        private readonly IDictionary<string, Rs232BaudRate> _baudRates = new Dictionary<string, Rs232BaudRate>()
        {
            {"9600", Rs232BaudRate.BaudMode9600},
            {"19200", Rs232BaudRate.BaudMode19200},
            {"38400", Rs232BaudRate.BaudMode38400},
            {"57600", Rs232BaudRate.BaudMode57600},
            {"115200", Rs232BaudRate.BaudMode115200}
        };

        private IDictionary<string, Rs232StopBits> _stopBits = new Dictionary<string, Rs232StopBits>()
        {
            {Localizer.Get(NoStopBitsKey), Rs232StopBits.None},
            {Localizer.Get(OneStopBitsKey), Rs232StopBits.One},
            {Localizer.Get(OneAndHalfStopBitsKey), Rs232StopBits.OneAndHalf},
            {Localizer.Get(TwoStopBitsKey), Rs232StopBits.Two}
        };
        
        private readonly IDictionary<string, int> _byteLengthBits = new Dictionary<string, int>()
        {
            {"5", 5},
            {"6", 6},
            {"7", 7},
            {"8", 8}
        };

        private IDictionary<string, Rs232Parity> _parities = new Dictionary<string, Rs232Parity>()
        {
            {Localizer.Get(NoParityKey), Rs232Parity.NoParity},
            {Localizer.Get(MarkParityKey), Rs232Parity.Mark},
            {Localizer.Get(SpaceParityKey), Rs232Parity.Space},
            {Localizer.Get(EvenParityKey), Rs232Parity.Even},
            {Localizer.Get(OddParityKey), Rs232Parity.Odd}
        };

        private IDictionary<string, Rs232FlowControl> _flowControls = new Dictionary<string, Rs232FlowControl>()
        {
            {Localizer.Get(NoFlowControlKey), Rs232FlowControl.NoControl},
            {Localizer.Get(CtsRtsFlowControlKey), Rs232FlowControl.RtsCts},
            {Localizer.Get(XonXoffFlowControlKey), Rs232FlowControl.XonXoff}
        };

        private const string NoStopBitsKey = "Zerial_No_Stop_Bits";
        private const string OneStopBitsKey = "Zerial_One_Stop_Bits";
        private const string OneAndHalfStopBitsKey = "Zerial_One_And_Half_Stop_Bits";
        private const string TwoStopBitsKey = "Zerial_Two_Stop_Bits";
        
        private const string NoParityKey = "Zerial_No_Parity";
        private const string MarkParityKey = "Zerial_Mark_Parity";
        private const string SpaceParityKey = "Zerial_Space_Parity";
        private const string EvenParityKey = "Zerial_Even_Parity";
        private const string OddParityKey = "Zerial_Odd_Parity";
        
        private const string NoFlowControlKey = "Zerial_No_Flow_Control";
        private const string CtsRtsFlowControlKey = "Zerial_RTS_CTS_Flow_Control";
        private const string XonXoffFlowControlKey = "Zerial_Xon_Xoff_Flow_Control";
        
        public const string DefaultXon = "Ox11";
        public const string DefaultXoff = "Ox13";
    }
}