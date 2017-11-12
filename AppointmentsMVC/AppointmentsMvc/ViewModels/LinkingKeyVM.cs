using AppointmentsDb.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentsMvc.ViewModels
{
    public class LinkingKeyVM
    {
        public List<LinkingKeyUiDto> LinkingKeys { get; set; }
        public CompanyUiDto CompanyUiDto { get; set; }
    }
}