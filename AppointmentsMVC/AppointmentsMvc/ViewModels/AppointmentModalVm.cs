using AppointmentsDb.Helpers;
using AppointmentsDb.ModelsDto;
using AppointmentsDb.ModelsDto.Custom;
using AppointmentsDb.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointmentsMvc.ViewModels
{
    public class AppointmentModalVm
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
        public IEnumerable<SelectListItem> ProfessionalsAccessableEmployeesDropDownModal;


        public DateTime ActiveDate { get; set; }


        public AppointmentForProUiDto Appointment { get; set; }


        public AppointmentModalVm(ICompanyQueries companyQueries, string userId, string professionalIdForAppointment, AppointmentForProUiDto appointment)
        {
            Guid proIdForAppt = GuidHelper.GetGuid(professionalIdForAppointment);
            _companyQueries = companyQueries;
            ProfessionalsAccessableEmployees = _companyQueries.GetAccessibleEmployeesForProfessional(userId, true, 0);
            Appointment = appointment;
            Appointment.ProfessionalId = proIdForAppt;

            if (ProfessionalsAccessableEmployees != null)
            {
                List<SelectListItem> sliList = new List<SelectListItem>();
                foreach(var row in ProfessionalsAccessableEmployees)
                {
                    SelectListItem itm = new SelectListItem();
                    itm.Selected = row.Id == proIdForAppt;
                    itm.Text = row.Forename + " " + row.Surname;
                    itm.Value = row.Id.ToString();
                    sliList.Add(itm);
                }
                ProfessionalsAccessableEmployeesDropDownModal = sliList.AsEnumerable();


                //ProfessionalsAccessableEmployeesDropDown = ProfessionalsAccessableEmployees.Select(i => new SelectListItem()
                //{
                //    Selected = (Appointment.ProfessionalId.ToString() == i.Id.ToString()),
                //    Text = i.Forename + " " + i.Surname,
                //    Value = i.Id.ToString()
                //});
            }

        }
    }
}