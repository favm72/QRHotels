using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string HotelCode { get; set; }
        public bool Active { get; set; }
        public string HourStart { get; set; }
        public string HourEnd { get; set; }
    }
}
