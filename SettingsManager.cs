using Newtonsoft.Json;
using System.IO;

namespace TimerApp
{
    public class SettingsManager
    {
        private static string settingsFilePath = "Settings.json";

        public static Settings LoadSettings()
        {
            if (!File.Exists(settingsFilePath))
            {
                return new Settings
                {
                    WarningTime = 10,
                    EndSound = "default_end_sound.mp3",
                    WarningSound = "default_warning_sound.mp3" 
                };
            }

            string json = File.ReadAllText(settingsFilePath);
            return JsonConvert.DeserializeObject<Settings>(json);
        }

        public static void SaveSettings(Settings settings)
        {
            string json = JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(settingsFilePath, json);
        }
    }

    public class Settings
    {
        public int WarningTime { get; set; }
        public string EndSound { get; set; }
        public string WarningSound { get; set; }
    }
}