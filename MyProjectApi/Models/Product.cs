using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyProjectApi.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }
        [Range(0.0, 30000000)]
        public decimal? Price { get; set; }
        [MaxLength(400)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Category { get; set; }
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
