using System.Collections.Concurrent;
using System.IO.Ports;
using Wissance.Zerial.Common.Rs232.Settings;

namespace Wissance.Zerial.Common.Rs232.Managers
{
    public class MultiDeviceRs232Manager : IRs232DeviceManager, IDisposable
    {
        public MultiDeviceRs232Manager(/*LoggerFactory loggerFactory*/)
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        
        public async Task<bool> OpenAsync(Rs232Settings settings)
        {
            try
            {
                // todo(umv): run timeout parallel to open task
                SerialPort serialPort = new SerialPort()
                {
                    PortName = $"COM{settings.PortNumber}",
                    BaudRate = (int)settings.BaudRate,
                    StopBits = _stopBitsMapping[settings.StopBits],
                    DataBits = settings.ByteLength,
                    Parity = _parityMapping[settings.Parity],
                    ReadBufferSize = 64,
                    WriteBufferSize = 64,
                    Handshake = _flowControlMapping[settings.FlowControl]
                };
                
                // todo(UMV): add Xon + Xoff symbols data

                serialPort.Open();
                return true;
            }
            catch (Exception e)
            {
                // todo(umv) :add logging
                return false;
            }
        }

        public Task<bool> CloseAsync(int portNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteAsync(int portNumber, byte[] data)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ReadAsync(int portNumber)
        {
            throw new NotImplementedException();
        }

        private IDictionary<int, SerialPort> _devices = new ConcurrentDictionary<int, SerialPort>();

        private readonly IDictionary<Rs232StopBits, StopBits> _stopBitsMapping = new Dictionary<Rs232StopBits, StopBits>()
            {
                { Rs232StopBits.None, StopBits.None },
                { Rs232StopBits.One, StopBits.One },
                { Rs232StopBits.OneAndHalf, StopBits.OnePointFive },
                { Rs232StopBits.Two, StopBits.Two }
            };

        private readonly IDictionary<Rs232Parity, Parity> _parityMapping = new Dictionary<Rs232Parity, Parity>()
        {
            { Rs232Parity.NoParity, Parity.None },
            { Rs232Parity.NoParity, Parity.None },
            { Rs232Parity.NoParity, Parity.None },
            { Rs232Parity.NoParity, Parity.None },
            { Rs232Parity.NoParity, Parity.None }
        };

        private readonly IDictionary<Rs232FlowControl, Handshake> _flowControlMapping = new Dictionary<Rs232FlowControl, Handshake>()
        {
            { Rs232FlowControl.NoControl , Handshake.None },
            { Rs232FlowControl.XonXoff , Handshake.XOnXOff },
            { Rs232FlowControl.RtsCts , Handshake.RequestToSend }
        };
    }
}