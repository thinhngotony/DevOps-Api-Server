using System;
using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace Vjp.Rfid.SmartShelf.Helper
{
    public  class AlarmTimer : IDisposable
    {
        private Timer timer = new Timer();

        public Stopwatch _stpWatch = new Stopwatch();
        /// <summary>
        /// Event every time changed
        /// </summary>
        public Action TimeChanged;  
        /// <summary>
        /// Event fire when count down timer finished
        /// </summary>
        public Action CountDownFinished;
        /// <summary>
        /// timer is running or not
        /// </summary>
        public bool IsRunning => timer.Enabled;
        
        private int StepMs
        {
            get=> timer.Interval;
            set => timer.Interval = value;  
        }

        private TimeSpan _max = TimeSpan.FromMilliseconds(0);
        public TimeSpan TimeLeft => (_max.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) > 0 ? TimeSpan.FromMilliseconds(_max.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) : TimeSpan.FromMilliseconds(0);
        private bool _mustStop => (_max.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) < 0;
        public string TimeLeftStr => TimeLeft.ToString(@"\mm\:ss");
        public string TimeLeftMsStr => TimeLeft.ToString(@"mm\:ss\.fff");

        private void TimerTick(object sender , EventArgs e)
        {
            TimeChanged?.Invoke();
            if (_mustStop)
            {
                CountDownFinished?.Invoke();
                _stpWatch.Stop();
                timer.Enabled = false;
            }
        }

        public AlarmTimer(int miliSeconds)
        {
            SetTime(miliSeconds);
            Init();
        }

        public AlarmTimer()
        {
            Init();
        }
        private void Init()
        {
            StepMs = 100;
            timer.Tick += new EventHandler(TimerTick);

        }
        public void SetTime(TimeSpan ts)
        {
            _max = ts;
            TimeChanged?.Invoke();
        }
        public void SetTime(int miliSeconds) =>SetTime(TimeSpan.FromMilliseconds(miliSeconds));
    
        public void Start()
        {
            timer.Start();
            _stpWatch.Start();
        }
        public void Pause()
        {
            timer.Stop();
            _stpWatch.Stop();
        }
        public void Stop()
        {
            Reset();
            Pause();
            timer.Enabled = false;
            CountDownFinished = null;
            TimeChanged = null;

        }
        public void Reset()
        {
            _stpWatch.Reset();
        }
        public void Restart()
        {
            _stpWatch.Reset();
            timer.Start();
        }

        public void Dispose() => timer.Dispose();
    }
}
