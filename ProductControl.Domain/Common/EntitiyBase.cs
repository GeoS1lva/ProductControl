using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Common
{
    public class EntitiyBase
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
    }
}
