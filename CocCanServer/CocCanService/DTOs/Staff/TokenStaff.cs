﻿using CocCanService.DTOs.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Customer
{
    public class TokenStaff
    {
        public StaffDTO staffDTO { get; set; }
        public string token { get; set; }
    }
}
