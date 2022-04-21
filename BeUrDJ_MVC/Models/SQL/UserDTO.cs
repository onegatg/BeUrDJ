using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeUrDJ_MVC.Models
{
    public class UserDTO : IdentityUser   
    {
        public int UserID { get; set; }
        override
        public string UserName { get; set; }
        public string Password { get; set; }
        public int TokenID { get; set; }
    }
}
