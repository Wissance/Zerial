using Wissance.Zerial.Common.Rs232.Settings;

namespace Wissance.Zerial.Common.Rs232.Managers
{
    public interface IRs232DeviceManager : IDisposable
    {
        Task<bool> OpenAsync(Rs232Settings settings);
        Task<bool> CloseAsync(string deviceName);
        Task<bool> WriteAsync(string deviceName, byte[] data);
        Task<byte[]> ReadAsync(string deviceName);
    }
}