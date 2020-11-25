using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WSClass.API.Models;

namespace WSClass.API.Controllers
{
    public class NotificationsController : Controller
    {
        private TaskDatabaseEntities db = new TaskDatabaseEntities();

        // GET: Notifications
        public async Task<ActionResult> Index()
        {
            var notification = db.Notification.Include(n => n.Image).Include(n => n.User1);
            return View(await notification.ToListAsync());
        }

        // GET: Notifications/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = await db.Notification.FindAsync(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // GET: Notifications/Create
        public ActionResult Create()
        {
            ViewBag.Icon = new SelectList(db.Image, "ID", "ID");
            ViewBag.User = new SelectList(db.User, "ID", "FirstName");
            return View();
        }

        // POST: Notifications/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Title,Content,Icon,User")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Notification.Add(notification);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Icon = new SelectList(db.Image, "ID", "ID", notification.Icon);
            ViewBag.User = new SelectList(db.User, "ID", "FirstName", notification.User);
            return View(notification);
        }

        // GET: Notifications/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = await db.Notification.FindAsync(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            ViewBag.Icon = new SelectList(db.Image, "ID", "ID", notification.Icon);
            ViewBag.User = new SelectList(db.User, "ID", "FirstName", notification.User);
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Title,Content,Icon,User")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notification).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Icon = new SelectList(db.Image, "ID", "ID", notification.Icon);
            ViewBag.User = new SelectList(db.User, "ID", "FirstName", notification.User);
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = await db.Notification.FindAsync(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Notification notification = await db.Notification.FindAsync(id);
            db.Notification.Remove(notification);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
