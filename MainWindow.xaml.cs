using System;
using System.Windows;
using TimerApp;

namespace TimerApp // Убедитесь, что пространство имен совпадает с XAML
{
    public partial class MainWindow : Window
    {
        private TimerManager timerManager;

        public MainWindow()
        {
            InitializeComponent(); // Этот метод генерируется из XAML
            timerManager = new TimerManager();
            UpdateTimerDisplay(timerManager.GetRemainingTime());

            // Подписываемся на события
            timerManager.OnTimeUpdated += time => UpdateTimerDisplay(time);
            timerManager.OnTimerCompleted += () => MessageBox.Show("Время вышло!");
        }

        private void UpdateTimerDisplay(TimeSpan time)
        {
            TimerLabel.Text = time.ToString(@"hh\:mm\:ss");
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timerManager.Start();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            timerManager.Pause();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            timerManager.Reset();
        }

        private void AddTenMinutes_Click(object sender, RoutedEventArgs e)
        {
            timerManager.AddTenMinutes();
        }

        private void OpenWidget_Click(object sender, RoutedEventArgs e)
        {
            var widget = new WidgetWindow(timerManager); // Передаем TimerManager
            widget.Show();
        }

        // Добавляем отсутствующие обработчики из XAML
        private void HideToTray_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; // Пример: сворачивание окна
            // Логика для сворачивания в трей может быть добавлена позже
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Закрытие приложения
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Настройки пока не реализованы.");
            // Здесь можно добавить открытие окна настроек
        }
    }
}