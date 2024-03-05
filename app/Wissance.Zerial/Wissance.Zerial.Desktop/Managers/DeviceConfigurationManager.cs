using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
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
            try
            {
                if (!File.Exists(configFile))
                    File.Create(configFile).Dispose();
                _configFile = Path.GetFullPath(configFile);
            }
            catch (Exception e)
            {
                if (OperatingSystem.IsLinux())
                {
                    string deviceConfigFile = Path.Combine(Environment.SpecialFolder.UserProfile.ToString(), configFile);
                    if (!File.Exists(deviceConfigFile))
                        File.Create(deviceConfigFile).Dispose();
                    _configFile = Path.GetFullPath(deviceConfigFile);
                }
            }
            
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