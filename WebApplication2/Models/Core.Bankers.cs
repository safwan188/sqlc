using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    [Table("Bankers", Schema = "Core")]
    public class Banker
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(125)]
        public string FirstName { get; set; }

        [MaxLength(125)]
        public string LastName { get; set; }

        // Foreign Key
        public Guid BranchId { get; set; }

        // Navigation Property
        [ForeignKey("BranchId")]
        public virtual Address? Branch { get; set; }
    }
}
