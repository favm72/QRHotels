using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class PaymType
    {
        public PaymType()
        {
            OrderHead = new HashSet<OrderHead>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool? UnlimitedCred { get; set; }

        public virtual ICollection<OrderHead> OrderHead { get; set; }
    }
}
