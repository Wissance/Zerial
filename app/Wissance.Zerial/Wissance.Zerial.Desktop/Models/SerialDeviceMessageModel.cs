using System;

namespace Wissance.Zerial.Desktop.Models
{
    public enum Direction
    {
        In,  // from Device to PC
        Out  // from PC to Device
    }

    public class SerialDeviceMessageModel
    {
        public DateTime Time { get; set; }
        public Direction Direction { get; set; }
        public byte[] Message { get; set; }
    }
}