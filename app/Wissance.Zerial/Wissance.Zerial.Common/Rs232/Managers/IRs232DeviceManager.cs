using Wissance.Zerial.Common.Rs232.Settings;

namespace Wissance.Zerial.Common.Rs232.Managers
{
    public interface IRs232DeviceManager
    {
        Task<bool> OpenAsync(Rs232Settings settings);
        Task<bool> CloseAsync(int portNumber);
        Task<bool> WriteAsync(int portNumber, byte[] data);
        Task<byte[]> ReadAsync(int portNumber);
    }
}