using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    [Table("Managers", Schema = "Investments")]
    public class Manager
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(125)]
        public string FirstName { get; set; }

        [MaxLength(125)]
        public string LastName { get; set; }
    }
}
