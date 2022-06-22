using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int? IdOrderHead { get; set; }
        public int? IdProduct { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Modifiers { get; set; }

        public virtual OrderHead IdOrderHeadNavigation { get; set; }
        public virtual Product IdProductNavigation { get; set; }
    }
}
