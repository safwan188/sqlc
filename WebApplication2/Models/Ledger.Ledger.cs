using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    [Table("Ledger", Schema = "Ledger")]
    public class LedgerEntry
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public int Type { get; set; }

        // Foreign Key
        public Guid AccountId { get; set; }

        // Navigation Property
        [ForeignKey("AccountId")]
        public virtual Account? Account { get; set; }
    }
}
