using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsList.Models
{
    public class ProductListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
    }
}
