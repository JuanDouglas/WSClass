using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSClass.API.Consumer.Models;
using WSClass.API.Consumer;
using WSClass.API.Consumer.Controllers;
using WSClass.API.Consumer.EventArgs;
using System.Threading;

namespace TesteConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var controller = new LoginController();
            var login = await controller.LoginAsync("juan@teste.com","teste12");
            var test = General.NotificationThread;
            test.NewNotification += new NotificationThread.NewNotificationHandler((object sender, NewNotificationArgs notificationArgs) => {
                Console.WriteLine($"Notification Date: {notificationArgs.RequestDate}, \n Notification Title: {notificationArgs.Notification.Title}, \n Notification: {notificationArgs.Notification.Content}");
            });
            test.Start(new NotificationThreadStart(login.Token, login.ValidKey,1000));

        }
    }
}
