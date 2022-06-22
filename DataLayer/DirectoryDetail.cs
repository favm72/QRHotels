using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class DirectoryDetail
    {
        public int Id { get; set; }
        public string HotelCode { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string Content { get; set; }
        public bool Active { get; set; }
        public int OrderNo { get; set; }
        public string BannerUrl { get; set; }
    }
}
