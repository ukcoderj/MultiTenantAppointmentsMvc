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
using AppointmentsDb.ModelsDto;
using AutoMapper;
using AppointmentsDb.Pattern;
using AppointmentsDb.Queries;
using Microsoft.AspNet.Identity;
using AppointmentsMvc.ViewModels;
using System.Configuration;

namespace AppointmentsMvc.Controllers
{
    [Authorize]
    public class LKController : Controller
    {
        AppointmentsDbContext _context;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        ProfessionalQueries _professionalQueries;
        CompanyQueries _companyQueries;
        LinkingKeyQueries _linkingKeyQueries;
        

        public LKController()
        {
            _context = new AppointmentsDb.Models.AppointmentsDbContext();
            _unitOfWork = new AppointmentsDb.Pattern.UnitOfWork(_context);
            _mapper = new AppointmentsDb.MapperStart.Automapper_Startup().StartAutomapper();
            _companyQueries = new CompanyQueries(_unitOfWork, _mapper);
            _professionalQueries = new ProfessionalQueries(_unitOfWork, _mapper);
            _linkingKeyQueries = new LinkingKeyQueries(_unitOfWork, _mapper);
        }

        /// <summary>
        /// View all keys generated
        /// </summary>
        /// <returns></returns>
        // GET: LK        
        public async Task<ActionResult> Index()
        {
            LinkingKeyVM lkvm = new LinkingKeyVM();

            lkvm.LinkingKeys = await Task.Run(() => _linkingKeyQueries.GetLinkingKeysForProfessional(User.Identity.GetUserId()));

            string thisUrl = ConfigurationManager.AppSettings["FullDeploymentUrl"];

            foreach (var row in lkvm.LinkingKeys)
            {
                row.SpecialKey = thisUrl + "/LK/UseCompanyKeyConfirmation/" + row.SpecialKey;
            }

            lkvm.CompanyUiDto = await Task.Run(() => _companyQueries.GetUiDto_CompanyFromUserGuid(User.Identity.GetUserId()));

            if(!lkvm.CompanyUiDto.UILoadOnly_IsUserCompanyOwner)
            {
                return RedirectToAction("Index", "UserCannotAccess");
            }


            lkvm.LinkingKeys = lkvm.LinkingKeys.OrderByDescending(i => i.CreatedDateTime).ToList();

            return View(lkvm);
        }



        #region "Linking Professional To A Company"

        /// <summary>
        /// A page where a key can be generated to add a professional to your company.
        /// </summary>
        /// <returns></returns>
        // GET: LK/Create
        public async Task<ActionResult> CreateCompanyProKey()
        {
            // Additional validation in case someone tries to guess the url.
            var companyOwner = await Task.Run(() => _companyQueries.GetCompanyFromOwnerUserGuid(User.Identity.GetUserId()));
            if (companyOwner == null)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        /// <summary>
        /// Create a key that will link a professional to your company.
        /// </summary>
        /// <param name="linkingKey"></param>
        /// <returns></returns>
        // POST: LK/Create
        // Creates a new linking key
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCompanyProKey([Bind(Include = "AvailableForEmailAddress")] LinkingKeyUiDto linkingKey)
        {
            var propertyToValidate = ModelState["AvailableForEmailAddress"];

            if (propertyToValidate != null && !propertyToValidate.Errors.Any())
            {
                var expiry = DateTime.Now.AddDays(3);
                _linkingKeyQueries.CreateLinkingKey_AddProfessionalToCompany(User.Identity.GetUserId(), linkingKey.AvailableForEmailAddress, expiry);

                return RedirectToAction("Index");
            }

            return View(linkingKey);
        }

        /// <summary>
        /// When the user click the link to join the company, show a confirm dialog.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> UseCompanyKeyConfirmation(string id)
        {
            var companyRow = _linkingKeyQueries.GetUiDto_LinkingKey_GetCompany(id);

            LinkingKeyVM vm = new LinkingKeyVM();
            vm.CompanyUiDto = companyRow;
            vm.LinkingKeys = new List<LinkingKeyUiDto>() { new LinkingKeyUiDto() { SpecialKey = id } };

            return View(vm);
        }


        /// <summary>
        /// The user definitely wants to join the company. Set a cookie if they're not logged in.
        /// NOTE: The professional edit and company edit pages will look for the cookie and act accordingly. 
        ///       Other pages will push the user through those pages if details are not complete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> UseCompanyKeyConfirmed(string id)
        {
            Company company = null;            
            if (User != null && User.Identity != null && User.Identity.GetUserId() != null)
            {
                company = await Task.Run(() => _companyQueries.GetCompanyAndThisEmployeeFromEmployeeProfessionalUserId(User.Identity.GetUserId()));
            }

            // If the professional does not exist, set a cookie for when they do and redirect them to set up!
            // Else, complete the process. 
            // NOTE: When the Pro has linked at Professional or Company Stage of setup, 
            //       they will be redirected back here to handle the setup.
            if (company == null)
            {
                // save something for connecting the pro after creation.
                new Helpers.Cookies().SetCookieValue(this, Helpers.Cookies.CompanyLinkingKeyCookieName, id, DateTime.Now.AddMinutes(20));

                return RedirectToAction("Edit", "ProfessionalProfile");
            }
            else
            {
                var success = await Task.Run(() => _linkingKeyQueries.UseLinkingKey_AddProfessionalToCompany(User.Identity.GetUserId(), id));
                return RedirectToAction("UseCompanyKeyComplete", "LK", new { isSuccess = success });
            }
        }

        /// <summary>
        /// Just a message to the user to inform them of the status.
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <returns></returns>
        public async Task<ActionResult> UseCompanyKeyComplete(bool isSuccess)
        {
            StatusVM status = new StatusVM();
            status.IsSuccess = isSuccess;


            status.SuccessMessage = isSuccess ? "You have successfully joined. You can now create appointments :)." : "";
            status.FailMessage = isSuccess ? "" : "An error occurred when assigning to the company. Please contact the company owner and try again. If the problem persists, please contact support.";

            return View(status);
        }


        #endregion


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
