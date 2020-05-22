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
using System.Globalization;
using WebGrease.Css.Extensions;

namespace Proyecto.Controllers
{
    public class RolesController : Controller
    {
        private DbContextUsers db = new DbContextUsers();

        // GET: Roles
        public async Task<ActionResult> Index()
        {
            var data = await (from role in db.Roles
                              select new
                              {
                                  role,
                                  role.Permissions
                              }).ToListAsync();
            var respuesta = data.Select(q =>
             new Role
             {
                 Descripcion = q.role.Descripcion,
                 Permissions = q.Permissions,
                 Id = q.role.Id,
                 Nombre = q.role.Nombre
             }).ToList();
            return View(respuesta);
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = await (from role in db.Roles
                        from permision in role.Permissions
                        from permisos in db.Permisos
                        where role.Id == id && permisos.Id == permision.Id
                        select new
                        {
                            role,
                            permisos
                        })
                        .ToListAsync();
            var resultado = data.GroupBy(q => q.role.Id)
                .Select(q => new Role
                {
                    Nombre = q.FirstOrDefault().role.Nombre,
                    Descripcion = q.FirstOrDefault().role.Descripcion,
                    Id = q.Key,
                    Permissions = q.Select(p => p.permisos).ToList()
                }).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(resultado);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            ViewBag.Permisos = db.Permisos.ToList();
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Descripcion,Permisos")] Role role, string[] Permisos)
        {

            if (ModelState.IsValid)
            {
                role.Permissions = db.Permisos.Where(q => Permisos
                .Any(p => p == q.Id.ToString()))
                    .ToList();
                db.Roles.Add(role);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.Roles.Include(q => q.Permissions).SingleOrDefault(q => q.Id == id);
            data.SelectedValues = data?.Permissions.Select(q => q.Id).ToArray();
            ViewBag.Permisos = db.Permisos.ToList();
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Descripcion")] Role role, string[] SelectedValues)
        {
            if (ModelState.IsValid)
            {
                using (var context = new DbContextUsers())
                {
                    var rolD = context.Roles.Include(q => q.Permissions).SingleOrDefault(q => q.Id == role.Id);
                    rolD.Permissions.Clear();
                    context.SaveChanges();
                    var rol = context.Roles.SingleOrDefault(q => q.Id == role.Id);
                    var permisos = context.Permisos.Where(q => SelectedValues.Any(a => a == q.Id.ToString())).ToList();
                    rol.Permissions = permisos;
                    context.SaveChanges();
                }

                db.Entry(role).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await db.Roles.FindAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Role role = await db.Roles.FindAsync(id);
            db.Roles.Remove(role);
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
