using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CarteMinistre2026.Data;
using CarteMinistre2026.Services;
using CarteMinistre2026.ViewModels;
using Microsoft.Win32;

namespace CarteMinistre2026.Views
{
    public partial class ImportView : UserControl
    {
        public ImportView()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (ExcelRadio.IsChecked == true)
                dialog.Filter = "Fichiers Excel|*.xlsx;*.xls";
            else if (CsvRadio.IsChecked == true)
                dialog.Filter = "Fichiers CSV|*.csv";
            else if (OdsRadio.IsChecked == true)
                dialog.Filter = "Fichiers OpenDocument|*.ods";

            if (dialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = dialog.FileName;
            }
        }

        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FilePathTextBox.Text))
            {
                MessageBox.Show("Veuillez sélectionner un fichier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                ImportButton.IsEnabled = false;
                ImportButton.Content = "Import en cours...";

                var service = new ImportService();
                var employees = await Task.Run(() => service.ImportFromFile(FilePathTextBox.Text));

                using (var db = new AppDbContext())
                {
                    db.Employees.AddRange(employees);
                    await db.SaveChangesAsync();
                }

                MessageBox.Show($"{employees.Count} employé(s) importé(s) avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Retour à la liste et rafraîchir
                ReturnToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'import : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ImportButton.IsEnabled = true;
                ImportButton.Content = " Importer";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnToList();
        }

        private void ReturnToList()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var viewModel = mainWindow.DataContext as MainViewModel;
                if (viewModel != null)
                {
                    viewModel.ShowEmployeeListCommand.Execute(null);
                }
            }
        }
    }
}