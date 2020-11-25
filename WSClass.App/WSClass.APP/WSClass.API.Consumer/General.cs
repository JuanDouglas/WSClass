using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WSClass.API.Consumer.EventArgs;
using WSClass.API.Consumer.Models;
using System.Threading.Tasks;
using WSClass.API.Consumer.Controllers;

namespace WSClass.API.Consumer
{
    public class General
    {
        public static Uri APIUri => new Uri("https://wsclass.azurewebsites.net/");
        public static NotificationThread NotificationThread => new NotificationThread();

        public General() { 
        }
    }

    public class NotificationThread {

        private Thread thNotifications;
        public ThreadState ThreadState => thNotifications.ThreadState;

        public delegate void NewNotificationHandler(object sender, NewNotificationArgs args);
        public event NewNotificationHandler NewNotification;
        public NotificationThread()
        {
            thNotifications = new Thread(ThreadNotifications);
        }
        private void ThreadNotifications(object @object) {
            if (!(@object is NotificationThreadStart))
            {
                throw new ArgumentException("Could not start this thread");
            }
            NotificationThreadStart notificationThreadStart = (NotificationThreadStart)@object;
            NotificationsController controller = new NotificationsController();
            do
            {
                Thread.Sleep(notificationThreadStart.UpdateDelay);
                var requestThread = controller.GetNotificationsAsync(notificationThreadStart.LoginToken, notificationThreadStart.ValidKey);
                requestThread.Wait();
                Notification[] notifications = requestThread.Result;
                if (notifications!=null)
                {
                    foreach (var notification in notifications)
                    {
                        NewNotification.Invoke(null, new NewNotificationArgs(notification, DateTime.Now));
                    }
                }
            } while (Thread.CurrentThread.ThreadState==ThreadState.Running);
        }
        public void Start(NotificationThreadStart notificationThreadStart) {
            thNotifications.Start(notificationThreadStart);
        }
        public void Stop() {
            thNotifications.Abort();
        }
    }
}