using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class DigitalMenu
    {
        public int Id { get; set; }
        public string HotelCode { get; set; }
        public int? IdProvider { get; set; }
        public bool IsCart { get; set; }
        public bool ViewOnly { get; set; }
        public string Image { get; set; }
        public int OrderNo { get; set; }
        public bool Active { get; set; }
    }
}
