using Wissance.Zerial.Common.Utils;

namespace Wissance.Zerial.Common.Tests.Utils
{
    public class TestSerialPortNameHelper
    {
        [TestCase(true, 4, "COM4", true)]
        [TestCase(false, 1, "/dev/tty1", false)]
        [TestCase(false, 2, "/dev/ttyUSB2", true)]
        public void TestBuildSerialDeviceName(bool isWindows, int portNumber, string expectedDeviceName, bool isUsb)
        {
            if (OperatingSystem.IsWindows() && isWindows || OperatingSystem.IsLinux() && !isWindows)
                Assert.That(SerialDeviceHelper.BuildSerialDeviceName(portNumber, isUsb), Is.EqualTo(expectedDeviceName));
        }
        
        [TestCase(true, "COM4",4)]
        [TestCase(false, "/dev/tty1",1)]
        [TestCase(false, "/dev/ttyUSB2",2)]
        public void TestGetSerialDeviceNumber(bool isWindows, string deviceName, int expectedPortNumber)
        {
            if (OperatingSystem.IsWindows() && isWindows || OperatingSystem.IsLinux() && !isWindows)
                Assert.That(SerialDeviceHelper.GetSerialDeviceNumber(deviceName), Is.EqualTo(expectedPortNumber));
        }
    }
}