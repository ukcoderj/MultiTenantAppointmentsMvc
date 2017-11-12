using AppointmentsDb.Helpers;
using AppointmentsDb.ModelsDto.Custom;
using AppointmentsDb.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointmentsMvc.ViewModels
{
    public class CalendarVm
    {
        private ICompanyQueries _companyQueries;

        /// <summary>
        /// This is a list of other professionals this professional can view the calendar for.
        /// This professional can also set appointments for them.
        /// </summary>
        private List<AccessibleEmployee> ProfessionalsAccessableEmployees;


        /// <summary>
        /// The list of ProfessionalsAccessableEmployees in a format MVC can use for selection.
        /// </summary>
        public IEnumerable<SelectListItem> ProfessionalsAccessableEmployeesDropDown;

        public List<DateTime> AvailableDates { get; set; }

        public DateTime ActiveDate { get; set; }

        public Guid CurrentProfessionalId { get; set; }

        public CalendarVm(ICompanyQueries companyQueries, string userId, Guid? professionalIdOfUser, string professionalIdOfCalendar)
        {
            _companyQueries = companyQueries;
            ProfessionalsAccessableEmployees = _companyQueries.GetAccessibleEmployeesForProfessional(userId, true, 0);

            if(!String.IsNullOrWhiteSpace(professionalIdOfCalendar))
            {
                CurrentProfessionalId = GuidHelper.GetGuid(professionalIdOfCalendar);
            }
            else if(professionalIdOfUser != null)
            {
                CurrentProfessionalId = (Guid)professionalIdOfUser;
            }

            if (ProfessionalsAccessableEmployees != null)
            {
                List<SelectListItem> sliList = new List<SelectListItem>();
                foreach (var row in ProfessionalsAccessableEmployees)
                {
                    SelectListItem itm = new SelectListItem();
                    itm.Selected = row.IsDefault;
                    itm.Text = row.Forename + " " + row.Surname;
                    itm.Value = row.Id.ToString();
                    sliList.Add(itm);
                }
                ProfessionalsAccessableEmployeesDropDown = sliList.AsEnumerable();
            }

        }

    }


}