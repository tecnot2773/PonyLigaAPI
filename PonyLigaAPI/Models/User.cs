using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class User
    {
        public int id { get; set; }
        public String first_name { get; set; }
        public String sur_name { get; set; }
        public String login_name { get; set; }
        public String password_hash { get; set; }

    }
}
