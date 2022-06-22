using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class ProductType
    {
        public ProductType()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? IdCategory { get; set; }
        public int? OrderNo { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public string Modifiers { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int? IdUser { get; set; }

        public virtual Category IdCategoryNavigation { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
