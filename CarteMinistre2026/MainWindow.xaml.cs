using CarteMinistre2026.ViewModels;
using CarteMinistre2026.Views;
using System;
using System.Collections.Generic;
using System.Linq;
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
using CarteMinistre2026.Models;


namespace CarteMinistre2026
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            // Charger les données au démarrage
            Loaded += (s, e) => _viewModel.RefreshCommand.Execute(null);
        }

        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Carte Ministre 2026\n\n" +
                "Application de gestion et d'impression de cartes ministérielles\n\n" +
                "Version 1.0\n" +
                "© 2026 - Tous droits réservés",
                "À propos",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void ExecutePreview(object parameter)
        {
            if (SelectedEmployee == null) return;

            var preview = new PreviewView();
            preview.SetEmployee(SelectedEmployee);
            CurrentView = preview;
        }
    }
}