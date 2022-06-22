using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class CartConfig
    {
        public int IdProvider { get; set; }
        public string HotelCode { get; set; }
        public string HourStart { get; set; }
        public string HourEnd { get; set; }
        public string DaysOfWeek { get; set; }
        public bool Active { get; set; }

        public virtual Provider IdProviderNavigation { get; set; }
    }
}
