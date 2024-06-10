using CocCanService.DTOs.OrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Order
{
    public class CreateOrderDTO
    {
        public DateTime OrderTime { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal CartTotalAmount { get; set; }
        public string Note { get; set; }
        public string Phone { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SessionId { get; set; }
        public Guid PickUpSpotId { get; set; }
        public List<CreateOrderDetailDTO> CreateOrderDetailDTOs { get; set; }
    }
}
