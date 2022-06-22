using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Reservation
    {
        public Reservation()
        {
            OrderHead = new HashSet<OrderHead>();
            UserLog = new HashSet<UserLog>();
        }

        public int Id { get; set; }
        public int ReseCode { get; set; }
        public int ReseYear { get; set; }
        public string HotelCode { get; set; }
        public string RoomCode { get; set; }
        public int? Adults { get; set; }
        public int? Childs { get; set; }
        public int? Guests { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string CountryCode { get; set; }
        public string CountryDescription { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string Number { get; set; }
        public DateTime Created { get; set; }
        public string Tipo { get; set; }
        public bool Active { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public string DocumentType { get; set; }
        public string MarketSegmentCode { get; set; }
        public string LoyaltyCode { get; set; }
        public DateTime? RegisterDate { get; set; }

        public virtual ICollection<OrderHead> OrderHead { get; set; }
        public virtual ICollection<UserLog> UserLog { get; set; }
    }
}
