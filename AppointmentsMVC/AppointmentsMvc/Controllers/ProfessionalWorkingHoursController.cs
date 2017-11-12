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
using AutoMapper;
using AppointmentsDb.Pattern;
using AppointmentsDb.Queries;
using Shared.Enums;
using Microsoft.AspNet.Identity;
using AppointmentsMvc.ViewModels;

namespace AppointmentsMvc.Controllers
{
    [Authorize]
    public class ProfessionalWorkingHoursController : Controller
    {
        AppointmentsDbContext _context;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        IAccessQueries _accessQueries;
        ICompanyQueries _companyQueries;
        ICompanyLocationGroupQueries _companyLocationGroupQueries;
        IProfessionalWorkingHoursQueries _professionalWorkingHoursQueries;


        public ProfessionalWorkingHoursController()
        {
            _context = new AppointmentsDb.Models.AppointmentsDbContext();
            _unitOfWork = new AppointmentsDb.Pattern.UnitOfWork(_context);
            _mapper = new AppointmentsDb.MapperStart.Automapper_Startup().StartAutomapper();

            _companyQueries = new CompanyQueries(_unitOfWork, _mapper);
            _accessQueries = new AccessQueries(_unitOfWork, _companyQueries, _mapper);

            _companyLocationGroupQueries = new CompanyLocationGroupQueries(_unitOfWork, _mapper, _accessQueries, _companyQueries);
            _professionalWorkingHoursQueries = new ProfessionalWorkingHoursQueries(_unitOfWork, _mapper, _accessQueries, _companyQueries, _companyLocationGroupQueries);
        }




        // GET: ProfessionalWorkingHours
        public async Task<ActionResult> Index()
        {
            AuthorizationState ignoreAuthstate = AuthorizationState.NotAllowed;

            var proWorkingHoursDtos = await Task.Run(() => _professionalWorkingHoursQueries.GetUiDto_ProfessionalWorkingHoursFromUserGuid(User.Identity.GetUserId()));
            var companyLocationGroupUiDtos = await Task.Run(() => _companyLocationGroupQueries.GetUiDto_CompanyLocationGroups(User.Identity.GetUserId(), out ignoreAuthstate));

            List<ProfessionalWorkingHourVm> workingHourVms = new List<ProfessionalWorkingHourVm>();

            if(proWorkingHoursDtos != null)
            {
                foreach (var row in proWorkingHoursDtos)
                {
                    ProfessionalWorkingHourVm vm = new ProfessionalWorkingHourVm(row, companyLocationGroupUiDtos);
                    workingHourVms.Add(vm);
                }
            }
            
            return View(workingHourVms);
        }


        // REMEMBER TO VALIDATE START < END Date!
        // LOOK INTO OVERPOSTING PROTECTION.
        // Cancel to post back to same page?

        // See CompanyLocationGroupsController example for shared razor and saves.










        // GET: ProfessionalWorkingHours/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessionalWorkingHour professionalWorkingHour = null;
            if (professionalWorkingHour == null)
            {
                return HttpNotFound();
            }
            return View(professionalWorkingHour);
        }

        // GET: ProfessionalWorkingHours/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfessionalWorkingHours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProfessionalWorkingHourId,ProfessionalWorkingHourIndex,DayOfWeek,StartTime,EndTime,IncludeBankHolidays,CreatedByProfessionalId,CreatedDate,UpdatedByProfessionalId,UpdatedDate,IsDeleted,DeletedByProfessionalId,DeletedDate")] ProfessionalWorkingHour professionalWorkingHour)
        {
            if (ModelState.IsValid)
            {
                professionalWorkingHour.ProfessionalWorkingHourId = Guid.NewGuid();
                //db.ProfessionalWorkingHours.Add(professionalWorkingHour);
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(professionalWorkingHour);
        }

        // GET: ProfessionalWorkingHours/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessionalWorkingHour professionalWorkingHour = null;
            if (professionalWorkingHour == null)
            {
                return HttpNotFound();
            }
            return View(professionalWorkingHour);
        }

        // POST: ProfessionalWorkingHours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProfessionalWorkingHourId,ProfessionalWorkingHourIndex,DayOfWeek,StartTime,EndTime,IncludeBankHolidays,CreatedByProfessionalId,CreatedDate,UpdatedByProfessionalId,UpdatedDate,IsDeleted,DeletedByProfessionalId,DeletedDate")] ProfessionalWorkingHour professionalWorkingHour)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(professionalWorkingHour).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(professionalWorkingHour);
        }

        // GET: ProfessionalWorkingHours/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessionalWorkingHour professionalWorkingHour = null;
            if (professionalWorkingHour == null)
            {
                return HttpNotFound();
            }
            return View(professionalWorkingHour);
        }

        // POST: ProfessionalWorkingHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            //ProfessionalWorkingHour professionalWorkingHour = await db.ProfessionalWorkingHours.FindAsync(id);
            //db.ProfessionalWorkingHours.Remove(professionalWorkingHour);
            //await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
