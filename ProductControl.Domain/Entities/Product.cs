using ProductControl.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Entities
{
    public class Product : EntitiyBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }
        public int CurrentQuantity { get; private set; }

        public ICollection<StockMovement> StockMovement { get; private set; }
    }
}
