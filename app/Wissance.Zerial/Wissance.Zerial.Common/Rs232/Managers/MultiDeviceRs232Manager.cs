using System.Collections.Concurrent;
using System.IO.Ports;
using Wissance.Zerial.Common.Rs232.Settings;

namespace Wissance.Zerial.Common.Rs232.Managers
{
    public class MultiDeviceRs232Manager : IRs232DeviceManager, IDisposable
    {
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        
        public Task<bool> OpenAsync(Rs232Settings settings)
        {
            //SerialPort sp = new SerialPort()
            throw new NotImplementedException();
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
    }
}