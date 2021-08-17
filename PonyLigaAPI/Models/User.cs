using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class User
    {
        public int id { get; set; }
        public String firstName { get; set; }
        public String surName { get; set; }
        public String loginName { get; set; }
        public String passwordHash { get; set; }
        public int userPrivileges { get; set; } 
    }
}
