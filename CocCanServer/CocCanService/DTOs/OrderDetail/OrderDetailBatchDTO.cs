using CocCanService.DTOs.Location;
using CocCanService.DTOs.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.OrderDetail
{
    public class OrderDetailBatchDTO
    {
        public StoreDTO storeDTO { get; set; }
        public List<OrderDetailDTO> orderDetailDTOs { get; set; }
    }
}
