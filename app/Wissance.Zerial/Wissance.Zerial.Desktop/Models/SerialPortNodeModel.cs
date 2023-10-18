namespace Wissance.Zerial.Desktop.Models
{
    public class SerialPortNodeModel
    {
        public SerialPortNodeModel(int portNumber, string configuration)
        {
            PortNumber = portNumber;
            Configuration = configuration;
        }

        public int PortNumber { get; set; }
        public string Configuration { get; set; }
    }
    
}