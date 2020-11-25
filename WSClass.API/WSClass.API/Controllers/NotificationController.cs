    using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WSClass.API.Models;
using System.Threading.Tasks;
using System.Web;

namespace WSClass.API.Controllers
{
    public class NotificationController : ApiController
    {
        private TaskDatabaseEntities db = new TaskDatabaseEntities();

        // GET: api/Notification
        public IQueryable<Notification> GetNotification()
        {
            return db.Notification;
        }

        // GET: api/Notification/5
        [ResponseType(typeof(Notification[]))]
        public async Task<IHttpActionResult> GetNotificationAsync(string login_token,string valid_key)
        {
            try
            {
                await LoginController.ValidUserAsync(Guid.Parse(login_token),HttpContext.Current,valid_key);
            }
            catch (Exception)
            {
                return Unauthorized();
            }

            List<Notification> dbNotifications = await db.Notification.Where(wh => wh.User1.ValidKey == valid_key).ToListAsync();
            if (dbNotifications == null)
            {
                return NotFound();
            }

            List<NotificationModel> notifications = new List<NotificationModel>();
            foreach (var item in dbNotifications)
            {
                notifications.Add(new NotificationModel(item));
            }
            //Remove as notificações
            //foreach (var notification in notifications)
            //{
            //    db.Notification.Remove(notification);
            //}
            //await db.SaveChangesAsync();
            return Ok(notifications.ToArray());
        }

        // PUT: api/Notification/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotification(int id, Notification Notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Notification.ID)
            {
                return BadRequest();
            }

            db.Entry(Notification).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Notification
        [ResponseType(typeof(Notification))]
        public IHttpActionResult PostNotification(Notification Notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Notification.Add(Notification);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Notification.ID }, Notification);
        }

        // DELETE: api/Notification/5
        [ResponseType(typeof(Notification))]
        public IHttpActionResult DeleteNotification(int id)
        {
            Notification Notification = db.Notification.Find(id);
            if (Notification == null)
            {
                return NotFound();
            }

            db.Notification.Remove(Notification);
            db.SaveChanges();

            return Ok(Notification);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationExists(int id)
        {
            return db.Notification.Count(e => e.ID == id) > 0;
        }
    }
}