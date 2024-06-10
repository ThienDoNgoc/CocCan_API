using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal CartTotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Note { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SessionId { get; set; }
        public Guid PickUpSpotId { get; set; }
        public string Phone { get; set; }
        public int OrderStatus { get; set; }
        public int Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual PickUpSpot PickUpSpot { get; set; }
        public virtual Session Session { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
