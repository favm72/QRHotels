using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Hotels
    {
        public string Url { get; set; }
        public string HotelCode { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int? MinutesBfast { get; set; }
        public string ShortDescription { get; set; }
        public int HotelType { get; set; }
        public string Rmc { get; set; }
        public string Email { get; set; }
        public bool ShowRestaurants { get; set; }
        public string ImageRestaurants { get; set; }
        public int HoursToAgreeBeforeCheckout { get; set; }
    }
}
