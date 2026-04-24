using CarteMinistre2026.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CarteMinistre2026.Views
{
    public partial class PreviewView : UserControl
    {
        private Employee _currentEmployee;

        public PreviewView()
        {
            InitializeComponent();
        }

        // Méthode pour afficher un employé
        public void SetEmployee(Employee employee)
        {
            _currentEmployee = employee;

            // Appeler la méthode SetData du CardTemplate
            CardTemplateControl.SetData(employee);

            // Forcer le rendu
            CardTemplateControl.UpdateLayout();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO : Navigation entre employés
            MessageBox.Show("Navigation précédent à implémenter", "Info",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO : Navigation entre employés
            MessageBox.Show("Navigation suivant à implémenter", "Info",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee == null) return;

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Fichier JPEG|*.jpg|Fichier PNG|*.png",
                DefaultExt = ".jpg",
                FileName = $"Carte_{_currentEmployee.LastName}_{_currentEmployee.FirstName}_{DateTime.Now:yyyyMMdd}.jpg"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Exporter le CardTemplate en image
                    ExportCardToImage(dialog.FileName);
                    MessageBox.Show("Carte exportée avec succès !", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur export : {ex.Message}", "Erreur",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportCardToImage(string filePath)
        {
            // S'assurer que le contrôle est rendu
            CardTemplateControl.Measure(new Size(CardTemplateControl.Width, CardTemplateControl.Height));
            CardTemplateControl.Arrange(new Rect(0, 0, CardTemplateControl.Width, CardTemplateControl.Height));
            CardTemplateControl.UpdateLayout();

            // Rendre en bitmap
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)CardTemplateControl.ActualWidth,
                (int)CardTemplateControl.ActualHeight,
                96d, 96d, PixelFormats.Pbgra32);

            renderBitmap.Render(CardTemplateControl);

            // Encoder en JPEG
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fs);
            }
        }

        internal void SetEmployee(object selectedEmployee)
        {
            throw new NotImplementedException();
        }
    }
}