using System;
using System.ComponentModel.DataAnnotations;

namespace Product.Api.Models
{
    public class ProductUpdateInputModel
    {     
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal Price { get; set; }
    }
}
