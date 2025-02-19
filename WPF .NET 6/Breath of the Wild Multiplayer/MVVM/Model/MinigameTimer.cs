using Breath_of_the_Wild_Multiplayer.Source_files;
using System;
using System.Windows.Threading;
using System.Windows;
using System.Diagnostics;

namespace Breath_of_the_Wild_Multiplayer.MVVM.Model
{
    public class MinigameTimer : ObservableObject
    {
        public enum CountModeEnum : byte
        {
            Countup = 0,
            Countdown = 1
        }

        public string TimerText
        {
            get
            {
                return ElapsedTime.ToString(@"mm\:ss\.ff");
            }
        }

        public Visibility Visible
        {
            get
            {
                return IsRunning ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public TimeSpan ElapsedTime = TimeSpan.Zero;
        public bool IsRunning = false;

        private CountModeEnum CountMode;
        private TimeSpan StartTime = TimeSpan.Zero;
        private TimeSpan MaxTime = TimeSpan.MinValue;
        private DispatcherTimer Timer;
        private Stopwatch Stopwatch;

        public MinigameTimer()
        {
            Timer = new DispatcherTimer(DispatcherPriority.Input);
            Stopwatch = new Stopwatch();
            Timer.Interval = TimeSpan.FromMilliseconds(51); // I know you are here. And you are wondering why this is 51. Just move along
            Timer.Tick += new EventHandler(Timer_Tick);
        }

        public void Start(CountModeEnum countMode, TimeSpan startTime, TimeSpan maxTime)
        {
            this.CountMode = countMode;
            this.StartTime = startTime;
            this.ElapsedTime = startTime;
            this.MaxTime = maxTime == startTime ? TimeSpan.MinValue : maxTime;
            Timer.Start();
            Stopwatch.Restart();
            IsRunning = true;
            OnPropertyChanged(nameof(Visible));
        }

        public void Stop()
        {
            Timer.Stop();
            Stopwatch.Stop();
            IsRunning = false;
            OnPropertyChanged(nameof(Visible));
        }

        public void Restart()
        {
            ElapsedTime = StartTime;
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            if (CountMode == CountModeEnum.Countup)
            {
                //this.ElapsedTime += TimeSpan.FromMilliseconds(51);
                this.ElapsedTime = StartTime + Stopwatch.Elapsed;

                if (this.MaxTime != TimeSpan.MinValue && this.ElapsedTime > this.MaxTime)
                {
                    this.Stop();
                }
            }
            else
            {
                //this.ElapsedTime -= TimeSpan.FromMilliseconds(51);
                this.ElapsedTime = StartTime - Stopwatch.Elapsed;

                if (this.MaxTime != TimeSpan.MinValue && this.ElapsedTime < this.MaxTime)
                {
                    this.Stop();
                }
            }

            OnPropertyChanged(nameof(TimerText));
            OnPropertyChanged(nameof(Visible));
        }
    }
}
