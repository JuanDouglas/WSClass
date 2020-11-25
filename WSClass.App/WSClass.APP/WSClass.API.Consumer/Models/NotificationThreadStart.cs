using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSClass.API.Consumer.Models
{
    public class NotificationThreadStart
    {
        public string LoginToken { get; set; }
        public string ValidKey { get; set; }
        public int UpdateDelay { get; set; }

        public NotificationThreadStart(string loginToken, string validKey, int updateDelay)
        {
            LoginToken = loginToken ?? throw new ArgumentNullException(nameof(loginToken));
            ValidKey = validKey ?? throw new ArgumentNullException(nameof(validKey));
            UpdateDelay = updateDelay;
        }
    }
}
