using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Product
    {
        public Product()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int? IdProductType { get; set; }
        public int? IdProvider { get; set; }
        public string HotelCode { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }

        public virtual ProductType IdProductTypeNavigation { get; set; }
        public virtual Provider IdProviderNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
