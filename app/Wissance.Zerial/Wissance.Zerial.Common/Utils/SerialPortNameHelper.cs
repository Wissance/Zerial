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
            string deviceName = string.Format(isUsbDevice ? LinuxUsbDeviceNamePattern : LinuxPureSerialDeviceNamePattern, number);
            return string.Format(LinuxPureSerialDeviceNamePattern, deviceName);
        }

        private const string WindowsComPortNamePattern = "COM{0}";
        private const string LinuxSerialDeviceNamePattern = "/dev/{0}";
        private const string LinuxPureSerialDeviceNamePattern = "tty{0}";
        private const string LinuxUsbDeviceNamePattern = "ttyUSB{0}";
    }
}