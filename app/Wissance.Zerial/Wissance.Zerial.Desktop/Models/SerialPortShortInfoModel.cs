namespace Wissance.Zerial.Desktop.Models
{
    public class SerialPortShortInfoModel
    {
        public SerialPortShortInfoModel(int portNumber, string configuration)
        {
            PortNumber = portNumber;
            Configuration = configuration;
        }

        public int PortNumber { get; set; }
        public string Configuration { get; set; }
    }
    
}