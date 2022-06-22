using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class AppUser
    {
        public AppUser()
        {
            OrderStatus = new HashSet<OrderStatus>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
    }
}
