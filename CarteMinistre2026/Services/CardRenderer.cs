using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using CarteMinistre2026.Models;

namespace CarteMinistre2026.Services
{
    public class CardRenderer
    {
        private readonly string _templatePath;
        private Image _templateImage;

        // Positions des champs sur le template (à ajuster selon ton image)
        // Ces valeurs sont en pixels, à définir avec l'éditeur plus tard
        public float NomX { get; set; } = 200;
        public float NomY { get; set; } = 300;

        public float PostNomX { get; set; } = 200;
        public float PostNomY { get; set; } = 330;

        public float PrenomX { get; set; } = 200;
        public float PrenomY { get; set; } = 360;

        public float LieuNaissanceX { get; set; } = 200;
        public float LieuNaissanceY { get; set; } = 390;

        public float DateNaissanceX { get; set; } = 450;
        public float DateNaissanceY { get; set; } = 390;

        public float MinistereX { get; set; } = 200;
        public float MinistereY { get; set; } = 420;

        public float FonctionX { get; set; } = 200;
        public float FonctionY { get; set; } = 450;

        public float DateOrdinationX { get; set; } = 450;
        public float DateOrdinationY { get; set; } = 450;

        public float DateEmissionX { get; set; } = 600;
        public float DateEmissionY { get; set; } = 550;

        public float DateExpirationX { get; set; } = 600;
        public float DateExpirationY { get; set; } = 580;

        // Zone photo
        public float PhotoX { get; set; } = 50;
        public float PhotoY { get; set; } = 300;
        public float PhotoWidth { get; set; } = 120;
        public float PhotoHeight { get; set; } = 140;

        // Police et couleurs
        public string FontFamily { get; set; } = "Arial";
        public float FontSize { get; set; } = 12;
        public Color TextColor { get; set; } = Color.Black;

        public CardRenderer(string templatePath)
        {
            _templatePath = templatePath;
            LoadTemplate();
        }

        private void LoadTemplate()
        {
            if (File.Exists(_templatePath))
            {
                _templateImage = Image.FromFile(_templatePath);
            }
            else
            {
                // Créer une image par défaut si le template n'existe pas
                _templateImage = new Bitmap(800, 600);
                using (var g = Graphics.FromImage(_templateImage))
                {
                    g.Clear(Color.White);
                    g.DrawRectangle(Pens.Gray, 10, 10, 780, 580);
                }
            }
        }

        public Bitmap GenerateCard(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            // Créer une copie du template
            Bitmap card = new Bitmap(_templateImage);

            using (Graphics g = Graphics.FromImage(card))
            {
                // Configurer la qualité du rendu
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                using (Font font = new Font(FontFamily, FontSize, FontStyle.Regular))
                using (Brush brush = new SolidBrush(TextColor))
                {
                    // Dessiner les textes
                    DrawText(g, $"{employee.LastName}", NomX, NomY, font, brush);
                    DrawText(g, $"{employee.PostName}", PostNomX, PostNomY, font, brush);
                    DrawText(g, $"{employee.FirstName}", PrenomX, PrenomY, font, brush);
                    DrawText(g, $"{employee.BirthPlace}", LieuNaissanceX, LieuNaissanceY, font, brush);
                    DrawText(g, employee.BirthDate?.ToString("dd/MM/yyyy") ?? "", DateNaissanceX, DateNaissanceY, font, brush);
                    DrawText(g, $"{employee.Ministry}", MinistereX, MinistereY, font, brush);
                    DrawText(g, $"{employee.JobTitle}", FonctionX, FonctionY, font, brush);
                    DrawText(g, employee.OrdinationDate?.ToString("dd/MM/yyyy") ?? "", DateOrdinationX, DateOrdinationY, font, brush);
                    DrawText(g, employee.IssueDate?.ToString("dd/MM/yyyy") ?? "", DateEmissionX, DateEmissionY, font, brush);
                    DrawText(g, employee.ExpiryDate?.ToString("dd/MM/yyyy") ?? "", DateExpirationX, DateExpirationY, font, brush);
                }

                // Dessiner la photo
                if (employee.Photo != null && employee.Photo.Length > 0)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(employee.Photo))
                        {
                            Image photo = Image.FromStream(ms);
                            g.DrawImage(photo, PhotoX, PhotoY, PhotoWidth, PhotoHeight);
                        }
                    }
                    catch
                    {
                        DrawPlaceholderPhoto(g);
                    }
                }
                else
                {
                    DrawPlaceholderPhoto(g);
                }
            }

            return card;
        }

        private void DrawText(Graphics g, string text, float x, float y, Font font, Brush brush)
        {
            if (!string.IsNullOrEmpty(text))
            {
                g.DrawString(text, font, brush, x, y);
            }
        }

        private void DrawPlaceholderPhoto(Graphics g)
        {
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                g.DrawRectangle(pen, PhotoX, PhotoY, PhotoWidth, PhotoHeight);
                g.DrawLine(pen, PhotoX, PhotoY, PhotoX + PhotoWidth, PhotoY + PhotoHeight);
                g.DrawLine(pen, PhotoX + PhotoWidth, PhotoY, PhotoX, PhotoY + PhotoHeight);
            }
        }

        public BitmapImage GenerateCardForDisplay(Employee employee)
        {
            using (Bitmap bitmap = GenerateCard(employee))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    ms.Position = 0;

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    return bitmapImage;
                }
            }
        }

        public void SaveCardToFile(Employee employee, string filePath)
        {
            using (Bitmap bitmap = GenerateCard(employee))
            {
                bitmap.Save(filePath, ImageFormat.Jpeg);
            }
        }

        public void Dispose()
        {
            _templateImage?.Dispose();
        }
    }
}