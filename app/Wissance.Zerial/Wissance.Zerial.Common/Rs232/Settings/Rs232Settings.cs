namespace Wissance.Zerial.Common.Rs232.Settings
{
    public enum Rs232BaudRate
    {
        BaudMode9600 = 9600,
        BaudMode19200 = 19200,
        BaudMode38400 = 38400,
        BaudMode57600 = 57600,
        BaudMode115200 = 115200
    }

    public enum Rs232StopBits
    {
        One = 1,
        OneAndHalf = 2,
        Two = 3,
        None = 4
    }

    public enum Rs232Parity
    {
        NoParity = 1,
        Mark = 2,
        Space = 3,
        Even = 4,
        Odd = 5
    }

    public enum Rs232FlowControl
    {
        NoControl = 1,
        RtsCts = 2,
        XonXoff = 3
    }

    public class Rs232Settings
    {
        [Obsolete("Should be used DeviceName instead of PortNumber")]
        public int PortNumber { get; set; }
        public string DeviceName { get; set; }
        public Rs232BaudRate BaudRate { get; set; }
        public Rs232StopBits StopBits { get; set; }
        public Rs232Parity Parity { get; set; }
        public int ByteLength { get; set; }
        public Rs232FlowControl FlowControl { get; set; }
        public byte Xon { get; set; }
        public byte Xoff { get; set; }
    }
}