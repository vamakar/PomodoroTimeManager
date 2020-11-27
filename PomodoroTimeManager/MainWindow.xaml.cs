using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace PomodoroTimeManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static DispatcherTimer _timer;
        private static TimeSpan _dialTime;
        private static DateTime _end;
        private static SoundPlayer player;

        private static readonly TimeSpan Second = new TimeSpan(0, 0, 1);
        private static readonly TimeSpan Minute = new TimeSpan(0, 1, 0);

        private NotifyIcon nIcon;

        public MainWindow()
        {
            InitializeComponent();

            nIcon = new NotifyIcon();
            nIcon.BalloonTipText = "Time is over!";
            nIcon.BalloonTipTitle = "Pomodoro";
            nIcon.Text = "Pomodoro";
            nIcon.Icon = new Icon(@"../../tomato.ico");
            nIcon.Click += new EventHandler(nIcon_Click);
            CheckTrayIcon();

            ResetTimer();
        }

        private void ResetTimer()
        {
            _dialTime = new TimeSpan(0, 25, 0);
            DialTextBox.Text = _dialTime.ToString("mm\\:ss");

            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Tick += CheckSecond;
            _timer.Interval = new TimeSpan(0, 0, 0,0,1);
        }

        private void CheckSecond(object sender, EventArgs e)
        {
            if (DateTime.Now < _end) return;

            _dialTime = _dialTime.Subtract(Second);
            DialTextBox.Text = _dialTime.ToString("mm\\:ss");

            if (_dialTime.TotalSeconds <= 0)
            {
                _timer.Stop();

                Hide();
                CheckTrayIcon();

                nIcon?.ShowBalloonTip(2000);

                Show();
                WindowState = m_storedWindowState;
                CheckTrayIcon();
            }

            _end = _end.Add(Second);
        }

        private void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            ResetTimer();
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            _end = DateTime.Now.Add(Second);
            _timer.Start();
        }

        private void Pause_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _dialTime = _dialTime.Add(Minute);

            var tfMinutes = new TimeSpan(0, 25, 0);
            if (_dialTime > tfMinutes)
            {
                _dialTime = tfMinutes;
            }

            DialTextBox.Text = _dialTime.ToString("mm\\:ss");
        }

        private void OnClose(object sender, CancelEventArgs args)
        {
            nIcon.Dispose();
            nIcon = null;
        }

        private WindowState m_storedWindowState = WindowState.Normal;

        private void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
            else
                m_storedWindowState = WindowState;
        }

        private void nIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }

        private void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        private void ShowTrayIcon(bool show)
        {
            if (nIcon != null)
                nIcon.Visible = show;
        }
    }
}
