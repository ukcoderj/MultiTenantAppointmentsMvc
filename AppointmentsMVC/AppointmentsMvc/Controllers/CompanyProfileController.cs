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
using AppointmentsDb.Queries;
using AppointmentsDb.Pattern;
using AutoMapper;
using Microsoft.AspNet.Identity;
using AppointmentsDb.ModelsDto;

namespace AppointmentsMvc.Controllers
{
    [Authorize]
    public class CompanyProfileController : Controller
    {
        AppointmentsDbContext _context;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        CompanyQueries _companyQueries;
        ProfessionalQueries _professionalQueries;
        LinkingKeyQueries _linkingKeyQueries;

        public CompanyProfileController()
        {
            _context = new AppointmentsDb.Models.AppointmentsDbContext();
            _unitOfWork = new AppointmentsDb.Pattern.UnitOfWork(_context);
            _mapper = new AppointmentsDb.MapperStart.Automapper_Startup().StartAutomapper();
            _companyQueries = new CompanyQueries(_unitOfWork, _mapper);
            _professionalQueries = new ProfessionalQueries(_unitOfWork, _mapper);
            _linkingKeyQueries = new LinkingKeyQueries(_unitOfWork, _mapper);
        }


        // GET: CompanyProfile/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            var company = _companyQueries.GetUiDto_CompanyFromUserGuid(User.Identity.GetUserId());

            // If there's a pending key to link the pro to the company, do it.
            var specialKey = new Helpers.Cookies().GetCookieAndDelete(this, Helpers.Cookies.CompanyLinkingKeyCookieName);
            if (!string.IsNullOrWhiteSpace(specialKey))
            {
                var success = await Task.Run(() => _linkingKeyQueries.UseLinkingKey_AddProfessionalToCompany(User.Identity.GetUserId(), specialKey));
                return RedirectToAction("UseCompanyKeyComplete", "LK", new { isSuccess = success });
            }

            // If they are not the owner, don't allow them to edit.
            if (company != null && !company.UILoadOnly_IsUserCompanyOwner)
            {
                return RedirectToAction("Details", "CompanyProfile");
            }

            // new company
            if (company == null)
            {
                var pro = _professionalQueries.GetUiDto_ProfessionalFromUserGuid(User.Identity.GetUserId());
                company = new AppointmentsDb.ModelsDto.CompanyUiDto();
                company.MainContactEmail = pro.EmailAddress;
                company.MainContactName = pro.Honorific + " " + pro.Forename + " " + pro.Surname + " " + pro.Suffix;
            }

            return View(company);
        }


        // POST: CompanyProfile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CompanyId,CompanyIndex,CompanyName,AddressLine1,AddressLine2,TownCity,County,Postcode,MainContactName,MainContactEmail,MainContactTel,SecondaryContactName,SecondaryContactEmail,SecondaryContactTel,IsApproved,ApprovedDate,BannedDate,BannedReason,Notes,ApiLiveKey,ApiTestKey,IsDeleted")] CompanyUiDto company)
        {
            if (ModelState.IsValid)
            {
                // New company ownership set up in this method.
                _companyQueries.AddOrUpdateCompanyFromDto(User.Identity.GetUserId(), company);
                return RedirectToAction("Index", "Appointments");
            }
            else
            {
                return View(company);
            }
        }


        // GET: CompanyProfile/Details/5
        public async Task<ActionResult> Details()
        {
            var company = _companyQueries.GetUiDto_CompanyFromUserGuid(User.Identity.GetUserId());
            return View(company);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
