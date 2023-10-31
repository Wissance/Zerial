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
            _cancellationSource.Cancel();
            foreach (KeyValuePair<int,SerialPort> device in _devices)
            {
                device.Value.Dispose();
            }
        }
        
        public async Task<bool> OpenAsync(Rs232Settings settings)
        {
            try
            {
                SerialPort serialPort = _devices[settings.PortNumber];
                
                if (serialPort == null)
                {
                    // todo(umv): run timeout parallel to open task
                    serialPort = new SerialPort()
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
                    
                }
                
                // todo(UMV): add Xon + Xoff symbols data
                if (settings.FlowControl == Rs232FlowControl.XonXoff)
                {
                    // ?
                }

                _devices[settings.PortNumber] = serialPort;
                
                Task openTask = new Task(async _ =>
                {
                    _devices[settings.PortNumber].Open();
                }, 
                    _cancellationSource.Token);
                Task delayTask = Task.Delay(DefaultOperationTimeout);
                Task.WaitAny(new Task[] { openTask, delayTask });
                
                // todo(UMV): add OnReceive handler
                if (openTask.Status != TaskStatus.RanToCompletion || openTask.Exception != null)
                {
                    // todo(UMV): log exception
                }

                return true;
            }
            catch (Exception e)
            {
                // todo(umv): add logging
                return false;
            }
        }

        public async Task<bool> CloseAsync(int portNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> WriteAsync(int portNumber, byte[] data)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> ReadAsync(int portNumber)
        {
            throw new NotImplementedException();
        }

        private const int DefaultOperationTimeout = 2000;

        private readonly IDictionary<int, SerialPort> _devices = new ConcurrentDictionary<int, SerialPort>();

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
            { Rs232Parity.Mark, Parity.Mark },
            { Rs232Parity.Space, Parity.Space },
            { Rs232Parity.Even, Parity.Even },
            { Rs232Parity.Odd, Parity.Odd }
        };

        private readonly IDictionary<Rs232FlowControl, Handshake> _flowControlMapping = new Dictionary<Rs232FlowControl, Handshake>()
        {
            { Rs232FlowControl.NoControl , Handshake.None },
            { Rs232FlowControl.XonXoff , Handshake.XOnXOff },
            { Rs232FlowControl.RtsCts , Handshake.RequestToSend }
        };

        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();
    }
}