using System;
using System.Text.Json.Serialization;
using Avalonia.Media.Imaging;
using Wissance.Zerial.Desktop.Helpers;

namespace Wissance.Zerial.Desktop.Models
{
    public class SerialPortShortInfoModel
    {
        public SerialPortShortInfoModel(bool connected,string deviceName, string configuration)
        {
            Connected = connected;
            DeviceName = deviceName;
            Configuration = configuration;
            Id = Guid.NewGuid();
        }

        [JsonIgnore]
        public bool Connected { get; set; }

        [JsonIgnore]
        public Bitmap? ConnectStateImage
        {
            get
            {
                if (Connected)
                    return ImageLoaderHelper.LoadFromResource(new Uri("avares://Wissance.Zerial.Desktop/Assets/Images/Device_Is_On_16x16.png"));
                return ImageLoaderHelper.LoadFromResource(new Uri("avares://Wissance.Zerial.Desktop/Assets/Images/Device_Is_Off_16x16.png"));
            }
        }
        [JsonIgnore]
        public Guid Id { get; }
        public string DeviceName { get; set; }
        public string Configuration { get; set; }
    }
    
}