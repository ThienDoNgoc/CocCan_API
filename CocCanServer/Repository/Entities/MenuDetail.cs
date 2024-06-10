using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class MenuDetail
    {
        public MenuDetail()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public Guid MenuId { get; set; }
        public Guid ProductId { get; set; }
        public int Status { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
