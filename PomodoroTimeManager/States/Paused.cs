using System;
using System.Windows.Threading;

namespace PomodoroTimeManager.States
{
    internal class Paused : IState
    {
        private MainWindow Window;

        public Paused(MainWindow window)
        {
            Window = window;
        }

        public void Start(DispatcherTimer timer)
        {
            Window.StartTimer();

            Window.State = Window.Ticking;
        }

        public void Pause(DispatcherTimer timer)
        {
        }

        public void Reset(DispatcherTimer timer)
        {
            timer.Stop();
            Window.InitTimer();

            Window.State = Window.Paused;
        }
    }
}
