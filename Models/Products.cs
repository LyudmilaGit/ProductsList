using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsList.Models
{
    public partial class Products
    {
        public int Id { get; set; }
        public DateTime Updated { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public double Price { get; set; }
        public bool? IsActive { get; set; }

        public Categories Category { get; set; }
    }
}
