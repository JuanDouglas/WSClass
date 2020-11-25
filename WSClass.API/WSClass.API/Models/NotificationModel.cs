using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSClass.API.Models
{
    public class NotificationModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ImageModel Icon { get; set; }

        public NotificationModel(Notification notification) {
            ID = notification.ID;
            Title = notification.Title;
            Content = notification.Content;
            if (notification.Icon!=null)
            {
                Icon = new ImageModel(notification.Image);
            }
        }
    }
}