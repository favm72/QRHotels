using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Promo
    {
        public Promo()
        {
            OrderHead = new HashSet<OrderHead>();
        }

        public int Id { get; set; }
        public int IdPromoType { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool Active { get; set; }
        public int Discount { get; set; }

        public virtual PromoType IdPromoTypeNavigation { get; set; }
        public virtual ICollection<OrderHead> OrderHead { get; set; }
    }
}
