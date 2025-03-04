using System.Collections.Generic;
using System.IO;

namespace TimerApp
{
    public static class SoundManager
    {
        private static string soundsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Sounds");

        public static List<string> GetAvailableSounds()
        {
            if (!Directory.Exists(soundsFolderPath))
            {
                Directory.CreateDirectory(soundsFolderPath);
                return new List<string>();
            }

            var soundFiles = Directory.GetFiles(soundsFolderPath, "*.*")
                                      .Where(file => file.EndsWith(".mp3") || file.EndsWith(".wav"))
                                      .Select(Path.GetFileName)
                                      .ToList();

            return soundFiles;
        }
    }
}