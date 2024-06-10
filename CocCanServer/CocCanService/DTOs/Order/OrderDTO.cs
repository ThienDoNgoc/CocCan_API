using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Order
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal CartTotalAmount { get; set; }
        public string Note { get; set; }
        public string Phone { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SessionId { get; set; }
        public Guid PickUpSpotId { get; set;}
        public string PickUpSpotFullName { get; set; }
        public Guid LocationID { get; set; }
        public string LocationName { get; set; }
        public Guid TimeSlotID { get; set; }
        public String? TimeSlotStart { get; set; }
        public String? TimeSlotEnd { get; set; }
        public int OrderDetailCount { get; set; }
        public int OrderStatus { get; set; }
    }
}
