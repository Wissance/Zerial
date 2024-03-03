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
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    int portNumber = 0;
                    bool portParseResult = int.TryParse(deviceName.Substring(WindowsComDeviceNameSign.Length),
                        out portNumber);
                    if (portParseResult)
                        return portNumber;
                    return -1;
                }

                if (OperatingSystem.IsLinux())
                {
                    string deviceNumber = "";
                    // name is /dev/ttyUSB{N}, where N - number 
                    if (deviceName.Contains(LinuxUsbDeviceNameSign))
                    {
                        int nameSignStartIndex = deviceName.IndexOf(LinuxUsbDeviceNameSign);
                        deviceNumber = deviceName.Substring(nameSignStartIndex + LinuxUsbDeviceNameSign.Length);
                    }
                    else
                    {
                        if (deviceName.Contains(LinuxComDeviceNameSign))
                        {
                            int nameSignStartIndex = deviceName.IndexOf(LinuxComDeviceNameSign);
                            deviceNumber = deviceName.Substring(nameSignStartIndex + LinuxComDeviceNameSign.Length);
                        }
                    }
                    
                    int portNumber = 0;
                    bool portParseResult = int.TryParse(deviceName.Substring(WindowsComDeviceNameSign.Length),
                        out portNumber);
                    if (portParseResult)
                        return portNumber;
                    return -1;

                }

                return -2;
            }
            catch (Exception)
            {
                return -255;
            }
        }

        private const string WindowsComDeviceNameSign = "COM";
        private const string LinuxComDeviceNameSign = "tty";
        private const string LinuxUsbDeviceNameSign = "ttyUSB";
        private const string WindowsComPortNamePattern = $"{WindowsComDeviceNameSign}{{0}}";
        private const string LinuxSerialDeviceNamePattern = "/dev/{0}";
        private const string LinuxComDeviceNamePattern = $"{LinuxComDeviceNameSign}{{0}}";
        private const string LinuxUsbDeviceNamePattern = $"{LinuxUsbDeviceNameSign}{{0}}";
    }
}