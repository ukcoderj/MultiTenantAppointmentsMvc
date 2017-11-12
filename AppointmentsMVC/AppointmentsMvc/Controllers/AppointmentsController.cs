using AppointmentsDb.ModelsDto;
using AppointmentsDb.Pattern;
using AppointmentsDb.Queries;
using AppointmentsMvc.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AppointmentsMvc.Controllers
{

    [Authorize]
    public class AppointmentsController : Controller
    {
        AppointmentsDb.Models.AppointmentsDbContext _context;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;

        IAccessQueries _accessQueries;
        IAppointmentQueries _appointmentQueries;
        ICompanyQueries _companyQueries;
        IProfessionalQueries _professionalQueries;
        

        public AppointmentsController()
        {
            _context = new AppointmentsDb.Models.AppointmentsDbContext();
            _unitOfWork = new AppointmentsDb.Pattern.UnitOfWork(_context);
            _mapper = new AppointmentsDb.MapperStart.Automapper_Startup().StartAutomapper();

            _companyQueries = new CompanyQueries(_unitOfWork, _mapper);
            _accessQueries = new AccessQueries(_unitOfWork, _companyQueries, _mapper);
            _appointmentQueries = new AppointmentQueries(_unitOfWork, _mapper, _accessQueries);
            
            _professionalQueries = new ProfessionalQueries(_unitOfWork, _mapper);
            
        }



        // GET: Appointments
        public ActionResult Index(string startDateString, string activeDateString, string professionalIdString)
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


            // Get the appointments with a list of employees details this user can access
            CalendarVm calendarVm = new CalendarVm(_companyQueries, User.Identity.GetUserId(), companyPro.GetProfessionalId(), professionalIdString);
            var startDate = GetStartDateFromStringParam(startDateString);
            var activeDate = GetStartDateFromStringParam(activeDateString);

            if (startDate < DateTime.Now) startDate = DateTime.Now;
            if (activeDate < startDate) activeDate = startDate;

            if (activeDate.Date > startDate.Date.AddDays(3))
            {
                startDate = activeDate.AddDays(-2);
            }

            calendarVm.AvailableDates = CreateAvailableDatesList(startDate);
            calendarVm.ActiveDate = activeDate;

            return View(calendarVm);
        }



        public JsonResult GetAppointmentsForDate(string apptDate, string professionalId)
        {
            DateTime dt;
            DateTime.TryParse(apptDate, out dt);
            if (dt == new DateTime())
                dt = DateTime.Now.Date;

            List<AppointmentForProUiDto> appointmentsList = _appointmentQueries.GetUiDto_AppointmentsForProfessionalOrEmployee(User.Identity.GetUserId(), professionalId, dt.Date, dt.Date.AddDays(1));

            return Json(appointmentsList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditModalNew(string startDateStr, int startHour, string professionalId)
        {
            DateTime startDate = DateTime.Parse(startDateStr);
            DateTime appointmentStart = startDate.AddHours(startHour);

            var appointment = new AppointmentForProUiDto(appointmentStart);
            appointment.DurationInMinutes = 60;
            appointment.Forename = "";
            appointment.Surname = "";
            appointment.TelephoneMobile = "";

            AppointmentModalVm appointmentVm = new AppointmentModalVm(_companyQueries, User.Identity.GetUserId(), professionalId, appointment);

            return PartialView("_editAppointmentPartial", appointmentVm);
        }


        public ActionResult EditModal(Guid? id, string professionalId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get the appointment for the professional.
            AppointmentForProUiDto appointment = _appointmentQueries.GetUiDto_AppointmentByIdForProfessionalOrEmployee(User.Identity.GetUserId(), professionalId, (Guid)id);
            
            if (appointment == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AppointmentModalVm appointmentVm = new AppointmentModalVm(_companyQueries, User.Identity.GetUserId(), professionalId, appointment);

            return PartialView("_editAppointmentPartial", appointmentVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModal([Bind(Include = "ProfessionalId, AppointmentId,AppointmentIndex,AppointmentType,Forename,Surname,EmailAddress,Telephone,TelephoneMobile,DurationInMinutes,IsCancelled,ProfessionalNotes,StartTime_Date,StartTime_Hour,StartTime_Minutes")] AppointmentForProUiDto appointment)
        {
            if (ModelState.IsValid)
            {
                _appointmentQueries.AddOrUpdateAppointmentFromDto(User.Identity.GetUserId(), appointment.ProfessionalId.ToString(), appointment);

                return Content("success");
            }
            else
            {
                // List the errors.
                StringBuilder sbError = new StringBuilder();
                foreach (var row in ModelState.Values)
                {
                    foreach (var err in row.Errors)
                    {
                        sbError.AppendLine(err.ErrorMessage);
                    }
                }
                return Content(sbError.ToString());
            }

        }



        #region "Helpers"

        DateTime GetStartDateFromStringParam(string dateString)
        {
            if (String.IsNullOrWhiteSpace(dateString))
            {
                dateString = DateTime.Now.Date.ToString();
            }

            DateTime dt;
            if (DateTime.TryParse(dateString, out dt))
            {
                return dt;
            }

            return DateTime.Now;

        }

        /// <summary>
        /// Return a list of dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        List<DateTime> CreateAvailableDatesList(DateTime startDate)
        {
            var numberOfDaysToShow = 31;
            List<DateTime> availableDatesList = new List<DateTime>();
            for (int i = 0; i < numberOfDaysToShow; i++)
            {
                availableDatesList.Add(startDate.Date.AddDays(i));
            }

            return availableDatesList;
        }

        #endregion





    }
}