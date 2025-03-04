using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TimerApp
{
    public partial class WidgetWindow : Window
    {
        private readonly TimerManager timerManager;
        private const double CircleRadius = 100; // Радиус круга для расчета дуги

        public WidgetWindow(TimerManager manager)
        {
            InitializeComponent();
            timerManager = manager ?? throw new ArgumentNullException(nameof(manager));

            // Подписываемся на события TimerManager
            timerManager.OnTimeUpdated += UpdateTimerDisplayAndProgress;
            timerManager.OnTimerCompleted += () => MessageBox.Show("Время вышло!");

            // Устанавливаем начальное значение
            UpdateTimerDisplayAndProgress(timerManager.GetRemainingTime());
        }

        private void UpdateTimerDisplayAndProgress(TimeSpan time)
        {
            TimerLabel.Text = time.ToString(@"hh\:mm\:ss");

            // Обновляем прогресс ползунка
            double totalSeconds = TimeSpan.FromMinutes(30).TotalSeconds; // Общее время в секундах
            double remainingSeconds = time.TotalSeconds;
            double progress = remainingSeconds / totalSeconds; // Доля оставшегося времени (0..1)

            AnimateProgressArc(progress);
        }

        private void AnimateProgressArc(double progress)
        {
            // Угол в радианах для дуги (полный круг = 2π)
            double angle = 2 * Math.PI * progress;
            double x = CircleRadius * Math.Cos(angle - Math.PI / 2); // Смещение для начала от 12 часов
            double y = CircleRadius * Math.Sin(angle - Math.PI / 2);

            // Создаем новую геометрию для дуги
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = new Point(CircleRadius, 0), // Начало в центре сверху
                IsClosed = false
            };
            ArcSegment arc = new ArcSegment
            {
                Point = new Point(CircleRadius + x, CircleRadius + y),
                Size = new Size(CircleRadius, CircleRadius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = progress > 0.5
            };
            figure.Segments.Add(arc);
            geometry.Figures.Add(figure);

            ProgressArc.Data = geometry;

            // Анимация (опционально, для плавного изменения)
            DoubleAnimation animation = new DoubleAnimation
            {
                From = (ProgressArc.Data as PathGeometry)?.Figures[0].Segments[0] is ArcSegment ? 0 : 1,
                To = progress,
                Duration = TimeSpan.FromSeconds(0.5), // Плавность анимации
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTarget(animation, ProgressArc);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Path.Data).(PathGeometry.Figures)[0].(PathFigure.Segments)[0].(ArcSegment.Point)"));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        // Остальные методы остаются прежними
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

        private void HideWidget_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private void CloseWidget_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WidgetWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ResizeBottomRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.ResizeMode = ResizeMode.CanResize;
                this.WindowState = WindowState.Normal;
                this.DragResizeWindow(ResizeDirection.BottomRight);
            }
        }

        private void DragResizeWindow(ResizeDirection direction)
        {
            switch (direction)
            {
                case ResizeDirection.BottomRight:
                    this.Cursor = Cursors.SizeNWSE;
                    break;
            }
        }
    }

    public enum ResizeDirection
    {
        BottomRight
    }
}