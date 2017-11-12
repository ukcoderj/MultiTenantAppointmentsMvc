using AppointmentsDb.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppointmentsMvc.ViewModels
{
    public class ProfessionalWorkingHourVm : ValidationMsg
    {

        /// <summary>
        /// The list of all the locations the pro could add for their availability.
        /// </summary>
        public IEnumerable<SelectListItem> ProfessionalsAccessableCompanyLocationGroups;

        public ProfessionalWorkingHourUiDto ProfessionalWorkingHourUiDto { get; set; }


        public ProfessionalWorkingHourVm(ProfessionalWorkingHourUiDto professionalWorkingHourUiDto, List<CompanyLocationGroupUiDto> companyLocationGroups)
        {
            ProfessionalWorkingHourUiDto = professionalWorkingHourUiDto;

            if (companyLocationGroups != null)
            {
                companyLocationGroups = companyLocationGroups.OrderBy(i => i.LocationGroupName).ToList();

                List<SelectListItem> sliList = new List<SelectListItem>();
                foreach (var row in companyLocationGroups)
                {
                    SelectListItem itm = new SelectListItem();                    
                    itm.Text = row.LocationGroupName;
                    itm.Value = row.CompanyLocationGroupId.ToString();
                    itm.Selected = (itm.Value == ProfessionalWorkingHourUiDto.CompanyLocationGroupId.ToString());
                    sliList.Add(itm);
                }
                ProfessionalsAccessableCompanyLocationGroups = sliList.AsEnumerable();
            }
        }
    }
}