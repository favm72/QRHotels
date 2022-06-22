using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class UserLog
    {
        public int Id { get; set; }
        public int IdReservation { get; set; }
        public string Action { get; set; }
        public DateTime Created { get; set; }

        public virtual Reservation IdReservationNavigation { get; set; }
    }
}
