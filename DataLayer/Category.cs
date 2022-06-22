using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Category
    {
        public Category()
        {
            ProductType = new HashSet<ProductType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public string Image { get; set; }
        public int? OrderNo { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int? IdUser { get; set; }

        public virtual ICollection<ProductType> ProductType { get; set; }
    }
}
