using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class HomeMenu
    {
        public int Id { get; set; }
        public string HotelCode { get; set; }
        public int OrderNo { get; set; }
        public string ImageUrl { get; set; }
        public bool IsHalf { get; set; }
        public string Description { get; set; }
        public string LinkUrl { get; set; }
        public bool Active { get; set; }
        public string Link { get; set; }
        public int? IdInfoPage { get; set; }
        public string Title { get; set; }
    }
}
