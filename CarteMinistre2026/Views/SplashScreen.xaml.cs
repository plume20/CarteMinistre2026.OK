using System;
using System.Windows;
using System.Windows.Threading;

namespace CarteMinistre2026.Views
{
    public partial class SplashScreen : Window
    {
        private DispatcherTimer _timer;
        private DispatcherTimer _loadingTextTimer;
        private int _dotCount = 0;

        public SplashScreen()
        {
            InitializeComponent();

            _loadingTextTimer = new DispatcherTimer();
            _loadingTextTimer.Interval = TimeSpan.FromMilliseconds(500);
            _loadingTextTimer.Tick += LoadingTextTimer_Tick;
            _loadingTextTimer.Start();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void LoadingTextTimer_Tick(object sender, EventArgs e)
        {
            _dotCount = (_dotCount + 1) % 4;
            string dots = new string('.', _dotCount);
            // Si tu as un TextBlock nommé LoadingText dans le XAML
            LoadingText.Text = $"Chargement en cours{dots}";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            _loadingTextTimer.Stop();

            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}