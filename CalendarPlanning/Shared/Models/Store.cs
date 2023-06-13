﻿using CalendarPlanning.Shared.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarPlanning.Shared.Models
{
    [Table("Stores", Schema = "dbo")]
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StoreId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public required string Name { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public required string Address { get; set; }

        public virtual ICollection<Employee>? Employees { get; set; }
    }
}
