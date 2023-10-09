namespace Wissance.Zerial.Desktop.Data
{
    public class SerialPortNode
    {
        public SerialPortNode(int portNumber, string configuration)
        {
            PortNumber = portNumber;
            Configuration = configuration;
        }

        public int PortNumber { get; set; }
        public string Configuration { get; set; }
    }
    
}