using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarteMinistre2026.Models
{
    public class PrintHistory  // ← Ajoute "public" ici
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public DateTime PrintedAt { get; set; }
        public int Copies { get; set; }
        public string PrinterName { get; set; }
    }
}