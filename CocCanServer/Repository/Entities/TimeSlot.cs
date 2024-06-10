using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            Sessions = new HashSet<Session>();
        }

        public Guid Id { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
