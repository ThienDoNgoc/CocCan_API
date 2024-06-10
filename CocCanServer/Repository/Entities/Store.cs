using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class Store
    {
        public Store()
        {
            Products = new HashSet<Product>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string? Phone { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
