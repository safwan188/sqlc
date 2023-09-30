using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2
{
    [Table("Address", Schema = "Core")]
    public class Address
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(125)]
        public string State { get; set; }

        [MaxLength(125)]
        public string City { get; set; }

        [MaxLength(125)]
        public string Street { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }
    }
}
