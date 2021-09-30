using Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string passwordEncrypt()
        {
            ScryptEncoder encoder = new ScryptEncoder();

            string hashedPassword = encoder.Encode(this.passwordHash);

            return hashedPassword;
        }

        public bool comparePassword(String hashedPassword)
        {
            ScryptEncoder encoder = new ScryptEncoder();

            bool areEquals = encoder.Compare(hashedPassword, this.passwordHash);

            return areEquals;
        }
    }
}
