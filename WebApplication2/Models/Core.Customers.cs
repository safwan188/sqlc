using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    [Table("Customers", Schema = "Core")]
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(125)]
        public string FirstName { get; set; }

        [MaxLength(125)]
        public string LastName { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalaryAmount { get; set; }
    }
}
