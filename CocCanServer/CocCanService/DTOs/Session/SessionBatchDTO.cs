using CocCanService.DTOs.OrderDetail;
using CocCanService.DTOs.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Session
{
    public class SessionBatchDTO
    {
        public StoreDTO storeDTO { get; set; }
        public List<OrderDetailDTO> orderDetailDTOs { get; set; }
    }
}
