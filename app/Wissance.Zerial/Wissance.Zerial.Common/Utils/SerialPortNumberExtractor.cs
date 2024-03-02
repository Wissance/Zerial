namespace Wissance.Zerial.Common.Utils
{
    [Obsolete("Use Wissance.Zerial.Common.Utils.SerialPortNameHelper")]
    public static class SerialPortNumberExtractor
    {
        
        public static int Extract(string text)
        {
            int portNumber = 0;
            bool portParseResult = int.TryParse(text.Substring("COM".Length), out portNumber);
            if (portParseResult)
                return portNumber;
            return -1;
        }
    }
}