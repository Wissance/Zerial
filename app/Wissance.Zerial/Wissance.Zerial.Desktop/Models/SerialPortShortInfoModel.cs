using System;
using Avalonia.Media.Imaging;
using Wissance.Zerial.Desktop.Helpers;

namespace Wissance.Zerial.Desktop.Models
{
    public class SerialPortShortInfoModel
    {
        public SerialPortShortInfoModel(bool connected, int portNumber, string configuration)
        {
            Connected = connected;
            PortNumber = portNumber;
            Configuration = configuration;
        }

        public bool Connected { get; set; }

        public Bitmap? ConnectStateImage
        {
            get
            {
                if (Connected)
                    return ImageLoaderHelper.LoadFromResource(new Uri("avares://Wissance.Zerial.Desktop/Assets/Images/Device_Is_On_16x16.png"));
                return ImageLoaderHelper.LoadFromResource(new Uri("avares://Wissance.Zerial.Desktop/Assets/Images/Device_Is_Off_16x16.png"));
            }
        }
        public int PortNumber { get; set; }
        public string Configuration { get; set; }
    }
    
}