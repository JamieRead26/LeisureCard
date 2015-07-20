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
        private int _lastExecutedDay=0;
        private readonly Dictionary<Action, int> _tasks = new Dictionary<Action, int>();

        private DailyTaskScheduler()
        {
            _timer = new Timer(30000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var day = DateTime.Now.Day;
            if (_lastExecutedDay==day)
                return;

            lock (_elapsedlock)
            {
                var now = DateTime.Now;
               
                foreach (var kvp in _tasks.Where(kvp => kvp.Value <= ((now.Hour*60) + now.Minute)))
                {
                    try
                    {
                        Log.Info("Executing scheduled task");

                        kvp.Key();
                        _lastExecutedDay = day;

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