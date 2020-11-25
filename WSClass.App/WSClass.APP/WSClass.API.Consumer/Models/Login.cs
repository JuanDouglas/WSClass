using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSClass.API.Consumer.Models
{
    public class Login
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public User User { get; set; }
    }
}