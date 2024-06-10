using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class Session
    {
        public Session()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public Guid TimeSlotId { get; set; }
        public Guid LocationId { get; set; }
        public Guid MenuId { get; set; }
        public int? SessionStatus { get; set; }
        public int Status { get; set; }

        public virtual Location Location { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
