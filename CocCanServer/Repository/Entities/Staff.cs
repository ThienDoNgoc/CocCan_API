using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Entities
{
    public partial class Staff
    {
        public Staff()
        {
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
    }
}
