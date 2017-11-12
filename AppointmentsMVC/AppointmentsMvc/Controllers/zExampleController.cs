using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppointmentsDb.Models;

namespace AppointmentsMvc.Controllers
{
    public class zExampleController : Controller
    {
        private AppointmentsDbContext db = null;// new AppointmentsDbContext();

        // GET: zExample
        public async Task<ActionResult> Index()
        {
            return View(await db.Companies.ToListAsync());
        }

        // GET: zExample/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await db.Companies.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: zExample/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: zExample/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CompanyId,CompanyIndex,CompanyName,AddressLine1,AddressLine2,TownCity,County,Postcode,MainContactName,MainContactEmail,MainContactTel,SecondaryContactName,SecondaryContactEmail,SecondaryContactTel,IsApproved,ApprovedDate,BannedDate,BannedReason,Notes,ApiLiveKey,ApiTestKey,IsDeleted")] Company company)
        {
            if (ModelState.IsValid)
            {
                company.CompanyId = Guid.NewGuid();
                db.Companies.Add(company);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: zExample/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await db.Companies.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: zExample/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CompanyId,CompanyIndex,CompanyName,AddressLine1,AddressLine2,TownCity,County,Postcode,MainContactName,MainContactEmail,MainContactTel,SecondaryContactName,SecondaryContactEmail,SecondaryContactTel,IsApproved,ApprovedDate,BannedDate,BannedReason,Notes,ApiLiveKey,ApiTestKey,IsDeleted")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: zExample/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await db.Companies.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: zExample/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Company company = await db.Companies.FindAsync(id);
            db.Companies.Remove(company);
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
