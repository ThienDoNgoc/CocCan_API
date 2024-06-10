using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class LocationStore
    {
        public Guid LocationId { get; set; }
        public Guid StoreId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Store Store { get; set; }
    }
}
