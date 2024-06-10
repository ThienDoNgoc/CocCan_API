using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class Product
    {
        public Product()
        {
            MenuDetails = new HashSet<MenuDetail>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? StoreId { get; set; }
        public int Status { get; set; }

        public virtual Category Category { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<MenuDetail> MenuDetails { get; set; }
    }
}
