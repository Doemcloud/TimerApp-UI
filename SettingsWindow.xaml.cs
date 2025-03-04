using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace TimerApp
{
    public partial class SettingsWindow : Window
    {
        private Settings settings;

        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
            PopulateSoundComboBoxes();
        }

        private void LoadSettings()
        {
            settings = SettingsManager.LoadSettings();
            EndSoundComboBox.SelectedItem = settings.EndSound;
            WarningSoundComboBox.SelectedItem = settings.WarningSound;
            WarningTimeTextBox.Text = settings.WarningTime.ToString();
        }

        private void PopulateSoundComboBoxes()
        {
            var availableSounds = SoundManager.GetAvailableSounds();
            EndSoundComboBox.ItemsSource = availableSounds;
            WarningSoundComboBox.ItemsSource = availableSounds;

            if (availableSounds.Count > 0)
            {
                EndSoundComboBox.SelectedItem = settings.EndSound;
                WarningSoundComboBox.SelectedItem = settings.WarningSound;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            settings.EndSound = EndSoundComboBox.SelectedItem?.ToString();
            settings.WarningSound = WarningSoundComboBox.SelectedItem?.ToString();
            settings.WarningTime = int.TryParse(WarningTimeTextBox.Text, out int warningTime) ? warningTime : 10;

            SettingsManager.SaveSettings(settings);
            MessageBox.Show("Настройки сохранены!");
        }

        private void PreviewEndSound_Click(object sender, RoutedEventArgs e)
        {
            PlaySound(EndSoundComboBox.SelectedItem?.ToString());
        }

        private void PreviewWarningSound_Click(object sender, RoutedEventArgs e)
        {
            PlaySound(WarningSoundComboBox.SelectedItem?.ToString());
        }

        private void PlaySound(string soundFileName)
        {
            if (string.IsNullOrEmpty(soundFileName))
            {
                MessageBox.Show("Выберите звук для прослушивания.");
                return;
            }

            string soundPath = Path.Combine(Directory.GetCurrentDirectory(), "Sounds", soundFileName);
            if (File.Exists(soundPath))
            {
                var player = new MediaPlayer();
                player.Open(new Uri(soundPath));
                player.Play();
            }
            else
            {
                MessageBox.Show("Файл звука не найден.");
            }
        }
    }
}