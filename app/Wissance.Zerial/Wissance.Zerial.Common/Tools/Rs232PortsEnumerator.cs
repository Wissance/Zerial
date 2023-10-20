

using System.IO.Ports;

namespace Wissance.Zerial.Common.Tools
{
    public static class Rs232PortsEnumerator
    {
        public static IList<string> GetAvailablePorts()
        {
            return SerialPort.GetPortNames().ToList();
        }
    }
}