using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AppointmentsDb.Queries;
using AutoMapper;
using AppointmentsDb.MapperStart;
using System.Data.Entity;
using AppointmentsDb.Pattern;
using AppointmentsMvc.ViewModels;

namespace AppointmentsMvc.Controllers
{
    public class HomeController : Controller
    {
        AppointmentsDb.Models.AppointmentsDbContext _context;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        ProfessionalQueries _professionalQueries;
        CompanyQueries _companyQueries;

        public HomeController()
        {
            _context = new AppointmentsDb.Models.AppointmentsDbContext();
            _unitOfWork = new AppointmentsDb.Pattern.UnitOfWork(_context);
            _mapper = new AppointmentsDb.MapperStart.Automapper_Startup().StartAutomapper();
            _professionalQueries = new ProfessionalQueries(_unitOfWork, _mapper);
            _companyQueries = new CompanyQueries(_unitOfWork, _mapper);
        }


        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Appointments", new { startDateString=DateTime.Now.ToString("yyyy-MM-dd") });
            }

            return View();
        }



        public ActionResult IndexAuth()
        {
            // If the Pro isn't set up, make them add the info.
            CompanyProfessionalVm companyPro = new CompanyProfessionalVm(_professionalQueries, _companyQueries, User.Identity.GetUserId());

            // We are logged in, but don't have the main info saved. 
            // Send them to the profile page.
            if (!companyPro.IsProfessionalValid())
            {
                return RedirectToAction("Edit", "ProfessionalProfile");
            }

            // If the Company isn't set up, make them add the info.
            if (!companyPro.IsCompanyInfoValid())
            {
                return RedirectToAction("Edit", "CompanyProfile");
            }


            return View(companyPro);

        }


        [Authorize]
        public ActionResult About()
        {
            

            // All is good. Carry on.
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}