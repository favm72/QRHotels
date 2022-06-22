using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Slider
    {
        public int Id { get; set; }
        public string HotelCode { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; }
        public int? OrderNo { get; set; }
        public bool Active { get; set; }
    }
}
