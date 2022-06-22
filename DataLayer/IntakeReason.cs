using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class IntakeReason
    {
        public IntakeReason()
        {
            OrderHead = new HashSet<OrderHead>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<OrderHead> OrderHead { get; set; }
    }
}
