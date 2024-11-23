using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Proxmox_Desktop_Client.Classes
{
    public class Configurations
    {
        private Dictionary<string, object> _settings;
        private readonly string _filePath;
        public readonly string AppDataFolder;
        private readonly string _appName = "ProxmoxDesktopClient";

        public Configurations()
        {
            _settings = new Dictionary<string, object>();
            AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appName);
            Directory.CreateDirectory(AppDataFolder); // Ensure the directory exists
            _filePath = Path.Combine(AppDataFolder, "settings.json");
            LoadFromFile();
        }

        public void SetSetting(string key, object value)
        {
            _settings[key] = value;
            SaveToFile();
        }

        public object GetSetting(string key)
        {
            _settings.TryGetValue(key, out var value);
            return value;
        }

        private void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private void LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
            }
        }
    }
}
