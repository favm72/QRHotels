using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class OrderStatus
    {
        public int Id { get; set; }
        public int? IdOrderHead { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public int? IdReason { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public int? IdAppUser { get; set; }

        public virtual AppUser IdAppUserNavigation { get; set; }
        public virtual OrderHead IdOrderHeadNavigation { get; set; }
        public virtual Reason IdReasonNavigation { get; set; }
    }
}
