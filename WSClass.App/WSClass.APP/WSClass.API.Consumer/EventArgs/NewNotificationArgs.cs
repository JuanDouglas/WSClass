using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSClass.API.Consumer.Models;

namespace WSClass.API.Consumer.EventArgs
{
   public class NewNotificationArgs
    {
        public Notification Notification { get; set; }
        public DateTime RequestDate { get; set; }

        public NewNotificationArgs(Notification notification, DateTime requestDate)
        {
            Notification = notification ?? throw new ArgumentNullException(nameof(notification));
            RequestDate = requestDate;
        }
        
    }
}
