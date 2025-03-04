using System;
using System.Windows.Threading;

namespace TimerApp
{
    public class TimerManager
    {
        private TimeSpan remainingTime;
        private bool isRunning = false;
        private DispatcherTimer timer;

        public event Action<TimeSpan> OnTimeUpdated;
        public event Action OnTimerCompleted;

        public TimerManager()
        {
            remainingTime = TimeSpan.FromMinutes(30);
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingTime.TotalSeconds <= 0)
            {
                timer.Stop();
                isRunning = false;
                remainingTime = TimeSpan.Zero; // Устанавливаем 0, чтобы не уйти в минус
                OnTimerCompleted?.Invoke();
                OnTimeUpdated?.Invoke(remainingTime);
                return;
            }

            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
            OnTimeUpdated?.Invoke(remainingTime);
        }

        public void Start()
        {
            if (!isRunning)
            {
                timer.Start();
                isRunning = true;
            }
        }

        public void Pause()
        {
            if (isRunning)
            {
                timer.Stop();
                isRunning = false;
            }
        }

        public void Reset()
        {
            timer.Stop();
            isRunning = false;
            remainingTime = TimeSpan.FromMinutes(30);
            OnTimeUpdated?.Invoke(remainingTime);
        }

        public void AddTenMinutes()
        {
            remainingTime = remainingTime.Add(TimeSpan.FromMinutes(10));
            OnTimeUpdated?.Invoke(remainingTime);
        }

        public TimeSpan GetRemainingTime()
        {
            return remainingTime;
        }

        public bool IsRunning()
        {
            return isRunning;
        }
    }
}