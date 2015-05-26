using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using log4net;

namespace GRG.LeisureCards.WebAPI
{
    /// <summary>
    /// A very simple scheduler that will suffice for the basic requirement.
    /// </summary>
    public class DailyTaskScheduler
    {
        public static readonly DailyTaskScheduler Instance = new DailyTaskScheduler();

        private static readonly ILog Log = LogManager.GetLogger(typeof(DailyTaskScheduler));
        
        private readonly Timer _timer;
        private readonly object _elapsedlock = new object();
        private readonly List<Action> _executedToday = new List<Action>();
        private readonly Dictionary<Action, int> _tasks = new Dictionary<Action, int>();

        private int _lastElapseDay = -1;
        
        private DailyTaskScheduler()
        {
            _timer = new Timer(30000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_elapsedlock)
            {
                var now = DateTime.Now;
                if (_lastElapseDay != now.Day)
                {
                    _executedToday.Clear();
                    _lastElapseDay = now.Day;
                }

                foreach (var kvp in _tasks.Where(kvp => kvp.Value <= ((now.Hour*60) + now.Minute) && !_executedToday.ToArray().Contains(kvp.Key)))
                {
                    try
                    {
                        kvp.Key();
                        _executedToday.Add(kvp.Key);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error executing scheduled task", ex);
                    }
                }
            }
        }

        public void ScheduleTask(Action action, int timeOfDayMinutes)
        {
            _tasks.Add(action, timeOfDayMinutes);
        }
    }
}