using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class Location
    {
        public Location()
        {
            PickUpSpots = new HashSet<PickUpSpot>();
            Sessions = new HashSet<Session>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }

        public virtual ICollection<PickUpSpot> PickUpSpots { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
