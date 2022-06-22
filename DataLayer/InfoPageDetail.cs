using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class InfoPageDetail
    {
        public int Id { get; set; }
        public int IdInfoPage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string MapUrl { get; set; }
        public bool Active { get; set; }
        public int OrderNo { get; set; }
        public string LinkUrl { get; set; }

        public virtual InfoPage IdInfoPageNavigation { get; set; }
    }
}
