using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class ProductModifier
    {
        public ProductModifier()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int IdProductType { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public virtual ProductType IdProductTypeNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
