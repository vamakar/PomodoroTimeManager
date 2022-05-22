using System;
using System.Windows.Threading;

namespace PomodoroTimeManager.States
{
    internal class Ticking : IState
    {
        private MainWindow Window;

        public Ticking(MainWindow window)
        {
            Window = window;
        }

        public void Start(DispatcherTimer timer)
        {
        }

        public void Pause(DispatcherTimer timer)
        {
            timer.Stop();
            Window.AddMinute();
            Window.SetTimerTable();

            Window.State = Window.Paused;
        }

        public void Reset(DispatcherTimer timer)
        {
            timer.Stop();
            Window.InitTimer();

            Window.State = Window.Paused;
        }
    }
}
