using System.Windows;
using Microsoft.Win32;

namespace CarteMinistre2026.Views
{
    public partial class ImportView : Window
    {
        public ImportView()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = ExcelRadio.IsChecked == true
                ? "Fichiers Excel|*.xlsx;*.xls"
                : "Fichiers CSV|*.csv";

            if (dialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = dialog.FileName;
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implémenter l'import
            MessageBox.Show("Import à implémenter", "Information",
                MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}