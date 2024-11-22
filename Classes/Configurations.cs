using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Proxmox_Desktop_Client.Classes
{
    public class Configurations
    {
        private Dictionary<string, object> settings;
        private string filePath;
        public  string appDataFolder;
        private string appName = "ProxmoxDesktopClient";

        public Configurations()
        {
            settings = new Dictionary<string, object>();
            appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName);
            Directory.CreateDirectory(appDataFolder); // Ensure the directory exists
            filePath = Path.Combine(appDataFolder, "settings.json");
            LoadFromFile();
        }

        public void SetSetting(string key, object value)
        {
            settings[key] = value;
            SaveToFile();
        }

        public object GetSetting(string key)
        {
            settings.TryGetValue(key, out var value);
            return value;
        }

        private void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void LoadFromFile()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
            }
        }
    }
}
