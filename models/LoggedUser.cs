using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.models
{
    public static class LoggedUser
    {
        public static UserInfo LoggedInUser { get; set; }

        public static void UpdateBalance(int amountToAdd)
        {
            if (LoggedInUser != null)
            {
                LoggedInUser.Balance += amountToAdd;
            }
        }
        public static int CheckBalance()
        {
            if (LoggedInUser != null)
            {
                return LoggedInUser.Balance;
            }
            return 0; // Return 0 if user is not logged in
        }
    }

}
