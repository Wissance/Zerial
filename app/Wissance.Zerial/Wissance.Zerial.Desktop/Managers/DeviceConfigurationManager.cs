using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AvaloniaEdit.Utils;
using Wissance.Zerial.Desktop.Models;

namespace Wissance.Zerial.Desktop.Managers
{
    public class DeviceConfigurationManager
    {
        public DeviceConfigurationManager(string configFile)
        {
            _configFile = Path.GetFullPath(configFile);
        }

        public ObservableCollection<SerialPortShortInfoModel> Load()
        {
            string content = File.ReadAllText(_configFile);
            ObservableCollection<SerialPortShortInfoModel> devicesConfigs = new ObservableCollection<SerialPortShortInfoModel>();
            if (string.IsNullOrEmpty(content))
                return devicesConfigs;
            SerialPortShortInfoModel[] deserializedConfigs = JsonSerializer.Deserialize<SerialPortShortInfoModel[]>(content);
            devicesConfigs.AddRange(deserializedConfigs);
            return devicesConfigs;
        }

        public void Save(ObservableCollection<SerialPortShortInfoModel> devicesConfigs)
        {
            string content = JsonSerializer.Serialize(devicesConfigs);
            File.WriteAllText(_configFile, content);
        }

        private readonly string _configFile;
    }
}