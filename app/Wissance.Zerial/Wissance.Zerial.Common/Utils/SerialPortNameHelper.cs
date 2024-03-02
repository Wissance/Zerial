namespace Wissance.Zerial.Common.Utils
{
    public static class SerialDeviceHelper
    {
        /// <summary>
        /// Constructs name depends on Operation System.
        /// For Windows 
        /// </summary>
        /// <param name="number">
        ///    In Windows name is COM4, COM5, therefore number is 4, 5
        ///    In Linux device name is /dev/tty0 for real COM, /dev/ttyUSB0 for USB-COM, therefore number is 0
        /// </param>
        /// <param name="isUsbDevice">
        ///    Don't have any sense for Windows, but have for Linux
        /// </param>
        public static string BuildSerialDeviceName(int number, bool isUsbDevice = true)
        {
            if (OperatingSystem.IsWindows())
                return string.Format(WindowsComPortNamePattern, number);
            if (OperatingSystem.IsLinux())
            {
                string deviceName = string.Format(isUsbDevice ? LinuxUsbDeviceNamePattern : LinuxComDeviceNamePattern, number);
                return string.Format(LinuxSerialDeviceNamePattern, deviceName);
            }

            return null;
        }

        public static int GetSerialDeviceNumber(string deviceName)
        {
            if (OperatingSystem.IsWindows())
            {
                int portNumber = 0;
                bool portParseResult = int.TryParse(deviceName.Substring("COM".Length), out portNumber);
                if (portParseResult)
                    return portNumber;
                return -1;
            }

            if (OperatingSystem.IsLinux())
            {
                if (deviceName.Contains("USB"))
                {
                }
            }

            return -1;
        }

        private const string WindowsComPortNamePattern = "COM{0}";
        private const string LinuxSerialDeviceNamePattern = "/dev/{0}";
        private const string LinuxComDeviceNamePattern = "tty{0}";
        private const string LinuxUsbDeviceNamePattern = "ttyUSB{0}";
    }
}