using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSClass.API.Consumer.Models
{
    public class NotificationAPI
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ImageModel Icon { get; set; }
    }
}
