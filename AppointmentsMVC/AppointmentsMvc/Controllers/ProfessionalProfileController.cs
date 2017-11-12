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
using Microsoft.AspNet.Identity;
using AutoMapper;
using AppointmentsDb.Pattern;
using AppointmentsDb.Queries;
using AppointmentsDb.ModelsDto;

namespace AppointmentsMvc.Controllers
{
    [Authorize]
    public class ProfessionalProfileController : Controller
    {
        AppointmentsDbContext _context;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        ProfessionalQueries _professionalQueries;
        LinkingKeyQueries _linkingKeyQueries;

        public ProfessionalProfileController()
        {
            _context = new AppointmentsDb.Models.AppointmentsDbContext();
            _unitOfWork = new AppointmentsDb.Pattern.UnitOfWork(_context);
            _mapper = new AppointmentsDb.MapperStart.Automapper_Startup().StartAutomapper();
            _professionalQueries = new ProfessionalQueries(_unitOfWork, _mapper);
            _linkingKeyQueries = new LinkingKeyQueries(_unitOfWork, _mapper);
        }


        public async Task<ActionResult> Edit()
        {
            var professional = _professionalQueries.GetUiDto_ProfessionalFromUserGuid(User.Identity.GetUserId());

            if (professional == null)
            {
                // Create the professional. If there's a linking key, don't worry about it for now.
                professional = new ProfessionalUiDto();
                professional.ProfessionalId = new Guid();
                professional.ProfessionalUserId = AppointmentsDb.Helpers.GuidHelper.GetGuid(User.Identity.GetUserId());
                professional.EmailAddress = User.Identity.Name;
            }
            else
            {
                // If there's a pending key to link the pro to the company, do it.
                var specialKey = new Helpers.Cookies().GetCookieAndDelete(this, Helpers.Cookies.CompanyLinkingKeyCookieName);
                if (!string.IsNullOrWhiteSpace(specialKey))
                {
                    var success = await Task.Run(() => _linkingKeyQueries.UseLinkingKey_AddProfessionalToCompany(User.Identity.GetUserId(), specialKey));
                    return RedirectToAction("UseCompanyKeyComplete", "LK", new { isSuccess = success });
                }
            }

            return View(professional);
        }

        // POST: ProfessionalProfile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProfessionalId,ProfessionalIndex,ProfessionalUserId,Honorific,Forename,MiddleName,Surname,Suffix,Gender,EmailAddress,Telephone,TelephoneMobile,IsAvailableForAppointments")] ProfessionalUiDto professional)
        {
            if (ModelState.IsValid)
            {
                // DONT LET ANYONE CHANGE THEIR EMAIL ADDRESS THEMSELVES. WE WANT THIS TO BE THE SAME AS THE LOGIN.
                professional.EmailAddress = User.Identity.Name;

                _professionalQueries.AddOrUpdateProfessionalFromDto(User.Identity.GetUserId(), professional);
                return RedirectToAction("Index", "Appointments");
            }
            else
            {
                return View(professional);
            }            
        }



        // GET: ProfessionalProfile
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.Professionals.ToListAsync());
        //}

        // GET: ProfessionalProfile/Details/5
        public async Task<ActionResult> Details()
        {
            var professional = _professionalQueries.GetUiDto_ProfessionalFromUserGuid(User.Identity.GetUserId());
            return View(professional);
        }



        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
