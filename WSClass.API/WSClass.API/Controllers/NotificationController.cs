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
        public IQueryable<Notifications> GetNotifications()
        {
            return db.Notifications;
        }

        // GET: api/Notification/5
        [ResponseType(typeof(Notifications))]
        public async Task<IHttpActionResult> GetNotificationsAsync(string login_token,string valid_key)
        {
            try
            {
                await LoginController.ValidUserAsync(Guid.Parse(login_token),HttpContext.Current,valid_key);
            }
            catch (Exception)
            {
                return Unauthorized();
            }

            List<Notifications> notifications = await db.Notifications.Where(wh => wh.User1.ValidKey == valid_key).ToListAsync();
            if (notifications == null)
            {
                return NotFound();
            }

            return Ok(notifications);
        }

        // PUT: api/Notification/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotifications(int id, Notifications notifications)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notifications.ID)
            {
                return BadRequest();
            }

            db.Entry(notifications).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationsExists(id))
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
        [ResponseType(typeof(Notifications))]
        public IHttpActionResult PostNotifications(Notifications notifications)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notifications.Add(notifications);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = notifications.ID }, notifications);
        }

        // DELETE: api/Notification/5
        [ResponseType(typeof(Notifications))]
        public IHttpActionResult DeleteNotifications(int id)
        {
            Notifications notifications = db.Notifications.Find(id);
            if (notifications == null)
            {
                return NotFound();
            }

            db.Notifications.Remove(notifications);
            db.SaveChanges();

            return Ok(notifications);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationsExists(int id)
        {
            return db.Notifications.Count(e => e.ID == id) > 0;
        }
    }
}