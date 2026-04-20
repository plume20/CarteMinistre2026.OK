using System;
using System.Windows;
using CarteMinistre2026.Services;

namespace CarteMinistre2026.Views
{
    public partial class LoginView : Window
    {
        private readonly AuthenticationService _authService;

        public LoginView()
        {
            InitializeComponent();
            _authService = new AuthenticationService();

            // Création silencieuse de l'admin par défaut
            try
            {
                _authService.CreateAdminUser("admin", "admin123");
            }
            catch
            {
                // Ignorer silencieusement - l'admin existe probablement déjà
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Veuillez remplir tous les champs.");
                return;
            }

            try
            {
                if (_authService.Login(username, password))
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    ShowError("Nom d'utilisateur ou mot de passe incorrect.");
                }
            }
            catch
            {
                ShowError("Erreur de connexion à la base de données.");
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowError(string message)
        {
            ErrorText.Text = message;
            ErrorText.Visibility = Visibility.Visible;
        }
    }
}