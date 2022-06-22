using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class LateCheckout
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ConfirmText { get; set; }
        public int? HourLimit { get; set; }
        public string HotelCode { get; set; }
        public int? Price { get; set; }
        public bool Active { get; set; }
    }
}
