using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyProjectApi.Entities
{
    public class Brand
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}