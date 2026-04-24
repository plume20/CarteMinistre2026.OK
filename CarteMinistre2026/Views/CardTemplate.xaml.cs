using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CarteMinistre2026.Views
{
    public partial class CardTemplate : UserControl
    {
        public CardTemplate()
        {
            InitializeComponent();
        }

        // Méthode simple pour remplir les champs
        public void SetData(Models.Employee emp)
        {
            if (emp == null) return;

            TxtNom.Text = emp.LastName ?? "";
            TxtPostNom.Text = emp.PostName ?? "";
            TxtPrenom.Text = emp.FirstName ?? "";
            TxtLieuNaissance.Text = emp.BirthPlace ?? "";
            TxtMinistere.Text = emp.Ministry ?? "";
            TxtFonction.Text = emp.JobTitle ?? "";
            TxtDateOrdination.Text = emp.OrdinationDate?.ToString("dd/MM/yyyy") ?? "";
            TxtDateEmission.Text = emp.IssueDate?.ToString("dd/MM/yyyy") ?? "";
            TxtDateExpiration.Text = emp.ExpiryDate?.ToString("dd/MM/yyyy") ?? "";

            // Photo
            if (emp.Photo != null && emp.Photo.Length > 0)
            {
                using (var ms = new MemoryStream(emp.Photo))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    image.Freeze();
                    PhotoImage.Source = image;
                }
            }
            else
            {
                PhotoImage.Source = null;
            }
        }
    }
}