using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    [Table("InvestmentAccounts", Schema = "Investments")]
    public class InvestmentAccount
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }

        [Required]
        public int Type { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        // Navigation Property
        public Customer? Customer { get; set; }
    }
}
