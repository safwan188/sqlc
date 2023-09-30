using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    [Table("Investments", Schema = "Investments")]
    public class Investment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string InvestmentType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime InvestmentDate { get; set; }

        [ForeignKey("InvestmentAccount")]
        public Guid AccountId { get; set; }

        [ForeignKey("Manager")]
        public Guid ManagerId { get; set; }

        // Navigation Properties
        public InvestmentAccount? InvestmentAccount { get; set; }
        public Manager? Manager { get; set; }
    }
}
