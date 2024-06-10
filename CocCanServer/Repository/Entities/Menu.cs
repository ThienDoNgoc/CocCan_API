using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class Menu
    {
        public Menu()
        {
            MenuDetails = new HashSet<MenuDetail>();
            Sessions = new HashSet<Session>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public virtual ICollection<MenuDetail> MenuDetails { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
