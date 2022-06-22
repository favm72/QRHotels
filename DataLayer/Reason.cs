using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Reason
    {
        public Reason()
        {
            OrderStatus = new HashSet<OrderStatus>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public int? IdAppUser { get; set; }

        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
    }
}
