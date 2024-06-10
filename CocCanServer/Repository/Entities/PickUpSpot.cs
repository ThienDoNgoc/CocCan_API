using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class PickUpSpot
    {
        public PickUpSpot()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public Guid LocationId { get; set; }
        public int Status { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
