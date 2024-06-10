using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.PickUpSpot
{
    public class CreatePickUpSpotDTO
    {
        public string Fullname { get; set; }
        public string Address { get; set; }
        public Guid LocationId { get; set; }
    }
}
