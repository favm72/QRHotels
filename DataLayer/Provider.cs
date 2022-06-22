using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Provider
    {
        public Provider()
        {
            CartConfig = new HashSet<CartConfig>();
            OrderHead = new HashSet<OrderHead>();
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? HourStart { get; set; }
        public int? HourEnd { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int? IdUser { get; set; }

        public virtual ICollection<CartConfig> CartConfig { get; set; }
        public virtual ICollection<OrderHead> OrderHead { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
