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
        public DeviceConfigurationManager(string env, string configFile)
        {
            if (env != Program.SnapEnvironmentKey)
            {
                bool result = PrepareDevConfigDirectory(configFile);
                if (!result)
                {
                    // probably we here in snap ...
                    string home = Environment.GetEnvironmentVariable("HOME");
                    if (home != null)
                    {
                        string deviceConfigFile = Path.Combine(home, configFile);
                        PrepareDevConfigDirectory(deviceConfigFile);
                    }
                }
            }
        }

        public ObservableCollection<SerialPortShortInfoModel> Load()
        {
            if (string.IsNullOrEmpty(_configFile))
                return new ObservableCollection<SerialPortShortInfoModel>();
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

        private bool PrepareDevConfigDirectory(string file)
        {
            try
            {
                if (!File.Exists(file))
                    File.Create(file).Dispose();
                _configFile = Path.GetFullPath(file);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string _configFile;
    }
}