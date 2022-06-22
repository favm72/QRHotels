using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class OrderHead
    {
        public OrderHead()
        {
            OrderDetail = new HashSet<OrderDetail>();
            OrderStatus = new HashSet<OrderStatus>();
        }

        public int Id { get; set; }
        public int IdReservation { get; set; }
        public int? IdProvider { get; set; }
        public int? AtentionTime { get; set; }
        public DateTime? BreakFastDate { get; set; }
        public int? IdSchedule { get; set; }
        public int? IdLateCheckout { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Total { get; set; }
        public DateTime Created { get; set; }
        public bool Viewed { get; set; }
        public int? IdPaymType { get; set; }
        public int? IdPromo { get; set; }
        public int? IdIntakeReason { get; set; }

        public virtual IntakeReason IdIntakeReasonNavigation { get; set; }
        public virtual PaymType IdPaymTypeNavigation { get; set; }
        public virtual Promo IdPromoNavigation { get; set; }
        public virtual Provider IdProviderNavigation { get; set; }
        public virtual Reservation IdReservationNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
    }
}
