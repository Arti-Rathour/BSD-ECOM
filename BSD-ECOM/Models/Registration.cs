﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Models
{
    public class Registration
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string password{get;set;}
        public string Email { get; set; }
        public string phone { get; set; }
        public string Address { get; set; }
    }
}
