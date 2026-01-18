using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.DTOs.ProductDTOs
{
    public class ReturnProduct
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CurrentQuantity { get; set; }
        public string CreateAt { get; set; }
    }
}
