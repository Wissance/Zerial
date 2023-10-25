using System.IO.Ports;

namespace Wissance.Zerial.Common.Rs232.Tools
{
    public static class Rs232PortsEnumerator
    {
        public static IList<string> GetAvailablePorts()
        {
            return SerialPort.GetPortNames().ToList();
        }
    }
}