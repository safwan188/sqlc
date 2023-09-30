using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    [Table("Accounts", Schema = "Ledger")]
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OverdraftLimit { get; set; }

        [Required]
        public int Type { get; set; }

        // Foreign Keys
        public Guid CustomerId { get; set; }
        public Guid BankerId { get; set; }

        // Navigation Properties
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("BankerId")]
        public virtual Banker? Banker { get; set; }
    }
}
