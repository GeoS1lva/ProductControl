using ProductControl.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Entities
{
    public class RevokedToken : EntitiyBase
    {
        public string Jti { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        public RevokedToken(string jti, DateTime expiryDate)
        {
            Jti = jti;
            ExpiryDate = expiryDate;
            CreatedAt = DateTime.UtcNow;
        }

        private RevokedToken() { }
    }
}
