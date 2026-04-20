using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using CarteMinistre2026.Models;
using CarteMinistre2026.Services;
using Microsoft.Win32;


namespace CarteMinistre2026.Views
{
    public partial class PreviewView : Window
    {
        private Employee _currentEmployee;
        private readonly CardRenderer _cardRenderer;
        private BitmapImage _currentCardImage;

        public PreviewView()
        {
            InitializeComponent();

            // Initialiser le renderer avec le template
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "template_carte2.jpg");
            _cardRenderer = new CardRenderer(templatePath);
        }

        public void SetEmployee(Employee employee)
        {
            _currentEmployee = employee;
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            if (_currentEmployee != null)
            {
                try
                {
                    _currentCardImage = _cardRenderer.GenerateCardForDisplay(_currentEmployee);
                    CardImage.Source = _currentCardImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la génération de l'aperçu : {ex.Message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Navigation entre employés
            MessageBox.Show("Navigation à implémenter", "Information",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Navigation entre employés
            MessageBox.Show("Navigation à implémenter", "Information",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee == null || _currentCardImage == null)
            {
                MessageBox.Show("Aucune carte à exporter.", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Fichier JPEG|*.jpg|Fichier PNG|*.png";
            dialog.DefaultExt = ".jpg";
            dialog.FileName = $"Carte_{_currentEmployee.LastName}_{_currentEmployee.FirstName}_{DateTime.Now:yyyyMMdd}.jpg";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _cardRenderer.SaveCardToFile(_currentEmployee, dialog.FileName);
                    MessageBox.Show("Carte exportée avec succès.", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'export : {ex.Message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            var printView = new PrintView();
            printView.Owner = this;
            printView.SetEmployeeName($"{_currentEmployee.FirstName} {_currentEmployee.LastName}");
            printView.ShowDialog();
        }

        internal void SetEmployee(object selectedEmployee)
        {
            throw new NotImplementedException();
        }
    }
}