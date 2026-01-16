using ProductControl.Domain.Common;
using ProductControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Entities
{
    public class StockMovement : EntitiyBase
    {
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }

        public Guid UserId { get; set; }
        public Users User { get; private set; }

        public int Amount { get; private set; }
    }
}
