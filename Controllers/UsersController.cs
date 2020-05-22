using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Text;
using Microsoft.Ajax.Utilities;

namespace Proyecto.Controllers
{
    public class UsersController : Controller
    {
        private DbContextUsers db = new DbContextUsers();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.Include(q => q.Roles).ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.Include(q => q.Roles).Where(q => q.Id == id).SingleOrDefaultAsync();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var roles = db.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Lastname,Phone,DateBirth,Password,Email")] User user, string[] Roles)
        {
            user.Roles = db.Roles.Where(q => Roles
                .Any(p => p == q.Id.ToString())).ToList();
            user.Password = Encriptar(user.Password);
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        private string Encriptar(string password)
        {
            SHA256 sha = SHA256.Create();
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            var pass = sha.ComputeHash(byteConverter.GetBytes(password));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < pass.Length; i++)
            {
                sBuilder.Append(pass[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.Include(q => q.Roles).SingleOrDefaultAsync(q => q.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var roles = await db.Roles.ToListAsync();
            user.SelectedRoles = user?.Roles.Select(q => q.Id).ToArray();
            ViewBag.Roles = roles;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Lastname,Phone,DateBirth,Password,Email")] User user, string[] SelectedRoles)
        {
            if (ModelState.IsValid)
            {
                using (var context = new DbContextUsers())
                {
                    var users = context.Users.Include(q => q.Roles).SingleOrDefault(q => q.Id == user.Id);
                    users.Roles.Clear();
                    context.SaveChanges();
                    var usuario = context.Users.SingleOrDefault(q => q.Id == user.Id);
                    var roles = context.Roles.Where(q => SelectedRoles.Any(a => a == q.Id.ToString())).ToList();
                    usuario.Roles = roles;
                    context.SaveChanges();
                }

                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
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
