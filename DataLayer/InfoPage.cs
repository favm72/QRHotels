using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class InfoPage
    {
        public InfoPage()
        {
            InfoPageDetail = new HashSet<InfoPageDetail>();
        }

        public int Id { get; set; }
        public string HotelCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<InfoPageDetail> InfoPageDetail { get; set; }
    }
}
