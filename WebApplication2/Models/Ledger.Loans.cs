using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    [Table("Loans", Schema = "Ledger")]
    public class Loan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime ApprovedDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public int Term { get; set; }

        // Foreign Key
        public Guid AccountId { get; set; }

        // Navigation Property
        [ForeignKey("AccountId")]
        public virtual Account? Account { get; set; }
    }
}
