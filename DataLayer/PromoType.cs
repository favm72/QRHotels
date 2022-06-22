using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class PromoType
    {
        public PromoType()
        {
            Promo = new HashSet<Promo>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Promo> Promo { get; set; }
    }
}
