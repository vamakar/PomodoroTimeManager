using System.Windows.Threading;

namespace PomodoroTimeManager.States
{
    internal interface IState
    {
        void Start(DispatcherTimer timer);
        void Pause(DispatcherTimer timer);
        void Reset(DispatcherTimer timer);
    }
}
