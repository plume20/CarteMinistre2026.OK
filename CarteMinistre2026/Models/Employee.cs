using System;
using System.ComponentModel.DataAnnotations;

namespace CarteMinistre2026.Models
{
    public class Employee  // ← Ajoute "public" ici
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; }        // Nom

        public string PostName { get; set; }        // Post Nom

        [Required]
        public string FirstName { get; set; }       // Prénom

        public string BirthPlace { get; set; }      // Lieu de naissance
        public DateTime? BirthDate { get; set; }    // Date de naissance

        public string Ministry { get; set; }        // Ministère
        public string JobTitle { get; set; }        // Fonction

        public DateTime? OrdinationDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public byte[] Photo { get; set; }           // Photo
    }
}