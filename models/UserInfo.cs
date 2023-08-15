using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.models
{
    public class UserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public int Balance { get; set; }

        public UserInfo()
        {
            Balance = 0;
        }
    }
    
}
