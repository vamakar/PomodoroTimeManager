using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using PomodoroTimeManager.States;

namespace PomodoroTimeManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly TimeSpan _second = new TimeSpan(0, 0, 1);
        private readonly TimeSpan _minute = new TimeSpan(0, 1, 0);

        internal IState State { get; set; }
        internal IState Ticking { get; }
        internal IState Paused { get; }
        private DispatcherTimer Timer { get; set; }
        private NotifyIcon NIcon { get; set; }
        private DateTime End { get; set; }
        private TimeSpan DialTime { get; set; }
        private TimeSpan InitTime { get; } = new TimeSpan(0, 25, 0);
        private WindowState StoredWindowState { get; } = WindowState.Normal;

        public MainWindow()
        {
            InitializeComponent();
            Ticking = new Ticking(this);
            Paused = new Paused(this);

            InitIcon();
            InitTimer();
            State = Paused;
        }

        private void InitIcon()
        {
            NIcon = new NotifyIcon
            {
                BalloonTipText = "Time is over!",
                BalloonTipTitle = "Pomodoro",
                Text = "Pomodoro",
                Icon = new Icon(@"../../tomato.ico")
            };
            NIcon.Click += nIcon_Click;
            CheckTrayIcon();
        }

        internal void InitTimer()
        {
            DialTime = InitTime;
            SetTimerTable();

            Timer = new DispatcherTimer(DispatcherPriority.Render);
            Timer.Tick += CheckSecond;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        internal void SetTimerTable()
        {
            DialTextBox.Text = DialTime.ToString("mm\\:ss");
        }

        internal void StartTimer()
        {
            End = DateTime.Now.Add(_second);
            Timer.Start();
        }

        internal void AddMinute()
        {
            DialTime = DialTime.Add(_minute);

            if (DialTime > InitTime)
            {
                DialTime = InitTime;
            }
        }

        private void CheckSecond(object sender, EventArgs e)
        {
            if (DateTime.Now < End) return;

            DialTime = DialTime.Subtract(_second);
            SetTimerTable();

            if (DialTime.TotalSeconds <= 0)
            {
                State.Reset(Timer);
                Notify();
            }

            End = End.Add(_second);
        }

        private void Notify()
        {
            Hide();
            CheckTrayIcon();

            NIcon?.ShowBalloonTip(2000);

            Show();
            WindowState = StoredWindowState;
        }

        private void OnClose(object sender, CancelEventArgs args)
        {
            NIcon.Dispose();
            NIcon = null;
        }

        private void CheckTrayIcon()
        {
            if (NIcon != null)
                NIcon.Visible = !IsVisible;
        }

        private void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            State.Reset(Timer);
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            State.Start(Timer);
        }

        private void Pause_OnClick(object sender, RoutedEventArgs e)
        {
            State.Pause(Timer);
        }

        private void nIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = StoredWindowState;
        }
    }
}
