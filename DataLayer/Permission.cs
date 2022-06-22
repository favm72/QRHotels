using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Permission
    {
        public int Id { get; set; }
        public string HotelCode { get; set; }
        public int IdUser { get; set; }
        public bool Active { get; set; }
    }
}
