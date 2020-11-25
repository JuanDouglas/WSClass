using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSClass.API.Consumer.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public System.Drawing.Image Icon { get; set; }
        public bool ContainIcon => Icon != null;

        public Notification(int iD, string title, string content, System.Drawing.Image icon)
        {
            ID = iD;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Icon = icon ?? throw new ArgumentNullException(nameof(icon));
        }

        public Notification(int iD, string title, string content)
        {
            ID = iD;
            Title = title;
            Content = content;
        }
    }
}
